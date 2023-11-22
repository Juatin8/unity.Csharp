using UnityEngine;

public class ProcessData : MonoBehaviour //���������ݸ���MPU6050�ı������
{
    public float[] data1 = new float[7];    //a1,a2,a3,g1,g2,g3,t ���ݸ�ʽ
    public float[] data2 = new float[7];
    public Quaternion q;
    private SerialPorter sp;
    public string recievedData1, recievedData2;

    private void Start()
    {
        sp = GetComponent<SerialPorter>();
    }

    private void Update()
    {
       SeperateDataStream(sp.receivedData, ref recievedData1, ref recievedData2);
       ProcessMPU6050Data(recievedData1, ref data1);
       ProcessMPU6050Data(recievedData2, ref data2);
    }

    void ProcessMPU6050Data(string Odata, ref float[]IMUdata)
    {
        string[] dataParts = Odata.Split(':');
        for(int i = 0; i < dataParts.Length-1; i++)
        {
            IMUdata[i] = float.Parse(dataParts[i+1]);
        }
    }

    private void SeperateDataStream(string Odata, ref string recievedData1, ref string recievedData2)  //��ͬԴ���������������ݷ���
    {
        string[] dataParts = Odata.Split(':');
        int num = int.Parse(dataParts[0]);
        if(num == 1)
            recievedData1 = Odata;
        else if(num == 2)
            recievedData2 = Odata;
    }
}