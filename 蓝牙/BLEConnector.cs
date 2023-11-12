using UnityEngine;
using TMPro;

public class BLEConnectorRead2 : MonoBehaviour
{
    //----------- 参数-------------
    const string DeviceName = "WT901BLE68";
    const string ServiceUUID = "0000FFE5-0000-1000-8000-00805F9A34FB";
    const string ReadUUID = "0000ffe4-0000-1000-8000-00805f9a34fb";
    const string WriteUUID = "0000ffe9-0000-1000-8000-00805f9a34fb";

    public float[] Q = new float[4];
    public float temp;

    //-------------------------------
    enum States
    {
        None,
        Scan,
        ScanRSSI,
        ReadRSSI,
        Connect,
        RequestMTU,
        Subscribe,
        Unsubscribe,
        Disconnect,
    }

    private bool _connected = false;
    private float _timeout = 0f;
    private States _state = States.None;
    private string _deviceAddress;
    public bool _foundUUID = false;
    private bool _rssiOnly = false;
    private int _rssi = 0;

    public TextMeshProUGUI StatusText;
    public byte[] BLEdata;

    void Start()
    {
        StartProcess(); //从none状态切换到scan，开始连接流程
    }

    void Update()
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

                    case States.Scan:
                        ScanState();
                        break;

                    case States.ScanRSSI:
                        break;

                    case States.ReadRSSI:
                        ReadRSSIState();
                        break;

                    case States.Connect:
                        ConnectState();
                        break;

                    case States.RequestMTU:
                        RequestMTUState();
                        break;

                    case States.Subscribe:
                        SubscribeState();
                        break;

                    case States.Unsubscribe:
                        UnsubscribeState();
                        break;

                    case States.Disconnect:
                        DisconnectState();
                        break;
                }
            }
        }
    }


    //------- 不同状态下执行的操作----------

    private void ScanState()
    {
        StatusMessage = "Scanning for " + DeviceName;
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
        {
            if (!_rssiOnly)
            {
                if (name.Contains(DeviceName))
                {
                    StatusMessage = "Found " + name;
                    _deviceAddress = address;
                    SetState(States.Connect, 0.1f);
                }
            }
        }, (address, name, rssi, bytes) =>
        {
            if (name.Contains(DeviceName))
            {
                StatusMessage = "Found " + name;
                if (_rssiOnly) { _rssi = rssi; }
                else
                {
                    _deviceAddress = address;
                    SetState(States.Connect, 0.1f);
                }
            }
        }, _rssiOnly);

        if (_rssiOnly)
            SetState(States.ScanRSSI, 0.1f);
    }

    private void ReadRSSIState()
    {
        StatusMessage = $"Call Read RSSI";
        BluetoothLEHardwareInterface.ReadRSSI(_deviceAddress, (address, rssi) =>
        {
            StatusMessage = $"Read RSSI: {rssi}";
        });

        SetState(States.ReadRSSI, 0.1f);
    }

    private void ConnectState()
    {
        StatusMessage = "Connecting...";
        _foundUUID = false;

        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
        {
            StatusMessage = "Connected...";
            BluetoothLEHardwareInterface.StopScan();
            if (IsEqual(serviceUUID, ServiceUUID))
            {
                StatusMessage = "Found Service UUID";
                bool isUuidFound = _foundUUID;
                bool isUuidMatched = IsEqual(characteristicUUID, ReadUUID);
                if (!isUuidFound && isUuidMatched)
                {
                    _foundUUID = true;
                }

                if (_foundUUID)
                {
                    _connected = true;
                    SetState(States.RequestMTU, 1f);
                }
            }
        });
    }

    private void RequestMTUState()
    {
        StatusMessage = "Requesting MTU";
        BluetoothLEHardwareInterface.RequestMtu(_deviceAddress, 185, (address, newMTU) =>
        {
            StatusMessage = "MTU set to " + newMTU.ToString();
            SetState(States.Subscribe, 0.1f);
        });
    }

    private void SubscribeState()
    {
        StatusMessage = "Subscribing to characteristics...";
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, ServiceUUID, ReadUUID,
        (notifyAddress, notifyCharacteristic) => //处理订阅结果
        {
            StatusMessage = "Waiting for user action...";
            _state = States.None; //切换到空状态，蓝牙不会再做状态切换，用户此时可发起指令
        }, (address, characteristicUUID, bytes) => //带数据的数据传进来了
        {
            BLEdata = bytes;
            WitBLESetting.ProcessBatteryData(BLEdata,ref temp ); //需要用户操作的地方
        });
    }

    private void UnsubscribeState()
    {
        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, ServiceUUID, ReadUUID, null);
        SetState(States.Disconnect, 4f);
    }

    private void DisconnectState()
    {
        StatusMessage = "Commanded disconnect.";

        if (_connected)
        {
            BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
            {
                StatusMessage = "Device disconnected";
                BluetoothLEHardwareInterface.DeInitialize(() =>
                {
                    _connected = false;
                    _state = States.None;
                });
            });
        }
        else
        {
            BluetoothLEHardwareInterface.DeInitialize(() =>
            {
                _state = States.None;
            });
        }
    }
    /*
    public void ReadOnce() //只读一次
    {
         BluetoothLEHardwareInterface.ReadCharacteristic(_deviceAddress, ServiceUUID, ReadUUID,
        (characteristic, bytes) =>
       {
           BLEdata = bytes;
           Debug.Log("bledata=" + BLEdata);
       });
    } */
 
    public void Write()
    {
        byte[] data = WitBLESetting.BatteryData;
        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ServiceUUID, WriteUUID, data, data.Length, true, (characteristicUUID) =>
        {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }


    //------------------
    public void StartProcess()
    {
        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            SetState(States.Scan, 0.1f);
        }, (error) =>
        {
            StatusMessage = "Error during initialize: " + error;
        });
    }

    private void Reset()
    {
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _deviceAddress = null;
        _foundUUID = false;
        _rssi = 0;
    }

    //--------------底层计算------------

    private string FullUUID(string uuid)
    {
        string fullUUID = uuid;
        if (fullUUID.Length == 4)
            fullUUID = "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
        return fullUUID;
    }

    private bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);
        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

    private void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    private string StatusMessage //获取信息
    {
        set
        {
            BluetoothLEHardwareInterface.Log(value);
            StatusText.text = value;
        }
    }
}
