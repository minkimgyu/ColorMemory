using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;
using Challenge;

public class ChallengeModeStageDataBuilder : MonoBehaviour
{
    const string URL = "https://docs.google.com/spreadsheets/d/1YMPCChQ5VHH_Gg7RgglN4P8YZzTqNt5HkDNI5ohNhj4/export?format=tsv";

    IEnumerator Load(System.Action<string> OnComplete)
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            OnComplete?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(request.result);
        }
    }

    List<ChallengeMode.StageData> ParseTsv(string tsv)
    {
        List<ChallengeMode.StageData> stageDatas = new List<ChallengeMode.StageData>();

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        // 첫 번째 줄은 헤더로 무시
        for (int i = 1; i < rowSize; i++)
        {
            string[] columns = row[i].Split('\t');

            int mapSize, colorCount, randomPointCount = 0;
            mapSize = int.Parse(columns[1]);
            colorCount = int.Parse(columns[2]);
            randomPointCount = int.Parse(columns[3]);

            ChallengeMode.StageData stageData = new ChallengeMode.StageData(mapSize, colorCount, randomPointCount);
            stageDatas.Add(stageData);
        }

        return stageDatas;
    }

    [ContextMenu("CreateData")]
    public void CreateData()
    {
        FileIO fileIO = new FileIO(new JsonParser(), ".txt");

        string fileName = "ChallengeModeLevelDataAsset";
        string fileLocation = "JsonDatas";

        StartCoroutine(Load((string tsv) => {

            List<ChallengeMode.StageData> stageDatas = ParseTsv(tsv);

            ChallengeMode.StageDataWrapper stageDataWrapper = new ChallengeMode.StageDataWrapper(stageDatas);
            fileIO.SaveData(stageDataWrapper, fileLocation, fileName, true);
        }));
    }
}
