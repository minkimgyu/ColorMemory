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
        AddResolution(groupType, "Galaxy S24 Ultra", 1440, 3120);
        AddResolution(groupType, "Galaxy S24+", 1440, 3120);
        AddResolution(groupType, "Galaxy S24", 1080, 2340);
        AddResolution(groupType, "Galaxy S23 Ultra", 1440, 3088);
        AddResolution(groupType, "Galaxy Z Fold 5 (Unfolded)", 1812, 2176);
        AddResolution(groupType, "Galaxy Tab S9 Ultra", 1848, 2960);
        // 필요 시 추가로 계속 작성
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
#endif
