using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Linq;

public class AddressableUpdater : MonoBehaviour
{
    public void Initialize()
    {
        _patchSize = 0;
        _patchMap = new Dictionary<string, long>();
    }

    public void UpdateAddressable(Action OnCompleted)
    {
        this.OnCompleted = OnCompleted;
        StartCoroutine(InitAddressable());
    }

    public void AddEvents(Action<double, List<string>> OnHavePatch, Action<float> OnProgress)
    {
        this.OnHavePatch = OnHavePatch;
        this.OnProgress = OnProgress;
    }

    Action OnCompleted;

    Action<double, List<string>> OnHavePatch;
    Action<float> OnProgress;

    long _patchSize;
    Dictionary<string, long> _patchMap;

    #region ��ġ üũ

    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;

        StartCoroutine(GetAllLabels((labels) => // ��� ���� �޾Ƽ� �Ѱ��ֱ�
        {
            if(labels != null)
            {
                StartCoroutine(CheckUpdateFiles(labels));
            }
        }));
    }

    IEnumerator CheckUpdateFiles(List<string> labels)
    {
        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            yield return handle;

            _patchSize += handle.Result;
        }

        if(_patchSize > decimal.Zero)
        {
            OnHavePatch?.Invoke(_patchSize, labels);
            // ��ġ�� �ʿ��� �����
        }
        else
        {
            OnCompleted?.Invoke();
            // �ʿ� ���ٸ� �ٷ� �Ϸ��Ű��
        }
        // _patchSize Ȱ��
    }

    IEnumerator GetAllLabels(Action<List<string>> OnCompleted)
    {
        // ��� ���ҽ� �����̼� �ҷ�����
        var locationsHandle = Addressables.LoadResourceLocationsAsync((object)null, typeof(UnityEngine.Object));
        yield return locationsHandle;

        if (locationsHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            var locations = locationsHandle.Result;

            HashSet<string> allLabels = new HashSet<string>();

            foreach (var location in locations)
            {
                allLabels.Add(location.PrimaryKey);
            }

            OnCompleted?.Invoke(new List<string>(allLabels));
        }
        else
        {
            Debug.LogError("���ҽ� �����̼� �ε� ����");
            OnCompleted?.Invoke(null);
        }

        Addressables.Release(locationsHandle);
    }

    #endregion




    #region ��ġ ����

    public void PatchFiles(List<string> labels)
    {
        StartCoroutine(PatchFilesCo(labels));
    }

    // Ȯ�� ��ư�� ����صα�
    IEnumerator PatchFilesCo(List<string> labels)
    {
        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            yield return handle;

            if(handle.Result != decimal.Zero)
            {
                StartCoroutine(DownloadLable(label));
            }
        }

        yield return CheckDownload();
    }

    IEnumerator DownloadLable(string label)
    {
        _patchMap.Add(label, 0);
        var handle = Addressables.DownloadDependenciesAsync(label);

        while (!handle.IsDone)
        {
            _patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame(); // ���� �Ҹ� ����
        }

        _patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }

    IEnumerator CheckDownload()
    {
        float total = 0f;

        while (true)
        {
            total += _patchMap.Sum(tmp => tmp.Value);
            OnProgress?.Invoke(total / _patchSize);

            if (total == _patchSize)
            {
                OnCompleted?.Invoke(); // �Ϸ�
            }

            total = 0f;
            yield return new WaitForEndOfFrame(); // ���� �����ӿ��� �ٿ�ε� ���� üũ
        }
    }

    #endregion
}
