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
        // 检查GPS位置服务是否可用
        if(Input.location.isEnabledByUser)
        {
            // 获取设备的当前位置信息
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float altitude = Input.location.lastData.altitude;

            GPSxText.text = "Latitude: " + latitude;
            GPSyText.text = "Longitude: " + longitude;
            GPSzText.text = "Altitude: " + altitude;

            // 在控制台输出位置信息
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
