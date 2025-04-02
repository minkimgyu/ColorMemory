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
    public override void CreateData()
    {
        FileIO fileIO = new FileIO(new JsonParser(), ".txt");

        StartCoroutine(Load(_address, _sheetID, (string tsv) => {

            List<ChallengeMode.StageData> stageDatas = ParseTsv(tsv);

            ChallengeMode.StageDataWrapper stageDataWrapper = new ChallengeMode.StageDataWrapper(stageDatas);
            fileIO.SaveData(stageDataWrapper, _fileLocation, _fileName, true);
        }));
    }
}
