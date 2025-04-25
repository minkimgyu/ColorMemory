using System;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets; // Debug.Log를 사용하기 위해 필요

namespace UnityBuilderAction
{
    public class AddressableBuildScript
    {
        /// <summary>
        /// GameCI 등 CI 환경에서 -executeMethod 인자로 호출될 Addressables 빌드 메서드
        /// </summary>
        public static void PerformBuild()
        {
            Debug.Log("AddressableBuildScript.PerformBuild: Addressables 빌드를 시작합니다.");

            try
            {
                // Addressables 빌드 시작
                // BuildPlayerContent()는 Addressables 설정을 기반으로 빌드를 수행합니다.
                // 빌드 결과는 AddressableAssetSettings 오브젝트에 설정된 Build Path에 저장됩니다.
                AddressableAssetSettings.CleanPlayerContent();
                AddressableAssetSettings.BuildPlayerContent();

                Debug.Log("AddressableBuildScript.PerformBuild: Addressables 빌드가 성공적으로 완료되었습니다.");
            }
            catch (Exception ex)
            {
                // 빌드 중 오류 발생 시 예외 처리
                Debug.LogError($"AddressableBuildScript.PerformBuild: Addressables 빌드 중 오류 발생: {ex.Message}");
                // CI 파이프라인이 실패하도록 에러 코드를 반환하거나 throw 할 수 있습니다.
                // Application.Quit(1); // 유니티 에디터를 에러 코드로 종료 (필요 시 사용)
                throw; // 예외를 다시 던져서 CI 시스템이 실패를 감지하게 함
            }
        }

        // 필요하다면 에디터 메뉴 아이템을 추가하여 수동 빌드 테스트 가능
        // [UnityEditor.MenuItem("Addressables/Build Player Content via Script")]
        // public static void BuildManually()
        // {
        //     PerformBuild();
        // }
    }
}