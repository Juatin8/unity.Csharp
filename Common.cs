
using UnityEngine;

public static class Common 
{
    public static float CalcDistance(GameObject A, GameObject B)    //计算两个物体之间的距离
    {
       Vector3 Pa = A.transform.position;         
        Vector3 Pb = B.transform.position; 
        float distance = Vector3.Distance(Pa, Pb); 
        return distance;
    }


    public static void SetBoundaryPosition(GameObject A, float Xmin, float Xmax, float Ymin, float Ymax)   //设置边界
    { 
        Vector3 playerPosition = A.transform.position; // 玩家位置也就是头显的位置
        A.transform.position = new Vector3
       (Mathf.Clamp(playerPosition.x, Xmin, Xmax), playerPosition.y, Mathf.Clamp(playerPosition.z, Ymin, Ymax));
    }


    public static void Float(GameObject A, float zhenfu, float hz)   //简谐运动
    {
        Vector3 trans = A.transform.position;
        trans.y += Mathf.Sin(Time.fixedTime * Mathf.PI * hz) * zhenfu;
        A.transform.position = trans;
    }

    public static void Follow(GameObject followed, GameObject following, ref Vector3 Porigin) //跟随
    {
        Vector3 Pnew = followed.transform.position; 
        following.transform.Translate(Pnew - Porigin); 
        Porigin = Pnew;
    }

    public static void DestroyAll(GameObject[] gbs, float Seconds)   //销毁所有物体
    {
        foreach(var go in gbs)
        {
            GameObject.Destroy(go, Seconds);
        }
    }
}
