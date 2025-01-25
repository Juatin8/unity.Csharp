using LeanCloud.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LeanCloudDataManager
{
    // ============================== 上传数据 =======================================
    public static async Task UploadData<T>(T instance, LCUser currentUser)      //上传一个类的数据
    {
        LCObject targetObject = new LCObject(instance.GetType().Name);
        targetObject["User"] = currentUser;
        var fields = ReflectionHelper.GetPublicFieldsFromInstance(instance);
        foreach (var field in fields)
        {
            targetObject[field.Name] = ReflectionHelper.AutoConvertValueToLeanCloudSupportType(field.GetValue(instance));
        }
        _ = LeanCloudDataHelper.TryUpload(targetObject);
    }

    public static async Task UploadFileData(string fileName, string filePath = null)     //上传文件
    {
        filePath ??= Path.Combine(Application.persistentDataPath, fileName);
        LCFile lcFile = new LCFile(fileName, filePath);
        _ = LeanCloudDataHelper.TryUpload(lcFile);
    }


    // ===================================== 同步 ========================================
    public static async Task UploadDataFromCSV<T>(T instance, LCUser currentUser, List<string> CSVDataList)      // 本地更加新的CSV数据上传到云端
    {
        var filePath = CSVManager.GenerateFilePathFromInstance(instance);
        foreach (var csvLine in CSVDataList)
        {
            var className = instance.GetType().Name;
            LCObject targetObject = new LCObject(className);
            var fields = ReflectionHelper.GetPublicFieldsFromInstance(instance);
            string[] variables = CSVHelper.SplitLine(csvLine);
            foreach (var field in fields)
            {
                int index = CSVHelper.GetIndexOfVariableInRow(filePath, field.Name);// 可以通过列的名称进行匹配（或者直接按顺序）
                if (index != -1 && index < variables.Length)
                {
                    string variable = variables[index];
                    if (field.Name == "UserID")
                    {
                        targetObject["User"] = LeanCloudDataHelper.GetUserByID(variable);  //这样一来，本地的userid就是和云端的user进行映射的，云端也不需要多余的userid
                    }
                    else
                    {
                        targetObject[field.Name] = ReflectionHelper.ConvertStringToFieldType(field.FieldType, variable);
                    }
                }
                else
                {
                    Debug.LogWarning($"字段 '{field.Name}' 在 CSV 中没有找到对应的列。");
                }
            }
            await LeanCloudDataHelper.TryUpload(targetObject);
        }
    }

    // ====================================== 查 ===========================================================
    public static async Task<LCObject> GetNewestUserData<T>(T instance, LCUser currentUser)      // 获得云端最新的一行用户数据
    {
        try
        {
            var className = instance.GetType().Name;
            LCQuery<LCObject> refineUserquery = CreateQueryForCurrentUser(className, currentUser);
            refineUserquery.OrderByDescending("createdAt");
            LCObject myObject = await refineUserquery.First();
            return myObject;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error getting object by ID: {ex.Message}");
            return null;
        }
    }
    public static async Task<ReadOnlyCollection<LCObject>> GetDataNewerThanLocal<T>(T instance, DateTime timeAtCSV, LCUser currentUser)   //获得比本地更加新的数据
    {
        try
        {
            var className = instance.GetType().Name;
            LCQuery<LCObject> refineUserquery = CreateQueryForCurrentUser(className, currentUser);
            LCQuery<LCObject> refineStartTimequery = new LCQuery<LCObject>(className);
            refineStartTimequery.WhereGreaterThan("StartTime", timeAtCSV);
            LCQuery<LCObject> query = LeanCloudDataHelper.CombineQueries(refineUserquery, refineStartTimequery);
            ReadOnlyCollection<LCObject> myObjects = await query.Find();
            Debug.Log(query.Count());
            return myObjects;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error getting object by ID: {ex.Message}");
            return null;
        }
    }


    public static LCQuery<LCObject> CreateQueryForCurrentUser(string className, LCUser currentUser)
    {
        LCQuery<LCObject> query = new LCQuery<LCObject>(className);
        query.WhereEqualTo("User", currentUser);
        return query;
    }

}


// 获得比本地新的云端数据
/*
public static async Task<ReadOnlyCollection<LCObject>> GetDataNewerThanLocal<T>(T instance, DateTime timeAtCSV)
{
    try
    {
        LCQuery<LCObject> query = new LCQuery<LCObject>(instance.GetType().Name);
        query.WhereGreaterThan("StartTime", timeAtCSV);
        ReadOnlyCollection<LCObject> myObjects = await query.Find();
        return myObjects;
    }
    catch (Exception ex)
    {
        Debug.LogError($"Error getting object by ID: {ex.Message}");
        return null;
    }
}
*/

