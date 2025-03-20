using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FileIO
{
    JsonParser _parser;
    string _extension;

    public FileIO(JsonParser parser, string extension)
    {
        _parser = parser;
        _extension = extension;
        // Application.dataPath + filePath + fileName + _extension
    }

    public bool HaveFile(string path)
    {
        return File.Exists(path);
    }

    string ReturnPath(string filePath, string fileName)
    { 
        return $"{Application.dataPath}/{filePath}/{fileName}{_extension}"; 
    }

    public T LoadData<T>(string jData)
    {
        return _parser.JsonToObject<T>(jData);
    }

    public T LoadData<T>(string filePath, string fileName)
    {
        string path = ReturnPath(filePath, fileName);
        if (File.Exists(path) == false) return default;

        string jData = File.ReadAllText(path);
        return _parser.JsonToObject<T>(jData);
    }

    public void SaveData(object objectToParse, string filePath, string fileName, bool canOverwrite = false)
    {
        string path;
        path = ReturnFilePath(filePath, fileName, canOverwrite); // ��ġ�� �ʴ� �̸��� ã��

        Debug.Log(path);

        string jsonAsset = _parser.ObjectToJson(objectToParse);

        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonAsset);
        //string encodedJson = System.Convert.ToBase64String(bytes);
        //File.WriteAllText(path, encodedJson); // �̷� ������� ����������

        File.WriteAllText(path, jsonAsset); // �̷� ������� ����������

#if UNITY_ANDROID && UNITY_EDITOR
        AssetDatabase.Refresh();
#elif UNITY_ANDROID
#endif
    }

    string ReturnFilePath(string filePath, string fileName, bool canOverwrite = false)
    {
        string path = ReturnPath(filePath, fileName);
        if (canOverwrite || HaveFile(path) == false) return path;

        Debug.LogError("�̹� �ش� ��ο� ������ ������");

        string originName = fileName;
        int index = 0;

        do
        {
            if (index > 0) originName = $"{originName} {index}";

            path = ReturnPath(filePath, originName);
            if (HaveFile(path) == false) break; // �������� ������ break
            else index++;
        }
        while (true);

        return path;
    }
}