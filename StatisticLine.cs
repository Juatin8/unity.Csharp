using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class StatisticLine
{
    [MenuItem("输出总代码行数/输出")]
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

            //文件名+文件行数
            //Debug.Log(String.Format("{0}——{1}", temp, nowLine));

            totalLine += nowLine;
        }

        Debug.Log(String.Format("总代码行数：{0}", totalLine));
    }
}