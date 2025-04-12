using Challenge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtworkDataBuilder : SheetDataBuilder
{
    Dictionary<int, ArtworkData> ParseTsv(string tsv)
    {
        Dictionary<int, ArtworkData> artworkDates = new Dictionary<int, ArtworkData>();

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        // 첫 번째 줄은 헤더로 무시
        for (int i = 1; i < rowSize; i++)
        {
            string[] columns = row[i].Split('\t');

            string artist, title, description;
            artist = columns[1];
            title = columns[2];
            description = columns[3];

            ArtworkData artworkData = new ArtworkData(artist, title, description);
            artworkDates.Add(i - 1, artworkData);
        }

        return artworkDates;
    }

    [ContextMenu("CreateData")]
    public override void CreateData()
    {
        FileIO fileIO = new FileIO(new JsonParser(), ".txt");

        StartCoroutine(Load(_address, _sheetID, (string tsv) => {

            Dictionary<int, ArtworkData> artworkDates = ParseTsv(tsv);

            ArtworkDateWrapper artworkDataWrapper = new ArtworkDateWrapper(artworkDates);
            fileIO.SaveData(artworkDataWrapper, _fileLocation, _fileName, true);
        }));
    }
}
