
using UnityEngine;

public class Float : MonoBehaviour
{
    Vector3 trans;//简谐运动变化的位置，计算得出

    public float zhenFu = 0.08f;//振幅
    public float HZ = 1f;//频率

    private void Update()
    {
        trans = transform.position;
        trans.y = Mathf.Sin(Time.fixedTime * Mathf.PI * HZ) * zhenFu + transform.position.y;
        transform.position = trans;
    }
}