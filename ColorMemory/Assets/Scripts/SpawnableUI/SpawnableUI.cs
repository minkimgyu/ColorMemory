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
    }

    public virtual void Initialize(Sprite profileSprite, string title, int score, int rank) { }
    public virtual void Initialize(Sprite artSprite, Sprite artFrameSprite) { }
    public virtual void Initialize(int currentStageCount, int totalStageCount, MapData data, Color[] pickColors) { }

    public virtual void Initialize(
       List<List<CollectiveArtData.Block>> blocks,
       List<List<CollectiveArtData.Color>> usedColors)
    { }

    public virtual void InjectClickEvent(System.Action<Vector2Int> OnClick) { }
    public virtual void InjectClickEvent(System.Action OnClick) { }

    public virtual void SetState(StageUI.State state) { }

    public virtual void ChangeIndex(Vector2Int index) { }
    public virtual void ChangeSelect(bool select) { }
    public virtual void ChangeScale(Vector3 scale, float ratio) { }

    public virtual void ResetPosition() { }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
