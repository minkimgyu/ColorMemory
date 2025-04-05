#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

[InitializeOnLoad]
public static class AddGameViewResolutions
{
    static AddGameViewResolutions()
    {
        AddCustomResolutions();
    }

    private static void AddCustomResolutions()
    {
        //// Galaxy S 시리즈
        //AddResolution("Galaxy S3", 720, 1280);
        //AddResolution("Galaxy S4", 1080, 1920);
        //AddResolution("Galaxy S5", 1080, 1920);
        //AddResolution("Galaxy S6", 1440, 2560);
        //AddResolution("Galaxy S7", 1440, 2560);
        //AddResolution("Galaxy S8", 1440, 2960);
        //AddResolution("Galaxy S9", 1440, 2960);
        //AddResolution("Galaxy S10", 1440, 3040);
        //AddResolution("Galaxy S20", 1440, 3200);
        //AddResolution("Galaxy S20 Ultra", 1440, 3200);
        //AddResolution("Galaxy S21", 1080, 2400);
        //AddResolution("Galaxy S21+", 1080, 2400);
        //AddResolution("Galaxy S21 Ultra", 1440, 3200);
        //AddResolution("Galaxy S22", 1080, 2340);
        //AddResolution("Galaxy S22+", 1080, 2340);
        //AddResolution("Galaxy S22 Ultra", 1440, 3088);
        //AddResolution("Galaxy S23", 1080, 2340);
        //AddResolution("Galaxy S23+", 1080, 2340);
        //AddResolution("Galaxy S23 Ultra", 1440, 3088);
        //AddResolution("Galaxy S24", 1080, 2340);
        //AddResolution("Galaxy S24+", 1440, 3120);
        //AddResolution("Galaxy S24 Ultra", 1440, 3120);

        //// Galaxy Note 시리즈
        //AddResolution("Galaxy Note 4", 1440, 2560);
        //AddResolution("Galaxy Note 5", 1440, 2560);
        //AddResolution("Galaxy Note 8", 1440, 2960);
        //AddResolution("Galaxy Note 9", 1440, 2960);
        //AddResolution("Galaxy Note 10", 1080, 2280);
        //AddResolution("Galaxy Note 10+", 1440, 3040);
        //AddResolution("Galaxy Note 20", 1080, 2400);
        //AddResolution("Galaxy Note 20 Ultra", 1440, 3088);

        //// Galaxy Z Fold 시리즈
        //AddResolution("Galaxy Z Fold", 840, 2260);            // 1st gen folded
        //AddResolution("Galaxy Z Fold", 1536, 2152);           // 1st gen unfolded
        //AddResolution("Galaxy Z Fold 2 (Folded)", 816, 2260);
        //AddResolution("Galaxy Z Fold 2 (Unfolded)", 1768, 2208);
        //AddResolution("Galaxy Z Fold 3 (Folded)", 832, 2268);
        //AddResolution("Galaxy Z Fold 3 (Unfolded)", 1768, 2208);
        //AddResolution("Galaxy Z Fold 4 (Folded)", 904, 2316);
        //AddResolution("Galaxy Z Fold 4 (Unfolded)", 1812, 2176);
        //AddResolution("Galaxy Z Fold 5 (Folded)", 904, 2316);
        //AddResolution("Galaxy Z Fold 5 (Unfolded)", 1812, 2176);

        //// Galaxy Tab 시리즈
        //AddResolution("Galaxy Tab S6", 1600, 2560);
        //AddResolution("Galaxy Tab S7", 1600, 2560);
        //AddResolution("Galaxy Tab S7+", 1752, 2800);
        //AddResolution("Galaxy Tab S8", 1600, 2560);
        //AddResolution("Galaxy Tab S8+", 1752, 2800);
        //AddResolution("Galaxy Tab S8 Ultra", 1848, 2960);
        //AddResolution("Galaxy Tab S9", 1600, 2560);
        //AddResolution("Galaxy Tab S9+", 1752, 2800);
        //AddResolution("Galaxy Tab S9 Ultra", 1848, 2960);
    }


    private static void AddResolution(string label, int width, int height)
    {
        var T = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(Editor).Assembly.GetType("UnityEditor.ScriptableSingleton`1")
            .MakeGenericType(T);
        var instanceProp = singleType.GetProperty("instance");
        var instance = instanceProp.GetValue(null, null);

        var groupType = T.GetMethod("GetGroup");
        var group = groupType.Invoke(instance, new object[] { (int)GameViewSizeGroupType.Standalone });

        var addCustomSize = group.GetType().GetMethod("AddCustomSize");
        var gameViewSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
        var gameViewSizeTypeEnum = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");

        var ctor = gameViewSizeType.GetConstructor(new[] {
            gameViewSizeTypeEnum,
            typeof(int),
            typeof(int),
            typeof(string)
        });

        var fixedResolutionEnum = Enum.Parse(gameViewSizeTypeEnum, "FixedResolution");
        var newSize = ctor.Invoke(new object[] { fixedResolutionEnum, width, height, label });

        // 중복 방지: 이미 있으면 추가 안 함
        var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
        var displayTexts = (string[])getDisplayTexts.Invoke(group, null);
        foreach (var text in displayTexts)
        {
            if (text.Contains(label))
                return;
        }

        addCustomSize.Invoke(group, new object[] { newSize });

        Debug.Log($"✅ Added resolution: {label} ({width}x{height})");
    }
}
#endif