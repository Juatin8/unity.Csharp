using UnityEngine;

public class Timer
{
    private static float startTime;
    public static void StartTimer()
    {
        startTime = Time.time;
        Debug.Log(startTime);
    }
    public static void StopTimer(ref string ModelTime)  //传入Model中的一个存时间的变量
    {
        float elapsedTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        ModelTime = string.Format("{0:00}:{1:00}", minutes, seconds);  //用string保存时间
        Debug.Log(string.Format("{0:00}:{1:00}", minutes, seconds));
        Debug.Log(Model.time1);
    }
}