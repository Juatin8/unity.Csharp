using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class TryMyBlueToothHandlernew : MonoBehaviour
{
	//TODO:
	//用IEnumerator来实现

	[System.Serializable]
	public class DeviceClass
	{
		public string ControllerName;
		public string DeviceName;
		public string ServiceUUID;
		public string Characteristic;
		public string _hm10;
		public bool gotHm;
		public bool connected;
		public bool requestedMTU;
		public bool subscribed;
	}

	[SerializeField]
	public List<DeviceClass> DeviceList;


    struct SwitchState
    {
        public SwitchState(bool _state)
        {
            ifInitCalled = false;
            ifScanCalled = false;
            ifConnectCalled = false;
            ifRequestMtuCalled = false;
            ifSubscribeCalled = false;
            ifAllDone = false;
        }
        public bool ifInitCalled;
        public bool ifScanCalled;
        public bool ifConnectCalled;
        public bool ifRequestMtuCalled;
        public bool ifSubscribeCalled;
        public bool ifAllDone;
    }

	public TMP_Text blueToothConnectState;

    private SwitchState switchState;

	private GameObject _crow;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(StartProcess());
	}

    private IEnumerator StartProcess()
    {
		blueToothConnectState.text = "蓝牙连接中...";
        Reset();
        yield return StartCoroutine(CallInitCor());
        for (int i = 0; i < DeviceList.Count; i++)
        {
            yield return StartCoroutine(CallScan(i));
            BluetoothLEHardwareInterface.StopScan(); //停止扫描
            yield return StartCoroutine(CallConnect(i));
            yield return StartCoroutine(CallRequestMtu(i));
            yield return StartCoroutine(CallSubscribe(i));
        }
        blueToothConnectState.text = "连接完成";
    }

    private void Reset ()
	{	
        ResetDeviceList();
		switchState = new SwitchState(false);
	}

	private void ResetDeviceList()
	{
		foreach (var item in DeviceList)
		{
			item._hm10 = null;
			item.gotHm = false;
			item.connected = false;
			item.requestedMTU = false;
			item.subscribed = false;
		}
	}

    private IEnumerator CallInitCor()
    {
        BluetoothLEHardwareInterface.Initialize (true, false, () => {
             switchState.ifInitCalled = true;
		}, (error) => {
		    BluetoothLEHardwareInterface.Log ("Error: " + error);
		});

        while(!switchState.ifInitCalled){
            Debug.Log("My ble initing");
            yield return new WaitForSeconds(0.05f);
        }
        
    }

    private IEnumerator CallScan(int _index)
    {
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {
			if (name.Contains (DeviceList[_index].DeviceName))
			{
   			    DeviceList[_index]._hm10 = address;
				DeviceList[_index].gotHm = true;
   			}
		}, null, false, false);

        while(!DeviceList[_index].gotHm){
            Debug.Log("My ble scanning: " + _index);
            yield return new WaitForSeconds(0.1f);
        }
    }


    private IEnumerator CallConnect(int _index)
    {
		BluetoothLEHardwareInterface.ConnectToPeripheral (DeviceList[_index]._hm10, null, null, (address, serviceUUID, characteristicUUID) => {
    
    		if (IsEqual (serviceUUID, DeviceList[_index].ServiceUUID))
    		{
    			if (IsEqual (characteristicUUID, DeviceList[_index].Characteristic))
    			{
    				DeviceList[_index].connected = true;
    			}
    		}

    	}, (disconnectedAddress) => {
    		BluetoothLEHardwareInterface.Log ("Device disconnected: " + disconnectedAddress);
    	});

        while(!DeviceList[_index].connected)
        {
            Debug.Log("My ble connecting: " + _index);
            yield return new WaitForSeconds(0.1f);
        }
    }


    private IEnumerator CallRequestMtu(int _index)
    {
        BluetoothLEHardwareInterface.RequestMtu(DeviceList[_index]._hm10, 185, (address, newMTU) =>
		{
			DeviceList[_index].requestedMTU = true;
		});

        while(!DeviceList[_index].requestedMTU)
        {
            Debug.Log("My ble requesting MTU: " + _index);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator CallSubscribe(int _index)
    {
		BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (DeviceList[_index]._hm10, DeviceList[_index].ServiceUUID, DeviceList[_index].Characteristic, null, (address, characteristicUUID, bytes) => {
            var q0 = (float)((Int16)(bytes[9] << 8 | bytes[8]))/32768;
            var q1 = (float)((Int16)(bytes[11] << 8 | bytes[10]))/32768;
            var q2 = (float)((Int16)(bytes[13] << 8 | bytes[12]))/32768;
            var q3 = (float)((Int16)(bytes[15] << 8 | bytes[14]))/32768;

            Quaternion _q = Quaternion.identity;
            _q[0] = q1;
            _q[1] = q2;
            _q[2] = q3;
            _q[3] = q0;
                       
            _q = TransformedQuaternion(_q);

			_crow = GameObject.FindGameObjectWithTag("Crow");
			if(_crow != null){
			    if(DeviceList[_index].ControllerName == "OldBalanceBall"){
					if(GameManager.Instance.cooperationMode == 0)
			    	    _crow.GetComponent<PlayerController>().ControlPlayerMovement(_q);
					else if(GameManager.Instance.cooperationMode == 1)
					    _crow.GetComponent<PlayerController>().ControlPlayerVerMovement(_q, 10); //因为平衡球幅度小, 所以range应该小
			    }else if(DeviceList[_index].ControllerName == "YoungBalanceBall"){
					if(GameManager.Instance.cooperationMode != 0)
			    	    _crow.GetComponent<PlayerController>().ControlPlayerHorMovement(_q);
			    }else if(DeviceList[_index].ControllerName == "HandMotion"){
					if(GameManager.Instance.cooperationMode == 2)
			    	    _crow.GetComponent<PlayerController>().ControlPlayerVerMovement(_q, 90); //因为手的摆动幅度大，所以range应该大
			    }
			}
            

            DeviceList[_index].subscribed = true;
		});
        yield return new WaitForSeconds(0.1f);
    }

    private Quaternion TransformedQuaternion(Quaternion _q)
    {
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        _q.ToAngleAxis(out angle, out axis);
        Vector3 newAxis = new Vector3();

		//按照如下的方式，换z 和 y 时，传感器沿着自身的轴是固定的
		newAxis = new Vector3(axis.x, axis.z, axis.y);

        return Quaternion.AngleAxis(angle, newAxis);
    }

	string FullUUID (string uuid)
	{
		return "0000" + uuid + "-0000-1000-8000-00805F9B34FB";
	}
	
	bool IsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = FullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = FullUUID (uuid2);

		return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
	}

}
