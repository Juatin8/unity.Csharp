using System.IO;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public static class JsonHelper
{
    // 加载 JSON 文件并返回 JSONNode 对象
    public static JSONNode LoadJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            return JSON.Parse(File.ReadAllText(filePath));
        }
        else
        {
            Debug.LogError($"JSON file not found at path: {filePath}");
            return null;
        }
    }

    public static Dictionary<string, string> LoadJson2Dict(string filePath)
    {
        var node = LoadJson(filePath);
        if (node == null) return new Dictionary<string, string>();

        var dict = new Dictionary<string, string>();
        foreach (var item in node)
        {
            dict[item.Key] = item.Value;  // 转换为字符串
        }

        return dict;
    }

    public static Dictionary<string, string> LoadStreamingAssetJson2Dict(string relativePath)
    {
        var path = Path.Combine(Application.streamingAssetsPath, relativePath);
        return LoadJson2Dict(path);
    } 
}
