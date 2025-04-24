//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

[InitializeOnLoad]
public static class AddGameViewResolutions
{
    static AddGameViewResolutions()
    {
        // Unity 켜질 때 자동 실행
        AddAllResolutions();
    }

    [MenuItem("Tools/Add Galaxy Device Resolutions %#g")]
    public static void AddAllResolutions()
    {
        var groupType = GetCurrentGroupType();
        AddCustomResolutions(groupType);
    }

    private static GameViewSizeGroupType GetCurrentGroupType()
    {
        var buildTarget = EditorUserBuildSettings.activeBuildTarget;
        return buildTarget switch
        {
            BuildTarget.Android => GameViewSizeGroupType.Android,
            BuildTarget.iOS => GameViewSizeGroupType.iOS,
            BuildTarget.StandaloneWindows => GameViewSizeGroupType.Standalone,
            BuildTarget.StandaloneWindows64 => GameViewSizeGroupType.Standalone,
            BuildTarget.StandaloneOSX => GameViewSizeGroupType.Standalone,
            _ => GameViewSizeGroupType.Standalone
        };
    }

    private static void AddCustomResolutions(GameViewSizeGroupType groupType)
    {
        AddResolution(groupType, "IOS", 1206, 2622);

        // Galaxy S 시리즈
        AddResolution(groupType, "Galaxy S4", 1080, 1920);
        AddResolution(groupType, "Galaxy S5", 1080, 1920);
        AddResolution(groupType, "Galaxy S6", 1440, 2560);
        AddResolution(groupType, "Galaxy S7", 1440, 2560);
        AddResolution(groupType, "Galaxy S8", 1440, 2960);
        AddResolution(groupType, "Galaxy S9", 1440, 2960);
        AddResolution(groupType, "Galaxy S10", 1440, 3040);
        AddResolution(groupType, "Galaxy S20", 1440, 3200);
        AddResolution(groupType, "Galaxy S21", 1080, 2400);
        AddResolution(groupType, "Galaxy S22", 1080, 2340);
        AddResolution(groupType, "Galaxy S23", 1080, 2340);

        // Galaxy S 시리즈
        AddResolution(groupType, "Galaxy S24 Ultra", 1440, 3120);
        AddResolution(groupType, "Galaxy S24+", 1440, 3120);
        AddResolution(groupType, "Galaxy S24", 1080, 2340);
        AddResolution(groupType, "Galaxy S23 Ultra", 1440, 3088);
        AddResolution(groupType, "Galaxy S23+", 1080, 2340);
        AddResolution(groupType, "Galaxy S23", 1080, 2340);
        AddResolution(groupType, "Galaxy S22 Ultra", 1440, 3088);
        AddResolution(groupType, "Galaxy S22+", 1080, 2340);
        AddResolution(groupType, "Galaxy S22", 1080, 2340);
        AddResolution(groupType, "Galaxy S21 Ultra", 1440, 3200);
        AddResolution(groupType, "Galaxy S21+", 1080, 2400);
        AddResolution(groupType, "Galaxy S21", 1080, 2400);
        AddResolution(groupType, "Galaxy S20 Ultra", 1440, 3200);
        AddResolution(groupType, "Galaxy S20+", 1440, 3200);
        AddResolution(groupType, "Galaxy S20", 1440, 3200);

        // Galaxy Z Fold 시리즈 (펼친 상태 기준)
        AddResolution(groupType, "Galaxy Z Fold5", 1812, 2176);
        AddResolution(groupType, "Galaxy Z Fold4", 1812, 2176);
        AddResolution(groupType, "Galaxy Z Fold3", 1768, 2208);
        AddResolution(groupType, "Galaxy Z Fold2", 1768, 2208);
        AddResolution(groupType, "Galaxy Z Fold", 1536, 2152);

        // Galaxy Z Flip 시리즈
        AddResolution(groupType, "Galaxy Z Flip5", 1080, 2640);
        AddResolution(groupType, "Galaxy Z Flip4", 1080, 2640);
        AddResolution(groupType, "Galaxy Z Flip3", 1080, 2640);
        AddResolution(groupType, "Galaxy Z Flip", 1080, 2636);

        // Galaxy Note 시리즈
        AddResolution(groupType, "Galaxy Note20 Ultra", 1440, 3088);
        AddResolution(groupType, "Galaxy Note20", 1080, 2400);
        AddResolution(groupType, "Galaxy Note10+", 1440, 3040);
        AddResolution(groupType, "Galaxy Note10", 1080, 2280);
        AddResolution(groupType, "Galaxy Note9", 1440, 2960);
        AddResolution(groupType, "Galaxy Note8", 1440, 2960);

        // Galaxy Tab 시리즈
        AddResolution(groupType, "Galaxy Tab S9 Ultra", 1848, 2960);
        AddResolution(groupType, "Galaxy Tab S9+", 1752, 2800);
        AddResolution(groupType, "Galaxy Tab S9", 1600, 2560);
        AddResolution(groupType, "Galaxy Tab S8 Ultra", 1848, 2960);
        AddResolution(groupType, "Galaxy Tab S8+", 1752, 2800);
        AddResolution(groupType, "Galaxy Tab S8", 1600, 2560);
        AddResolution(groupType, "Galaxy Tab S7+", 1752, 2800);
        AddResolution(groupType, "Galaxy Tab S7", 1600, 2560);
        AddResolution(groupType, "Galaxy Tab S6", 1600, 2560);
        AddResolution(groupType, "Galaxy Tab S5e", 1600, 2560);
    }

    private static void AddResolution(GameViewSizeGroupType groupType, string label, int width, int height)
    {
        var T = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(Editor).Assembly.GetType("UnityEditor.ScriptableSingleton`1").MakeGenericType(T);
        var instance = singleType.GetProperty("instance").GetValue(null, null);

        var group = T.GetMethod("GetGroup").Invoke(instance, new object[] { (int)groupType });
        var addCustomSize = group.GetType().GetMethod("AddCustomSize");
        var gameViewSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
        var gameViewSizeTypeEnum = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");
        var ctor = gameViewSizeType.GetConstructor(new[] { gameViewSizeTypeEnum, typeof(int), typeof(int), typeof(string) });

        var fixedResolutionEnum = Enum.Parse(gameViewSizeTypeEnum, "FixedResolution");
        var newSize = ctor.Invoke(new object[] { fixedResolutionEnum, width, height, label });

        // 중복 방지
        var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
        var displayTexts = (string[])getDisplayTexts.Invoke(group, null);
        foreach (var text in displayTexts)
        {
            if (text.Contains(label))
                return;
        }

        addCustomSize.Invoke(group, new object[] { newSize });
        Debug.Log($"✅ Added GameView resolution: {label} ({width}x{height})");
    }
}
//#endif
