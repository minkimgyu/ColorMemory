using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;
using Challenge;
using System.Net;

public class ChallengeModeStageDataBuilder : SheetDataBuilder
{
    List<LevelData> ParseTsv(string tsv)
    {
        List<LevelData> stageDatas = new List<LevelData>();

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        // 첫 번째 줄은 헤더로 무시
        for (int i = 1; i < rowSize; i++)
        {
            string[] columns = row[i].Split('\t');

            int mapSize, colorCount, randomPointCount, memorizeDuration = 0;
            mapSize = int.Parse(columns[1]);
            colorCount = int.Parse(columns[2]);
            randomPointCount = int.Parse(columns[3]);
            memorizeDuration = int.Parse(columns[4]);

            LevelData stageData = new LevelData(mapSize, colorCount, randomPointCount, memorizeDuration);
            stageDatas.Add(stageData);
        }

        return stageDatas;
    }

    [ContextMenu("CreateData")]
    public override void CreateData()
    {
        FileIO fileIO = new FileIO(new JsonParser(), ".txt");

        StartCoroutine(Load(_address, _sheetID, (string tsv) => {

            List<LevelData> stageDatas = ParseTsv(tsv);

            LevelDataWrapper stageDataWrapper = new LevelDataWrapper(stageDatas);
            fileIO.SaveData(stageDataWrapper, _fileLocation, _fileName, true);
        }));
    }
}
