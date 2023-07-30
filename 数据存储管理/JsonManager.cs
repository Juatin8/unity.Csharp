using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    //-----------------------------------------��json��ĳ��ֵ-----------------------------------------------------------
    public static void ChangeJsonValue<T>(string filepath, string fieldName, object newValue) where T : class, new()  
    {
        T data = ReadJson<T>(filepath);  // Read the original JSON file
        Type type = typeof(T);
        FieldInfo field = type.GetField(fieldName);  // Get the FieldInfo object for the specified field
        if(field != null)  //����ֶβ�Ϊ�գ���ô
        {
            field.SetValue(data, Convert.ChangeType(newValue, field.FieldType));  // Set the value of the field to the new value
            WriteJson(filepath, data);  // Write the updated object back to the JSON file
        }
        else
        {
            Debug.Log("no feild");
        }
    }

    //------------------------------------------���μ���json���ݸ�ĳ��ֵ----------------------------------
    //�����json�ļ��Ͷ�ȡjson�������е�ĳ���ֶθ�ֵ����ǰĳ��ֵ������͸�ĳ���ֶζ�һ��Ԥ��ֵ�����½�json�ļ�
    public static void FirstTimeLoadValueFromJson<T>(T data, string filePath, string fieldName, string defaultValue, out string fieldValue)
    {
        fieldValue = string.Empty;

        if(File.Exists(filePath))  //������ھͶ�ȡ
        {
            data = ReadJson<T>(filePath);
            var property = typeof(T).GetProperty(fieldName);
            if(property != null && property.PropertyType == typeof(string))
            {
                fieldValue = (string)property.GetValue(data);
            }
        }
        else //��������ھ�д��Ԥ��
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

    //------------------------------------- ��Json-----------------------------------------------
    public static T ReadJson<T>(string filePath)   //��json
    {
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<T>(json);
    }


    //--------------------------------------- дJson-----------------------------------------------
    public static void WriteJson<T>(string filePath, T data)  //дjson
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }
}