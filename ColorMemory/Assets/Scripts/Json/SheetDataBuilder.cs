using Challenge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class SheetDataBuilder : MonoBehaviour
{
    [SerializeField] protected string _address = "";
    [SerializeField] protected int _sheetID = 0;

    [SerializeField] protected string _fileName = "";
    [SerializeField] protected string _fileLocation = "";

    FileIO _fileIO = new FileIO(new JsonParser(), ".txt");

    string GetURL(string address) { return $"{address}/export?format=tsv"; }
    string GetURL(string address, int sheetID) { return $"{address}/export?format=tsv&gid={sheetID}"; }

    protected IEnumerator Load(string address, System.Action<string> OnComplete)
    {
        UnityWebRequest request = UnityWebRequest.Get(GetURL(address));
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            OnComplete?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(request.result);
        }
    }

    protected IEnumerator Load(string address, int sheetID, System.Action<string> OnComplete)
    {
        UnityWebRequest request = UnityWebRequest.Get(GetURL(address, sheetID));
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            OnComplete?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(request.result);
        }
    }

    public abstract void CreateData();
}
