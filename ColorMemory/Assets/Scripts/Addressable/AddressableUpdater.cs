using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Linq;

public class AddressableUpdater : MonoBehaviour
{
    List<AssetLabelReference> _assetLabels;

    public void Initialize(List<AssetLabelReference> assetLabels)
    {
        _patchSize = 0;
        _patchMap = new Dictionary<string, long>();
        _assetLabels = assetLabels;
    }

    public void UpdateAddressable(Action OnCompleted)
    {
        this.OnCompleted = OnCompleted;
        StartCoroutine(InitAddressable());
    }

    public void AddEvents(Action<string, List<string>> OnHavePatch, Action<float> OnProgress)
    {
        this.OnHavePatch = OnHavePatch;
        this.OnProgress = OnProgress;
    }

    Action OnCompleted;

    Action<string, List<string>> OnHavePatch;
    Action<float> OnProgress;

    long _patchSize;
    Dictionary<string, long> _patchMap;

    #region 패치 체크

    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;

        if(_assetLabels != null)
        {
            List<string> labels = new List<string>();

            for (int i = 0; i < _assetLabels.Count; i++)
            {
                labels.Add(_assetLabels[i].labelString);
            }

            StartCoroutine(CheckUpdateFiles(labels));
        }
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
            Debug.Log("패치가 필요함");

            string patchSize = FormatBytes(_patchSize);
            OnHavePatch?.Invoke(patchSize, labels);
            // 패치가 필요한 경우임
        }
        else
        {
            Debug.Log("패치 내역이 존재하지 않음");
            OnCompleted?.Invoke();
            // 필요 없다면 바로 완료시키기
        }
        // _patchSize 활용
    }

    public string FormatBytes(long bytes)
    {
        const long KB = 1024;
        const long MB = KB * 1024;
        const long GB = MB * 1024;

        if (bytes >= GB)
            return $"{(bytes / (float)GB):0.##} GB";
        else if (bytes >= MB)
            return $"{(bytes / (float)MB):0.##} MB";
        else if (bytes >= KB)
            return $"{(bytes / (float)KB):0.##} KB";
        else
            return $"{bytes} B";
    }


    #endregion


    #region 패치 진행

    public void PatchFiles(List<string> labels)
    {
        StartCoroutine(PatchFilesCo(labels));
    }

    // 확인 버튼에 등록해두기
    IEnumerator PatchFilesCo(List<string> labels)
    {
        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            yield return handle;

            if (handle.Result != decimal.Zero)
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
            yield return new WaitForEndOfFrame(); // 연산 소모 감소
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
                OnCompleted?.Invoke(); // 완료
                break;
            }

            total = 0f;
            yield return new WaitForEndOfFrame(); // 다음 프레임에서 다운로드 상태 체크
        }
    }

    #endregion
}
