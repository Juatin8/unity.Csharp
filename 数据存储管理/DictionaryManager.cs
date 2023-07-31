using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{

    public static Dictionary<TKey, TValue> LoadDictionaryFromFile<TKey, TValue>(string filePath)  // 从文件中加载字典
    {
        Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        if(File.Exists(filePath))
        {
            using(StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if(parts.Length == 2)
                    {
                        TKey key = (TKey)Convert.ChangeType(parts[0], typeof(TKey));
                        TValue value = (TValue)Convert.ChangeType(parts[1], typeof(TValue));
                        dictionary[key] = value;
                    }
                }
            }
        }
        return dictionary;
    }


    public static int CountValues<T>(Dictionary<string, T> dict, T value)  // 计算字典中值为value的键值对的数量
    {
        int count = 0;
        foreach(var kvp in dict)
        {
            if(EqualityComparer<T>.Default.Equals(kvp.Value, value))
            {
                count++;
            }
        }
        return count;
    }


public static void SaveDictionaryToFile<TKey, TValue>(Dictionary<TKey, TValue> dictionary, string filePath)   // 将字典保存到文件中(覆盖原有文件)
    {
        using(StreamWriter writer = new StreamWriter(filePath))
        {
            foreach(var kvp in dictionary)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value}");
            }
        }

    }

    public static bool ModifyDictionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue newValue)  // 修改字典中的值（如果键存在）
    {
        if(dictionary.ContainsKey(key))
        {
            dictionary[key] = newValue;
            return true;
        }
        else
        {
            return false;
        }
    }





}
