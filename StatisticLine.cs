using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class StatisticLine
{
    [MenuItem("����ܴ�������/���")]
    private static void PrintTotalLine()
    {
        string[] fileName = Directory.GetFiles("Assets/Scripts", "*.cs", SearchOption.AllDirectories);

        int totalLine = 0;
        foreach(var temp in fileName)
        {
            int nowLine = 0;
            StreamReader sr = new StreamReader(temp);
            while(sr.ReadLine() != null)
            {
                nowLine++;
            }

            //�ļ���+�ļ�����
            //Debug.Log(String.Format("{0}����{1}", temp, nowLine));

            totalLine += nowLine;
        }

        Debug.Log(String.Format("�ܴ���������{0}", totalLine));
    }
}