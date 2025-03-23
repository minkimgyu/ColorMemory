using UnityEngine;

abstract public class GameMode : MonoBehaviour
{
    public enum Type
    {
        Challenge,
        Collect
    }

    protected virtual void Start()
    {
        Initialize();
    }

    public abstract void Initialize();
    //public abstract void OnGameClear();
    //public abstract void OnGameFail();
}
