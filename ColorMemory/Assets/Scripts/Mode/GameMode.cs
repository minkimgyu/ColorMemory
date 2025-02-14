using UnityEngine;

abstract public class GameMode : MonoBehaviour
{
    public abstract void OnGameClear();
    public abstract void OnGameFail();
}
