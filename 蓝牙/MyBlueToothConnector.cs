using UnityEngine;
using System;

public class MyBlueToothConnector: MonoBehaviour
{
	//本代码用于连接蓝牙
	
	public string OldDeviceName = "DSD TECH";
	public string OldServiceUUID = "FFE0";
	public string OldCharacteristic = "FFE1";

    public string YoungDeviceName = "DSD TECH";
	public string YoungServiceUUID = "FFE0";
	public string YoungCharacteristic = "FFE1";

	private GameObject _crow;

    public PlayerController playerController;

    private bool sentFirst = false;

	public ModularTerrainCameraControl modularTerrainCameraControl;

	public 

	enum States
	{
		None,
		ScanOld,
        ScanYoung,
		ConnectOld,
        ConnectYoung,
		RequestOldMTU,
        RequestYoungMTU,
		SubscribeOld,
        SubscribeYoung,
		Unsubscribe,
		Disconnect,
		Communication,
	}

	private bool _workingFoundDevice = true;
	private bool _connected = false;
	private float _timeout = 0f;
	private States _state = States.None;
	private bool _foundID = false;

	// this is our hm10 device
	private string old_hm10;
    private string young_hm10;

	private static bool created = false;
    void Awake()
    {
        // Ensure the script is not deleted while loading.
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
		    sentFirst = false;
		    StartProcess ();
        }
        else
        {
            Destroy(this.gameObject);
        }

    }


	void Reset ()
	{
		_workingFoundDevice = false;	// used to guard against trying to connect to a second device while still connecting to the first
		_connected = false;
		_timeout = 0f;
		_state = States.None;
		_foundID = false;
		old_hm10 = null;
        young_hm10 = null;
	}

	void SetState (States newState, float timeout)
	{
		_state = newState;
		_timeout = timeout;
	}

	void StartProcess ()
	{

		Reset ();
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
			//ble_state.text = "init";
			SetState (States.ScanOld, 0.1f);

		}, (error) => {
			//ble_state.text = "error";
			BluetoothLEHardwareInterface.Log ("Error: " + error);
		});
	}

	// Use this for initialization
	void Start ()
	{
        // sentFirst = false;
		// StartProcess ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_timeout > 0f)
		{
			_timeout -= Time.deltaTime;
			if (_timeout <= 0f)
			{
				_timeout = 0f;

				switch (_state)
				{
				case States.None:
					break;

				case States.ScanOld:

					BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {
						//ble_state.text = "scan old";
						// we only want to look at devices that have the name we are looking for
						// this is the best way to filter out devices
						if (name.Contains (OldDeviceName))
						{
							_workingFoundDevice = true;

							// it is always a good idea to stop scanning while you connect to a device
							// and get things set up
							BluetoothLEHardwareInterface.StopScan ();

							// add it to the list and set to connect to it
							old_hm10 = address;

							SetState (States.ScanYoung, 0.1f);

							_workingFoundDevice = false;
						}

					}, null, false, false);
					break;
                
                case States.ScanYoung:

					BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {
						//ble_state.text = "scan young";
						// we only want to look at devices that have the name we are looking for
						// this is the best way to filter out devices
						if (name.Contains (YoungDeviceName))
						{
							_workingFoundDevice = true;

							// it is always a good idea to stop scanning while you connect to a device
							// and get things set up
							BluetoothLEHardwareInterface.StopScan ();

							// add it to the list and set to connect to it
							young_hm10 = address;

							SetState (States.ConnectOld, 0.1f);

							_workingFoundDevice = false;
						}

					}, null, false, false);
					break;

				case States.ConnectOld:
					// set these flags
					_foundID = false;


					// note that the first parameter is the address, not the name. I have not fixed this because
					// of backwards compatiblity.
					// also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
					// the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
					// large enough that it will be finished enumerating before you try to subscribe or do any other operations.
					BluetoothLEHardwareInterface.ConnectToPeripheral (old_hm10, null, null, (address, serviceUUID, characteristicUUID) => {
						//ble_state.text = "connect old";
						if (IsEqual (serviceUUID, OldServiceUUID))
						{
							// if we have found the characteristic that we are waiting for
							// set the state. make sure there is enough timeout that if the
							// device is still enumerating other characteristics it finishes
							// before we try to subscribe
							if (IsEqual (characteristicUUID, OldCharacteristic))
							{
								_connected = true;

								SetState (States.ConnectYoung, .1f);

							}
						}
					}, (disconnectedAddress) => {
						BluetoothLEHardwareInterface.Log ("Device disconnected: " + disconnectedAddress);
					});
					break;

                    case States.ConnectYoung:
					// set these flags
					_foundID = false;


					// note that the first parameter is the address, not the name. I have not fixed this because
					// of backwards compatiblity.
					// also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
					// the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
					// large enough that it will be finished enumerating before you try to subscribe or do any other operations.
					BluetoothLEHardwareInterface.ConnectToPeripheral (young_hm10, null, null, (address, serviceUUID, characteristicUUID) => {
						//ble_state.text = "connect young";
						if (IsEqual (serviceUUID, YoungServiceUUID))
						{
							// if we have found the characteristic that we are waiting for
							// set the state. make sure there is enough timeout that if the
							// device is still enumerating other characteristics it finishes
							// before we try to subscribe
							if (IsEqual (characteristicUUID, YoungCharacteristic))
							{
								_connected = true;

								SetState (States.RequestOldMTU, .5f);

							}
						}
					}, (disconnectedAddress) => {
						BluetoothLEHardwareInterface.Log ("Device disconnected: " + disconnectedAddress);
					});
					break;

					case States.RequestOldMTU:

						BluetoothLEHardwareInterface.RequestMtu(old_hm10, 185, (address, newMTU) =>
						{
							//ble_state.text = "Requested old MTU";
							SetState(States.RequestYoungMTU, 0.1f);
						});
						break;
                    
                    case States.RequestYoungMTU:

						BluetoothLEHardwareInterface.RequestMtu(young_hm10, 185, (address, newMTU) =>
						{
							//ble_state.text = "Requested young MTU";
							//modularTerrainCameraControl.SetSpeed();
							SetState(States.SubscribeOld, 0.1f);
						});
						break;

					case States.SubscribeOld:

					BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (old_hm10, OldServiceUUID, OldCharacteristic, null, (address, characteristicUUID, bytes) => {
						var Q0 = (float)((Int16)(bytes[9] << 8 | bytes[8]))/32768;
						var Q1 = (float)((Int16)(bytes[11] << 8 | bytes[10]))/32768;
						var Q2 = (float)((Int16)(bytes[13] << 8 | bytes[12]))/32768;
						var Q3 = (float)((Int16)(bytes[15] << 8 | bytes[14]))/32768;
                        Quaternion q = new Quaternion();
                        q[0] = Q1;
                        q[1] = Q2;
                        q[2] = Q3;
                        q[3] = Q0;
                        if(!sentFirst){
                            //playerController.InitDeviceSetting(q);
                            sentFirst = true;
                        }else{
							_crow = GameObject.FindGameObjectWithTag("Crow");
							if(_crow != null)
							    _crow.GetComponent<PlayerController>().ControlPlayerHorMovement(q);
                            //playerController.ControlPlayerHorMovement(q);
                        }


					});

                    BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (young_hm10, YoungServiceUUID, YoungCharacteristic, null, (address, characteristicUUID, bytes) => {
						var Q0 = (float)((Int16)(bytes[9] << 8 | bytes[8]))/32768;
						var Q1 = (float)((Int16)(bytes[11] << 8 | bytes[10]))/32768;
						var Q2 = (float)((Int16)(bytes[13] << 8 | bytes[12]))/32768;
						var Q3 = (float)((Int16)(bytes[15] << 8 | bytes[14]))/32768;
                        Quaternion q = new Quaternion();
                        q[0] = Q1;
                        q[1] = Q2;
                        q[2] = Q3;
                        q[3] = Q0;
                        if(!sentFirst){
                            //playerController.InitDeviceSetting(q);
                            sentFirst = true;
							
                        }else{
							_crow = GameObject.FindGameObjectWithTag("Crow");
							if(_crow != null)
							    _crow.GetComponent<PlayerController>().ControlPlayerVerMovement(q);
							//GameObject.FindGameObjectWithTag("Crow").GetComponent<playerController>().ControlPlayerVerMovement(q);
                            //playerController.ControlPlayerVerMovement(q);
                        }


					});

					// set to the none state and the user can start sending and receiving data
					//SetState (States.SubscribeYoung, .1f);

                    _state = States.None;

					break;

                case States.SubscribeYoung:
                    

					// set to the none state and the user can start sending and receiving data
					

					break;

				case States.Unsubscribe:
					BluetoothLEHardwareInterface.UnSubscribeCharacteristic (old_hm10, OldServiceUUID, OldCharacteristic, null);
                    BluetoothLEHardwareInterface.UnSubscribeCharacteristic (young_hm10, YoungServiceUUID, YoungCharacteristic, null);
					SetState (States.Disconnect, 4f);
					break;

				case States.Disconnect:
					if (_connected)
					{
						BluetoothLEHardwareInterface.DisconnectPeripheral (old_hm10, (address) => {
							BluetoothLEHardwareInterface.DeInitialize (() => {
								
								_connected = false;
								_state = States.None;
							});
						});

                        BluetoothLEHardwareInterface.DisconnectPeripheral (young_hm10, (address) => {
							BluetoothLEHardwareInterface.DeInitialize (() => {
								
								_connected = false;
								_state = States.None;
							});
						});
					}
					else
					{
						BluetoothLEHardwareInterface.DeInitialize (() => {
							
							_state = States.None;
						});
					}
					break;
				}
			}
		}
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
