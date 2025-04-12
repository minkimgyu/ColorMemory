using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SpawnableUI : MonoBehaviour
{
    public enum Name
    {
        ArtworkUI,
        ClearPatternUI,
        RankingUI,
        StageSelectBtnUI,
        ShopBundleUI,
        FilteredArtworkUI,
        FilterItemUI
    }

    public virtual void Initialize(string title, string description, int reward, int price) { }
    public virtual void Initialize(Sprite profileSprite, string title, int score, int rank) { }


    public virtual void Initialize(Dictionary<NetworkService.DTO.Rank, Sprite> StageRankIconAssets) { }


    public virtual void Initialize(Sprite artSprite, string description) { }
    public virtual void Initialize(string description) { }

    public virtual void Initialize(Sprite artSprite, string title, bool hasIt = true) { }
    public virtual void Initialize(Sprite artSprite, Sprite artFrameSprite, Sprite rankIconSprite, bool hasIt = true) { }
    public virtual void Initialize(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors) { }
    public virtual void Initialize(int currentStageCount, MapData data, Color[] pickColors) { }
    public virtual void Initialize() { }

    public virtual void InjectClickEvent(System.Action<int> OnClick) { }
    public virtual void InjectClickEvent(System.Action OnClick) { }

    public virtual void SetRank(NetworkService.DTO.Rank rank) { }
    public virtual void SetState(StageUI.State state) { }

    public virtual void ChangeSelect(bool select) { }
    public virtual void ChangeScale(Vector3 scale) { }

    public virtual void ResetPosition() { }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
