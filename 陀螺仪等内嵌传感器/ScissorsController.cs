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
        InitiateGryo(); //����������

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
            Vector3 gravity = Input.gyro.gravity;                 // �������ٶ�
            transform.rotation = Quaternion.LookRotation(Vector3.forward, gravity);   // ��ת����
        }
    }

    public void Reset()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    // ------------------------------------ ������װ----------------------------------
    public void InitiateGryo()  //����������
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
