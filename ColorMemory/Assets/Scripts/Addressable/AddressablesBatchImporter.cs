using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AddressablesBatchImporter
{
    [MenuItem("Tools/Add Files to Addressables")]
    public static void AddFilesToAddressables()
    {
        string folderPath = "Assets/Sprites/Profile"; // ��巹���� �߰��� ���� ���
        string groupName = "ProfileIcons"; // �߰��� ��巹���� �׷� �̸�

        // ��巹���� ���� ��������
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("AddressableAssetSettings not found!");
            return;
        }

        // �׷� �������� �Ǵ� ����
        AddressableAssetGroup group = settings.FindGroup(groupName) ?? settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema));

        // ���� ���� ��� ���� ��������
        string[] files = Directory.GetFiles(folderPath)
            .Where(f => !f.EndsWith(".meta")) // ��Ÿ���� ����
            .OrderBy(f => f) // ���� �̸� �� ����
            .ToArray();

        for (int i = 0; i < files.Length; i++)
        {
            string assetPath = files[i].Replace("\\", "/"); // ������ ��� ó��
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(assetPath), group);
            entry.address = (i + 1).ToString(); // 1���� �����ϴ� �ε��� �ο�
        }

        // ���� ���� ����
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true);
        AssetDatabase.SaveAssets();
        Debug.Log("Addressables ��� �Ϸ�");
    }
}
