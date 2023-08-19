
using UnityEngine;
using UnityEngine.UI;

public class StartingExample : MonoBehaviour
{
    public string DeviceName = "ledbtn";
    public string ServiceUUID = "A9E90000-194C-4523-A473-5FDF36AA4D20";
    public string LedUUID = "A9E90001-194C-4523-A473-5FDF36AA4D20";
    public string ButtonUUID = "A9E90002-194C-4523-A473-5FDF36AA4D20";

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

    #region 一些无需改动的变量
    private bool _connected = false;
    private float _timeout = 0f;
    private States _state = States.None;
    private string _deviceAddress;
    private bool _foundButtonUUID = false;
    private bool _foundLedUUID = false;
    private bool _foundUUID = false;
    private bool _rssiOnly = false;
    private int _rssi = 0;       //RSSI表示接收信号强度
    #endregion

    public Text StatusText;
    public Text ButtonPositionText;




    // Use this for initialization
    void Start()
    {
        StartProcess();
    }


    // Update is called once per frame
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
                    #region 无状态
                    case States.None:
                        break;
                    #endregion

                    #region 扫描
                    case States.Scan:
                        StatusMessage = "寻找 " + DeviceName;
                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>  
                        { //扫描所有设备,并且返回设备地址和设备名称
                            if (!_rssiOnly) //如果不是只读RSSI
                            {
                                if (name.Contains(DeviceName))
                                {
                                    StatusMessage = "找到 " + name;
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f); //切换到连接状态
                                }
                            }

                        }, (address, name, rssi, bytes) =>
                        { //扫描所有设备,并且返回设备地址,设备名称,信号强度,广播数据
                            if (name.Contains(DeviceName)) 
                            {
                                StatusMessage = "找到 " + name;
                                if (_rssiOnly)   
                                {
                                    _rssi = rssi;
                                }
                                else
                                {
                                    _deviceAddress = address;       //保存地址
                                    SetState(States.Connect, 0.5f); //切换到连接状态
                                }
                            }

                        }, _rssiOnly);

                        if (_rssiOnly)  //如果只读RSSI
                            SetState(States.ScanRSSI, 0.5f);  
                        break;
                    #endregion

                    #region 扫描RSSI
                    case States.ScanRSSI:
                        break;
                    #endregion

                    #region 读取RSSI
                    case States.ReadRSSI:
                        StatusMessage = $"Call Read RSSI";
                        BluetoothLEHardwareInterface.ReadRSSI(_deviceAddress, (address, rssi) =>
                        {
                            StatusMessage = $"Read RSSI: {rssi}";
                        });

                        SetState(States.ReadRSSI, 2f);
                        break;
                    #endregion

                    #region 连接
                    case States.Connect:
                        StatusMessage = "正在连接中";

                        _foundButtonUUID = false;
                        _foundLedUUID = false;

                        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                        {
                            StatusMessage = "连接成功";

                            BluetoothLEHardwareInterface.StopScan(); //停止扫描

                            if (IsEqual(serviceUUID, ServiceUUID)) //判断是否找到 Service UUID
                            {
                                StatusMessage = "找到 Service UUID";
                                _foundButtonUUID = _foundButtonUUID || IsEqual(characteristicUUID, ButtonUUID);  
                                _foundLedUUID = _foundLedUUID || IsEqual(characteristicUUID, LedUUID);

                                if (_foundButtonUUID && _foundLedUUID)
                                {
                                    _connected = true;
                                    SetState(States.RequestMTU, 2f); //切换到请求MTU状态
                                }
                            }
                        });
                        break;
                    #endregion

                    #region 读取MTU
                    case States.RequestMTU:
                        StatusMessage = "请求 MTU";
                        BluetoothLEHardwareInterface.RequestMtu(_deviceAddress, 185, (address, newMTU) =>
                        {
                            StatusMessage = "MTU 设置为 " + newMTU.ToString();
                            SetState(States.Subscribe, 0.1f);
                        });
                        break;
                    #endregion

                    #region 订阅
                    case States.Subscribe:
                        StatusMessage = "订阅特征...";

                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, ServiceUUID, ButtonUUID, (notifyAddress, notifyCharacteristic) =>
                        {
                            StatusMessage = "等待用户操作(1)...";
                            _state = States.None;  //切换到无状态

                            BluetoothLEHardwareInterface.ReadCharacteristic(_deviceAddress, ServiceUUID, ButtonUUID, (characteristic, bytes) =>
                            {
                                ProcessButton(bytes);
                            });

                            SetState(States.ReadRSSI, 1f);  //切换到读取RSSI状态

                        }, (address, characteristicUUID, bytes) =>
                        {
                            if (_state != States.None)
                            {
                                StatusMessage = "等待用户操作(2)...";
                                SetState(States.ReadRSSI, 1f);
                            }
                            ProcessButton(bytes);
                        });
                        break;
                    #endregion

                    #region 取消订阅
                    case States.Unsubscribe:
                        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, ServiceUUID, ButtonUUID, null);
                        SetState(States.Disconnect, 4f);
                        break;
                    #endregion

                    #region 断开连接
                    case States.Disconnect:
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
                        break;
                }
                #endregion
            }
        }
    }

    //------------------------------ 和按钮绑定的封装------------------------------------

    private bool SwichON = true;
    public void OnSwitch()
    {
        if(SwichON)
        {
            SendByte((byte)0x05);
        }
        else
        {
            SendByte((byte)0x00);
        }
        SwichON = !SwichON;
    }

    public void Program1()
    {
            SendByte((byte)0x01);
    }
    public void Program2()
    {
        SendByte((byte)0x02);
    }
    //----------------------- 其它封装  -------------------------------------------

    private string StatusMessage
    {
        set
        {
            BluetoothLEHardwareInterface.Log(value);
            StatusText.text = value;
        }
    }

    void Reset()
    {
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _deviceAddress = null;
        _foundButtonUUID = false;
        _foundLedUUID = false;
        _rssi = 0;
    }


    void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    void StartProcess()
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

    private void ProcessButton(byte[] bytes)
    {
        if(bytes[0] == 0x00)
            ButtonPositionText.text = "Not Pushed";
        else
            ButtonPositionText.text = "Pushed";
    }

   
    string FullUUID(string uuid)
    {
        string fullUUID = uuid;
        if (fullUUID.Length == 4)
            fullUUID = "0000" + uuid + "-0000-1000-8000-00805f9b34fb";

        return fullUUID;
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

    void SendByte(byte value)
    {
        byte[] data = { value };
        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ServiceUUID, LedUUID, data, data.Length, true, (characteristicUUID) =>
        {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }
}