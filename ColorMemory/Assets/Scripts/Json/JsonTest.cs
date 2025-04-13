using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    //[SerializeField] TextAsset _textAsset;

    [ContextMenu("Json Test")]
    public void Test()
    {
        JsonParser parser = new JsonParser();
        FileIO fileIO = new FileIO(parser, ".txt");
        fileIO.SaveData(GetArtData(), "JsonDatas", "ArtworkData", true);
    }

    public ArtworkDataObject GetArtData()
    {
        Dictionary<ArtName, ArtData> artDatas = new Dictionary<ArtName, ArtData>
        {
            { ArtName.ABlossomingBush , new ArtData(Rank.COPPER, "A Blossoming Bush", "A landscape painting by Hugo Darnaut depicting a vibrant bush in full bloom.") },
            { ArtName.AChristmasRepast , new ArtData(Rank.COPPER, "A Christmas Repast", "A warm depiction of a family gathered for a festive meal, painted by Stanhope Alexander Forbes in 1913.") },
            { ArtName.ACoastalLandscapeintheSouthofFrance , new ArtData(Rank.COPPER, "A Coastal Landscape in the South of France", "A serene painting by Eugène Robert capturing the natural beauty of the French coastline.") },
            { ArtName.ACottageGardenWithChickens , new ArtData(Rank.COPPER, "A Cottage Garden With Chickens", "Peder Mørk Mønsted's 1919 masterpiece showcasing a peaceful rural garden with free-roaming chickens.") },
            { ArtName.ADoewithFawn , new ArtData(Rank.COPPER, "A Doe with Fawn", "Carl Schweninger Jr. paints a tender moment between a doe and her fawn in a lush natural setting.") },
            { ArtName.AFavoriteSummerPastime , new ArtData(Rank.COPPER, "A Favorite Summer Pastime", "An 1873 artwork by Henry Joseph Thouron illustrating leisurely summer activities of the time.") },
            { ArtName.AForestPathwithHunteratSunset , new ArtData(Rank.COPPER, "A Forest Path with Hunter at Sunset", "Désiré Thomassin captures a hunter walking along a forest path bathed in golden sunset light.") },
            { ArtName.AFreshBreeze , new ArtData(Rank.COPPER, "A Fresh Breeze", "George Wesley Bellows' 1913 painting vividly portraying the movement of wind over the sea.") },
            { ArtName.AGardenIdyll , new ArtData(Rank.COPPER, "A Garden Idyll", "Hugo Charlemont’s enchanting scene of a beautiful and tranquil garden filled with colorful flowers.") },
            { ArtName.AGardeninSeptember , new ArtData(Rank.COPPER, "A Garden in September", "Mary Hiester Reid’s late 19th-century artwork depicting the seasonal transition in a garden setting.") },
        };

        return new ArtworkDataObject(artDatas);
    }
}
