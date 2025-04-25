using System;
using UnityEditor.AddressableAssets.Settings;

public static class AddressablesUpdateBuilder
{
    public static void BuildAddressablesUpdate()
    {
        AddressableAssetSettings.BuildPlayerContent(out var result);
        if (!string.IsNullOrEmpty(result.Error))
        {
            throw new Exception($"Addressables Build Error: {result.Error}");
        }
    }
}