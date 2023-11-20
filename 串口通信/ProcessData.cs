using UnityEngine;

public class ProcessData : MonoBehaviour
{
    public float a1, a2, a3, g1, g2, g3, t;
    public Quaternion q;
    SerialPorter sp;

    private void Start()
    {
        sp=GetComponent<SerialPorter>();
    }

    private void Update()
    {
        ProcessData1(sp.receivedData);
        q = Quaternion.Euler(-g1, -g3, -g2);
    }


    void ProcessData1(string data)
    {
        string[] dataParts = data.Split(':');
        string firstLetter = dataParts[0];
        a1 = float.Parse(dataParts[1]);
        a2 = float.Parse(dataParts[2]);
        a3 = float.Parse(dataParts[3]);
        g1 = float.Parse(dataParts[4]);
        g2 = float.Parse(dataParts[5]);
        g3 = float.Parse(dataParts[6]);
        t = float.Parse(dataParts[7]);
    }


    void ProcessData2(string data)
    {
        string[] dataParts = data.Split(':');
        string firstLetter = dataParts[0];
        a1 = float.Parse(dataParts[1]);
        a2 = float.Parse(dataParts[2]);
        a3 = float.Parse(dataParts[3]);
        g1 = float.Parse(dataParts[4]);
        g2 = float.Parse(dataParts[5]);
        g3 = float.Parse(dataParts[6]);
        t = float.Parse(dataParts[7]);
    }
}
