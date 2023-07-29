
using UnityEngine;

public static class Common 
{
    public static float CalcDistance(GameObject A, GameObject B)    //������������֮��ľ���
    {
       Vector3 Pa = A.transform.position;         
        Vector3 Pb = B.transform.position; 
        float distance = Vector3.Distance(Pa, Pb); 
        return distance;
    }


    public static void SetBoundaryPosition(GameObject A, float Xmin, float Xmax, float Ymin, float Ymax)   //���ñ߽�
    { 
        Vector3 playerPosition = A.transform.position; // ���λ��Ҳ����ͷ�Ե�λ��
        A.transform.position = new Vector3
       (Mathf.Clamp(playerPosition.x, Xmin, Xmax), playerPosition.y, Mathf.Clamp(playerPosition.z, Ymin, Ymax));
    }


    public static void Float(GameObject A, float zhenfu, float hz)   //��г�˶�
    {
        Vector3 trans = A.transform.position;
        trans.y += Mathf.Sin(Time.fixedTime * Mathf.PI * hz) * zhenfu;
        A.transform.position = trans;
    }

    public static void Follow(GameObject followed, GameObject following, ref Vector3 Porigin) //����
    {
        Vector3 Pnew = followed.transform.position; 
        following.transform.Translate(Pnew - Porigin); 
        Porigin = Pnew;
    }

    public static void DestroyAll(GameObject[] gbs, float Seconds)   //������������
    {
        foreach(var go in gbs)
        {
            GameObject.Destroy(go, Seconds);
        }
    }
}
