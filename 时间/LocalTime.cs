using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LocalTime : MonoBehaviour
{
    public Text showTime;
    public string strNowtime;

    void Awake()
    {
        showTime = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentTime();
    }
    public void GetCurrentTime()//获取当前时间
    {
        DateTime dateTime = DateTime.Now;
        strNowtime = dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second;
        showTime.text = strNowtime; 
    }
}