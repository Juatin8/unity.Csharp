using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{

    public static Dictionary<TKey, TValue> LoadDictionaryFromFile<TKey, TValue>(string filePath)  // ���ļ��м����ֵ�
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


    public static int CountValues<T>(Dictionary<string, T> dict, T value)  // �����ֵ���ֵΪvalue�ļ�ֵ�Ե�����
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


public static void SaveDictionaryToFile<TKey, TValue>(Dictionary<TKey, TValue> dictionary, string filePath)   // ���ֵ䱣�浽�ļ���(����ԭ���ļ�)
    {
        using(StreamWriter writer = new StreamWriter(filePath))
        {
            foreach(var kvp in dictionary)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value}");
            }
        }

    }

    public static bool ModifyDictionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue newValue)  // �޸��ֵ��е�ֵ����������ڣ�
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
