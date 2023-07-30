using TMPro;
using UnityEngine;

public class GPSDetector : MonoBehaviour
{
    public TextMeshProUGUI GPSxText;
    public TextMeshProUGUI GPSyText;
    public TextMeshProUGUI GPSzText;

    void Start()
    {
        Input.location.Start();
    }

    void Update()
    {
        // ���GPSλ�÷����Ƿ����
        if(Input.location.isEnabledByUser)
        {
            // ��ȡ�豸�ĵ�ǰλ����Ϣ
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float altitude = Input.location.lastData.altitude;

            GPSxText.text = "Latitude: " + latitude;
            GPSyText.text = "Longitude: " + longitude;
            GPSzText.text = "Altitude: " + altitude;

            // �ڿ���̨���λ����Ϣ
            Debug.Log("Latitude: " + latitude + ", Longitude: " + longitude + ", Altitude: " + altitude);
        }
        else
        { 
            Debug.Log("no GPS service "); 
        }
    }

    void OnApplicationQuit()
    {
        Input.location.Stop();
    }

}
