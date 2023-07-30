using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IMUController : MonoBehaviour
{
    public TextMeshProUGUI accelerationText;
    public TextMeshProUGUI gyroscopeText;
    public TextMeshProUGUI magnetometerText;

    private Vector3 acceleration;
    private Vector3 gyroscope;
    private Vector3 magnetometer;

    void Update()
    {
        acceleration = Input.acceleration;
        gyroscope = Input.gyro.rotationRateUnbiased;
        magnetometer = Input.compass.rawVector;

        accelerationText.text = "Acceleration: " + acceleration.ToString();
        gyroscopeText.text = "Gyroscope: " + gyroscope.ToString();
        magnetometerText.text = "Magnetometer: " + magnetometer.ToString();
    }
}
