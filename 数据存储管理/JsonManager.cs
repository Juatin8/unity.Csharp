using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    //-----------------------------------------改json中某个值-----------------------------------------------------------
    public static void ChangeJsonValue<T>(string filepath, string fieldName, object newValue) where T : class, new()  
    {
        T data = ReadJson<T>(filepath);  // Read the original JSON file
        Type type = typeof(T);
        FieldInfo field = type.GetField(fieldName);  // Get the FieldInfo object for the specified field
        if(field != null)  //如果字段不为空，那么
        {
            field.SetValue(data, Convert.ChangeType(newValue, field.FieldType));  // Set the value of the field to the new value
            WriteJson(filepath, data);  // Write the updated object back to the JSON file
        }
        else
        {
            Debug.Log("no feild");
        }
    }

    //------------------------------------------初次加载json数据给某个值----------------------------------
    //如果有json文件就读取json并把其中的某个字段赋值给当前某个值，否则就给某个字段定一个预设值，并新建json文件
    public static void FirstTimeLoadValueFromJson<T>(T data, string filePath, string fieldName, string defaultValue, out string fieldValue)
    {
        fieldValue = string.Empty;

        if(File.Exists(filePath))  //如果存在就读取
        {
            data = ReadJson<T>(filePath);
            var property = typeof(T).GetProperty(fieldName);
            if(property != null && property.PropertyType == typeof(string))
            {
                fieldValue = (string)property.GetValue(data);
            }
        }
        else //如果不存在就写入预设
        {
            var newData = Activator.CreateInstance<T>();
            var newProperty = typeof(T).GetProperty(fieldName);
            if(newProperty != null && newProperty.PropertyType == typeof(string))
            {
                fieldValue = defaultValue;
                newProperty.SetValue(newData, fieldValue);
                WriteJson(filePath, newData);
            }
        }
    }

    //------------------------------------- 读Json-----------------------------------------------
    public static T ReadJson<T>(string filePath)   //读json
    {
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<T>(json);
    }


    //--------------------------------------- 写Json-----------------------------------------------
    public static void WriteJson<T>(string filePath, T data)  //写json
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }
}