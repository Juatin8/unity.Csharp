using UnityEngine;

public class ScissorsController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;

    private Quaternion initialGyroRotation;
    private Quaternion initialObjectRotation;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        InitiateGryo(); //激活陀螺仪

        initialGyroRotation = Quaternion.identity;
        initialObjectRotation = transform.rotation;
        initialPosition = transform.position;
        initialRotation = Quaternion.identity;
    }

    void Update()
    {
        if(Model.ScissorBtnPressed)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.zero);
        }

        if(Input.gyro != null)
        {
            Vector3 gravity = Input.gyro.gravity;                 // 重力加速度
            transform.rotation = Quaternion.LookRotation(Vector3.forward, gravity);   // 旋转物体
        }
    }

    public void Reset()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    // ------------------------------------ 函数封装----------------------------------
    public void InitiateGryo()  //激活陀螺仪
    {
        if(SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.Log("Gyroscope not supported");
        }
    }
}
