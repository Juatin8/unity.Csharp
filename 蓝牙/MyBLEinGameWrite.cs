﻿using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MyBLEinGameWrite : MonoBehaviour  
{
    public string DeviceName = "MyESP32";
    public string ServiceUUID = "4fafc201-1fb5-459e-8fcc-c5c9c331914b";//"A9E90000-194C-4523-A473-5FDF36AA4D20";
    public string MyUUID = "beb5483e-36e1-4688-b7f5-ea07361b26a8";//"A9E90001-194C-4523-A473-5FDF36AA4D20";
    public string SendUUID = "beb5483e-36e1-4688-b7f5-ea07361b26a8";//"A9E90002-194C-4523-A473-5FDF36AA4D20";

    enum States  //不同的状态
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
    private bool _foundUUID = false;
    private bool _rssiOnly = false;
    private int _rssi = 0;       //RSSI表示接收信号强度
    #endregion

    public Text StatusText;
    public Text ParamText;
    public float ParamtValue;  //蓝牙传进来的参数值
    private bool isOn = false; //按钮开关状态

    void Start()     //初始化
    {
        StartProcess();
    }

    void Update()     //根据状态机状态不同，执行不同的BLE操作，比如扫描设备、连接设备、读取设备信息
    {
        #region 时间操作
        if(_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if(_timeout <= 0f)
            {
                _timeout = 0f;
                switch(_state)
                {
                    #endregion
                    #region 1.没有状态
                    case States.None:  //1.没有状态
                        break;
                    #endregion
                    #region 2.扫描
                    case States.Scan:   //2.扫描
                        StatusMessage = "Scanning for " + DeviceName;
                        // 下面有两个回调函数：①只有地址和名称 ②有地址、名称、rssi和bytes
                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                        {
                            if(!_rssiOnly)   //如果不是只使用RSSI进行扫描
                            {
                                if(name.Contains(DeviceName))  //如果找到了设备
                                {
                                    StatusMessage = "Found " + name;
                                    _deviceAddress = address;        // 设置设备地址
                                    SetState(States.Connect, 0.5f);  // 进入连接状态
                                }
                            }

                        }, (address, name, rssi, bytes) =>
                        {
                            if(name.Contains(DeviceName))   //如果找到了设备
                            {
                                StatusMessage = "Found " + name;
                                if(_rssiOnly) { _rssi = rssi; }
                                else
                                {
                                    _deviceAddress = address;        // 设置设备地址
                                    SetState(States.Connect, 0.5f);  // 进入连接状态
                                }
                            }

                        }, _rssiOnly);              // 设置是否只使用RSSI进行扫描

                        if(_rssiOnly)               // 如果只使用RSSI进行扫描，则进入连接状态，0.5s超时
                            SetState(States.ScanRSSI, 0.5f);
                        break;
                    #endregion
                    #region 3&4 扫描和读取RSSI
                    case States.ScanRSSI:  //3.扫描RSSI
                        break;

                    case States.ReadRSSI:   //4.读取RSSI
                        StatusMessage = $"Call Read RSSI";
                        BluetoothLEHardwareInterface.ReadRSSI(_deviceAddress, (address, rssi) =>
                        {
                            StatusMessage = $"Read RSSI: {rssi}";
                        });

                        SetState(States.ReadRSSI, 2f);
                        break;
                    #endregion
                    #region 5.连接上了
                    case States.Connect:   //5.连接上了
                        StatusMessage = "Connecting...";
                        _foundUUID = false;

                        //如果连接上了蓝牙
                        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                        {
                            StatusMessage = "Connected...";            //更新显示
                            BluetoothLEHardwareInterface.StopScan();   //停止scan
                            if(IsEqual(serviceUUID, ServiceUUID))      //如果serviceUUID正确
                            {
                                StatusMessage = "Found Service UUID";    //显示找到UUID

                                bool isUuidFound = _foundUUID;
                                bool isUuidMatched = IsEqual(characteristicUUID, MyUUID);
                                if(!isUuidFound && isUuidMatched)  //如果还没有找到UUID，但是MyUUID对上了，那么就是找到UUID了
                                {
                                    _foundUUID = true;
                                }

                                if(_foundUUID)    //如果找到了UUID
                                {
                                    _connected = true;                   //连接上了
                                    SetState(States.RequestMTU, 2f);     //进入请求MTU状态
                                }
                            }
                        });
                        break;
                    #endregion
                    # region 6.请求MTU
                    case States.RequestMTU:    //6.请求MTU MTU代表最大传输单元（Maximum Transmission Unit）
                        StatusMessage = "Requesting MTU";
                        BluetoothLEHardwareInterface.RequestMtu(_deviceAddress, 185, (address, newMTU) =>
                        {
                            StatusMessage = "MTU set to " + newMTU.ToString();  //显示MTU
                            SetState(States.Subscribe, 0.1f);                   //进入订阅状态
                        });
                        break;
                    #endregion
                    case States.Subscribe:     //7.订阅
                        #region 无需关心的部分订阅操作
                        StatusMessage = "Subscribing to characteristics...";
                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, ServiceUUID, MyUUID,
                        (notifyAddress, notifyCharacteristic) =>
                        {
                            StatusMessage = "Waiting for user action (1)...";    //显示：等待用户操作
                            _state = States.None;                                 //状态机状态设置为None

                            // 读取最初状态
                            BluetoothLEHardwareInterface.ReadCharacteristic(_deviceAddress, ServiceUUID, MyUUID,
                            (characteristic, bytes) =>
                            {
                                //ProcessButton(bytes);
                                // ButtonPositionText.text = Encoding.UTF8.GetString(bytes);
                            });

                            // SetState(States.ReadRSSI, 1f);
                        # endregion
                        }, (address, characteristicUUID, bytes) =>
                        {
                            //--------------------------------------------------------------- 获取数据的关键代码 -----------------------------------
                            if(_state != States.None)  //如果不是空状态
                            {
                                string mystring = Encoding.UTF8.GetString(bytes);
                                ParamText.text = mystring;
                                ParamtValue = float.Parse(mystring); //将bytes转化为float
                            }
                            //----------------------------------------------------------------------------------------------------------------------------
                        });
                        break;
                    #region 8.取消订阅
                    case States.Unsubscribe:  //取消订阅
                        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, ServiceUUID, MyUUID, null);
                        SetState(States.Disconnect, 4f);
                        break;
                    #endregion
                    #region 9.断开连接
                    case States.Disconnect:  //断开连接
                        StatusMessage = "Commanded disconnect.";

                        if(_connected)
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
                            { _state = States.None; });
                        }
                        break;
