using UnityEngine;
using System.IO;
using SimpleJSON;
using System;
using System.Reflection;

public class JsonPrefs
{
    // 获取 JSON 文件的路径，使用类型名称作为key
 /*   private static string GetFilePath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key + ".json");
    }
    */
    private static readonly string JsonFolderPath = Path.Combine(Application.persistentDataPath, "JSON");

     // 获取 JSON 文件的路径，使用类型名称作为key
    private static string GetFilePath(string key)
    {
        // 确保 JSON 文件夹存在
        if (!Directory.Exists(JsonFolderPath))
        {
            Directory.CreateDirectory(JsonFolderPath);
        }

        return Path.Combine(JsonFolderPath, key + ".json");
    }

    // 存储对象类型（类的实例）
    public static void SetClass<T>(T instance)
    {
        string key = typeof(T).Name;  // 使用类型名作为key
        JSONObject jsonData = Load(key) ?? new JSONObject();
        
        jsonData[key] = JSON.Parse(JsonUtility.ToJson(instance)).AsObject;
        
        string filePath = GetFilePath(key);
        File.WriteAllText(filePath, jsonData.ToString());
    }

    // 存储基础数据类型（string, int, float, bool, etc.）
    public static void SetVariable<T>(string key, T value)
    {
        JSONObject jsonData = Load(key) ?? new JSONObject();

        // 对不同类型进行处理
        if (value is string)
        {
            jsonData[key] = value.ToString();
        }
        else if (value is int)
        {
            jsonData[key] = (int)(object)value;
        }
        else if (value is float)
        {
            jsonData[key] = (float)(object)value;
        }
        else if (value is bool)
        {
            jsonData[key] = (bool)(object)value;
        }
        else if (value is JSONObject)
        {
            jsonData[key] = (JSONObject)(object)value;
        }
        else if (value is JSONArray)
        {
            jsonData[key] = (JSONArray)(object)value;
        }

        string filePath = GetFilePath(key);
        File.WriteAllText(filePath, jsonData.ToString());
    }

public static T GetClass<T>(T instance, T defaultValue)
{
    string key = typeof(T).Name;  // 使用类型名作为key
    JSONObject jsonData = Load(key);  // 加载时传入类型判断
    
    if (jsonData != null)
    {
        var value = jsonData[key];
        string json = value.ToString();  // 获取 JSON 字符串

        // 反序列化 JSON 为临时对象
        T tempInstance = JsonUtility.FromJson<T>(json);

        // 获取类型的所有字段
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

        foreach (var field in fields)
        {
            Type fieldType = field.FieldType;

            // 获取临时对象中该字段的值
            var fieldValue = field.GetValue(tempInstance);

            // 如果值为空或类型不匹配则跳过
            if (fieldValue == null || !fieldType.IsAssignableFrom(fieldValue.GetType()))
                continue;

            // 根据字段类型进行赋值
            if (fieldType == typeof(int))
            {
                field.SetValue(instance, Convert.ToInt32(fieldValue)); // 转换为 int 并赋值
            }
            else if (fieldType == typeof(float))
            {
                field.SetValue(instance, Convert.ToSingle(fieldValue)); // 转换为 float 并赋值
            }
            else if (fieldType == typeof(DateTime))
            {
                field.SetValue(instance, Convert.ToDateTime(fieldValue)); // 转换为 DateTime 并赋值
            }
            else
            {
                field.SetValue(instance, fieldValue);  // 其他类型直接赋值
            }
        }
    }

    return instance; // 如果没有找到数据，返回原始实例
}

   /* // 获取整个对象类型（类的实例）
    public static T GetClass<T>(T instance, T defaultValue)
    {
        string key = typeof(T).Name;  // 使用类型名作为key
        JSONObject jsonData = Load(key);  // 加载时传入类型判断
        
        if (jsonData != null)
        {
            var value = jsonData[key];
            return JsonUtility.FromJson<T>(value.ToString()); // 反序列化 JSON 字符串为对象
        }
        return defaultValue;
    }

public static void UpdateFieldsFromJson<T>(T instance, string json)
{
    T tempInstance = JsonUtility.FromJson<T>(json);
    FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

    foreach (var field in fields)
    {
        Type fieldType = field.FieldType;

        var value = field.GetValue(tempInstance);

        if (value == null || !fieldType.IsAssignableFrom(value.GetType()))
            continue;

        if (fieldType == typeof(int))
        {
            field.SetValue(instance, Convert.ToInt32(value)); // 转换为int并赋值
        }
        else if (fieldType == typeof(float))
        {
            field.SetValue(instance, Convert.ToSingle(value)); // 转换为float并赋值
        }
        else if (fieldType == typeof(DateTime))
        {
            field.SetValue(instance, Convert.ToDateTime(value)); // 转换为DateTime并赋值
        }
        else
        { 
            field.SetValue(instance, value);
        }
    }
}  */


    // 获取基础数据类型（string, int, float, bool, etc.）
    public static T GetVariable<T>(string key,T defaultValue)
    {
        JSONObject jsonData = Load(key);  // 加载时传入类型判断
        
        if (jsonData != null)
        {
            var value = jsonData[key];

            if (typeof(T) == typeof(string))
            {
                return (T)(object)value.Value;
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)value.AsInt;
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)value.AsFloat;
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)value.AsBool;
            }
            else if (typeof(T) == typeof(JSONObject))
            {
                return (T)(object)value.AsObject;
            }
            else if (typeof(T) == typeof(JSONArray))
            {
                return (T)(object)value.AsArray;
            }
        }
        return defaultValue;
    }
public static T GetKey<T>(T instance, string jsonKey, T defaultValue)
{
    string key = typeof(T).Name;  // 使用类型名作为key
    JSONObject jsonData = Load(key);  // 加载文件

    if (jsonData != null)  // 使用 IsDefined 判断是否存在
    {
        var value = jsonData[jsonKey];

        if (typeof(T) == typeof(string))
        {
            return (T)(object)value.Value;
        }
        else if (typeof(T) == typeof(int))
        {
            return (T)(object)value.AsInt;
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)(object)value.AsFloat;
        }
        else if (typeof(T) == typeof(bool))
        {
            return (T)(object)value.AsBool;
        }
        else if (typeof(T) == typeof(JSONObject))
        {
            return (T)(object)value.AsObject;
        }
        else if (typeof(T) == typeof(JSONArray))
        {
            return (T)(object)value.AsArray;
        }
    }
    return defaultValue;  // 如果找不到，返回默认值
}

    // 删除整个 JSON 文件
    public static void Delete<T>()
    {
        string key = typeof(T).Name;  // 使用类型名作为key
        string filePath = GetFilePath(key);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);  // 删除文件
        }
        else
        {
            Debug.LogWarning($"File for type {typeof(T).Name} not found.");
        }
    }

    // 删除基础数据类型相关文件
    public static void Delete(string key)
    {
        string filePath = GetFilePath(key);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);  // 删除文件
        }
        else
        {
            Debug.LogWarning($"File for key {key} not found.");
        }
    }

    // 加载文件并判断类型（对象或基础数据类型）
    private static JSONObject Load(string key)
    {
        string filePath = GetFilePath(key);
        if (!File.Exists(filePath))
        {
            return null;
        }

        string fileContents = File.ReadAllText(filePath);
        return JSON.Parse(fileContents).AsObject;
    }
}
