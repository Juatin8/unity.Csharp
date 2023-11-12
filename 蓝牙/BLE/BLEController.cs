
using UnityEngine;

public class BLEController : MonoBehaviour //这个脚本专门用来处理蓝牙数据以及发送指令，修改最多的脚本
{
    public byte[] data;
    public float temp;
    private string address;
    private string serviceuuid;
    private string writeuuid;
    private BLEConnector bleconnector;

    void Start()
    {
        bleconnector = GetComponent<BLEConnector>();
        bleconnector.OnGetData.AddListener(ProcessData); //一旦收到数据，即做数据处理
    }

    private void ProcessData()
    {
        data = bleconnector.BLEdata;
        address = bleconnector._deviceAddress;
        serviceuuid = BLEConnector.ServiceUUID;
        writeuuid = BLEConnector.WriteUUID;
        WitBLESetting.ProcessBatteryData(data, ref temp); //需要用户操作的地方
    }

    public void Write()
    {

        byte[] data = WitBLESetting.BatteryData;
        BluetoothLEHardwareInterface.WriteCharacteristic(address, serviceuuid, writeuuid, data, data.Length, true, (characteristicUUID) =>
        {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });

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

}