#endregion
                }
            }
        }
    }


//--------------------------------------------- 函数封装------------------------------------------------
    void SendByte(byte value, string uuid)   //用蓝牙发送数据
    {
        byte[] data = { value };
        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ServiceUUID, uuid, data, data.Length, true, (characteristicUUID) =>
        {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }
    public void OnLED()   //发送蓝牙数据的按钮开关
    {
        isOn = !isOn; //按下后状态反转
        if(isOn)
        {
            SendByte((byte)0x01, SendUUID);   //(byte)0x01表示把16进制值0x01强行转化为二进制。0x01就是1，0x00就是0
        }
        else
        {
            SendByte((byte)0x00, SendUUID);
        }
    }

    #region 基本无需关心的函数封装
    private string StatusMessage //获取信息
    {
        set
        {
            BluetoothLEHardwareInterface.Log(value);
            StatusText.text = value;
        }
    }
    void SetState(States newState, float timeout)  //设置状态和超时时间
    {
        _state = newState;
        _timeout = timeout;
    }
    void StartProcess()     //初始化并设置初始状态
    {
        Reset(); //重置一下状态
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            SetState(States.Scan, 0.1f);   //扫描状态
        }, (error) =>
        {
            StatusMessage = "Error during initialize: " + error;
        });
    }
    void Reset()   //重置状态和变量
    {
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _deviceAddress = null;
        _foundUUID = false;
        _rssi = 0;
    }
//------------------------------------------------------------- UUID相关 -----------------------------------------------------
    string FullUUID(string uuid)  //将UUID补全为完整的UUID
    {
        string fullUUID = uuid;
        if(fullUUID.Length == 4)
            fullUUID = "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
        return fullUUID;
    }
    bool IsEqual(string uuid1, string uuid2)  //判断两个UUID是否相等
    {
        if(uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if(uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);
        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }
    #endregion
}