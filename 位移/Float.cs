
using UnityEngine;

public class Float : MonoBehaviour
{
    Vector3 trans;//��г�˶��仯��λ�ã�����ó�

    public float zhenFu = 0.08f;//���
    public float HZ = 1f;//Ƶ��

    private void Update()
    {
        trans = transform.position;
        trans.y = Mathf.Sin(Time.fixedTime * Mathf.PI * HZ) * zhenFu + transform.position.y;
        transform.position = trans;
    }
}