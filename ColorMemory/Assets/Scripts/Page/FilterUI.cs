using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class FilterUI : MonoBehaviour
{
    public enum FilterType
    {
        Rank,
        Date
    }

    public enum RankFilter
    {
        None,
        All,
        NoClear,
        Bronze,
        Silver,
        Gold
    }

    public enum DateFilter
    {
        None,
        Old,
        New
    }

    [SerializeField] BottomSheetUI _bottomSheetUI;
    [SerializeField] Transform _filterItemParent;
    [SerializeField] Transform _filteredArtworkParent;

    public void Initialize()
    {
        _bottomSheetUI.Initialize();
    }

    public void Activate(bool active)
    {
        gameObject.SetActive(active);
    }

    public void ActivateBottomSheet(bool active)
    {
        _bottomSheetUI.ActivatePanel(active);
    }

    public void CloseBottomSheet()
    {
        _bottomSheetUI.ClosePanel();
    }

    public void DestroyFilteredArtwork()
    {
        for (int i = _filteredArtworkParent.childCount - 1; i >= 0; i--)
        {
            _filteredArtworkParent.GetChild(i).GetComponent<SpawnableUI>().DestroyObject();
        }
    }

    public void AddFilterItem(SpawnableUI spawnableUI)
    {
        spawnableUI.transform.SetParent(_filterItemParent);
        spawnableUI.transform.localScale = Vector3.one;
    }

    public void AddFilteredArtwork(SpawnableUI spawnableUI)
    {
        spawnableUI.transform.SetParent(_filteredArtworkParent);
        spawnableUI.transform.localScale = Vector3.one;
    }
}
