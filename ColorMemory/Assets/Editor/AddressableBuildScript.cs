using System;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets; // Debug.Log�� ����ϱ� ���� �ʿ�

namespace UnityBuilderAction
{
    public class AddressableBuildScript
    {
        /// <summary>
        /// GameCI �� CI ȯ�濡�� -executeMethod ���ڷ� ȣ��� Addressables ���� �޼���
        /// </summary>
        public static void PerformBuild()
        {
            Debug.Log("AddressableBuildScript.PerformBuild: Addressables ���带 �����մϴ�.");

            try
            {
                // Addressables ���� ����
                // BuildPlayerContent()�� Addressables ������ ������� ���带 �����մϴ�.
                // ���� ����� AddressableAssetSettings ������Ʈ�� ������ Build Path�� ����˴ϴ�.
                AddressableAssetSettings.CleanPlayerContent();
                AddressableAssetSettings.BuildPlayerContent();

                Debug.Log("AddressableBuildScript.PerformBuild: Addressables ���尡 ���������� �Ϸ�Ǿ����ϴ�.");
            }
            catch (Exception ex)
            {
                // ���� �� ���� �߻� �� ���� ó��
                Debug.LogError($"AddressableBuildScript.PerformBuild: Addressables ���� �� ���� �߻�: {ex.Message}");
                // CI ������������ �����ϵ��� ���� �ڵ带 ��ȯ�ϰų� throw �� �� �ֽ��ϴ�.
                // Application.Quit(1); // ����Ƽ �����͸� ���� �ڵ�� ���� (�ʿ� �� ���)
                throw; // ���ܸ� �ٽ� ������ CI �ý����� ���и� �����ϰ� ��
            }
        }

        // �ʿ��ϴٸ� ������ �޴� �������� �߰��Ͽ� ���� ���� �׽�Ʈ ����
        // [UnityEditor.MenuItem("Addressables/Build Player Content via Script")]
        // public static void BuildManually()
        // {
        //     PerformBuild();
        // }
    }
}