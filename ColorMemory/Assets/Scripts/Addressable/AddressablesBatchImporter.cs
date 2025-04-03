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
        string folderPath = "Assets/Sprites/Profile"; // 어드레서블에 추가할 폴더 경로
        string groupName = "ProfileIcons"; // 추가할 어드레서블 그룹 이름

        // 어드레서블 설정 가져오기
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("AddressableAssetSettings not found!");
            return;
        }

        // 그룹 가져오기 또는 생성
        AddressableAssetGroup group = settings.FindGroup(groupName) ?? settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema));

        // 폴더 내의 모든 파일 가져오기
        string[] files = Directory.GetFiles(folderPath)
            .Where(f => !f.EndsWith(".meta")) // 메타파일 제외
            .OrderBy(f => f) // 파일 이름 순 정렬
            .ToArray();

        for (int i = 0; i < files.Length; i++)
        {
            string assetPath = files[i].Replace("\\", "/"); // 윈도우 경로 처리
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(assetPath), group);
            entry.address = (i + 1).ToString(); // 1부터 시작하는 인덱스 부여
        }

        // 변경 사항 저장
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true);
        AssetDatabase.SaveAssets();
        Debug.Log("Addressables 등록 완료");
    }
}
