using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonParser
{
    public T JsonToObject<T>(string json)
    {
        var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return JsonConvert.DeserializeObject<T>(json, setting);
    }

    public T JsonToObject<T>(TextAsset tmpAsset)
    {
        var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return JsonConvert.DeserializeObject<T>(tmpAsset.text, setting);
    }

    public string ObjectToJson(object objectToParse)
    {
        var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return JsonConvert.SerializeObject(objectToParse, setting);
    }
}