using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    [SerializeField] TextAsset _textAsset;

    [ContextMenu("Json Test")]
    public void Test()
    {
        JsonParser parser = new JsonParser();
        ArtData artData = parser.JsonToObject<ArtData>(_textAsset.text);

        Debug.Log(artData);
    }
}
