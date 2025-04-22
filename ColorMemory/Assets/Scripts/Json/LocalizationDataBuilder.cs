using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationDataBuilder : SheetDataBuilder
{
    Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> ParseTsv(string tsv)
    {
        Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> word =
            new Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>>();

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;


        string[] firstColumn = row[0].Split('\t');
        ILocalization.Language[] languages = new ILocalization.Language[columnSize - 1]; // key 값은 빼준다.
        for (int i = 1; i < columnSize; i++)
        {
            languages[i - 1] = (ILocalization.Language)Enum.Parse(typeof(ILocalization.Language), firstColumn[i]);
        }

        // 첫 번째 줄은 헤더로 무시
        for (int i = 1; i < rowSize; i++)
        {
            string[] columns = row[i].Split('\t');

            ILocalization.Key key = (ILocalization.Key)Enum.Parse(typeof(ILocalization.Key), columns[0]);
            word[key] = new Dictionary<ILocalization.Language, string>();

            for (int j = 1; j < columnSize; j++)
            {
                int languageIndex = j - 1;
                word[key].Add(languages[languageIndex], columns[j].Trim());
            }
        }

        return word;
    }

    [ContextMenu("CreateData")]
    public override void CreateData()
    {
        FileIO fileIO = new FileIO(new JsonParser(), ".txt");

        StartCoroutine(Load(_address, _sheetID, (string tsv) => {

            Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> word = ParseTsv(tsv);

            Localization localization = new Localization(word);
            fileIO.SaveData(localization, _fileLocation, _fileName, true);
        }));
    }
}
