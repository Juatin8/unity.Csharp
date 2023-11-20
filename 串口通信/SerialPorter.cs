using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialPorter : MonoBehaviour
{
    SerialPort serialPort;
    public string receivedData;
    public int portNumber;

    Thread readThread;  

    void Start()
    {
        string portName;
        portName = "com" + portNumber;
        serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);         // 创建SerialPort对象，设置串口参数
        serialPort.ReadTimeout = 1000; // 设置读取超时时间

        serialPort.Open();  // 打开串口连接

        readThread = new Thread(ReadSerialData);  //开一个线程来处理这个事情，不占用游戏主线程。
        readThread.Start();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        if(serialPort != null && serialPort.IsOpen)  // 关闭串口连接
        {
            serialPort.Close();
        }

        if(readThread != null && readThread.IsAlive)
        {
            readThread.Join();
        }
    }

    void ReadSerialData()
    {
        while(serialPort.IsOpen)
        {
            try
            {
                receivedData = serialPort.ReadLine();   // 读取串口数据
                Debug.Log("Received data: " + receivedData);
            }
            catch(System.Exception e)
            {
                Debug.LogWarning("Serial port read error: " + e.Message);
            }
        }
    }
}