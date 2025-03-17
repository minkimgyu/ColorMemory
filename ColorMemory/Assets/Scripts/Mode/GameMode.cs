using UnityEngine;

abstract public class GameMode : MonoBehaviour
{
    public enum Type
    {
        Challenge,
        Collect
    }

    public abstract void OnGameClear();
    public abstract void OnGameFail();
}
