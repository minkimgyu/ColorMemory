using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IPoolObject
{
    Queue<T> _pool;
    GameObject _prefab;

    public void Initialize(GameObject prefab, int startPoolSize)
    {
        _pool = new Queue<T>();
        _prefab = prefab;
        for (int i = 0; i < startPoolSize; i++)
        {
            CreateItem();
        }
    }

    void CreateItem()
    {
        GameObject go = Instantiate(_prefab);
        T itemGO = go.GetComponent<T>();
        itemGO.InjectReturnToPoolEvent((value) => { DisableItem((T)value); });

        itemGO.SetParent(transform);
        _pool.Enqueue(itemGO);
        itemGO.Active(false);
    }

    public T GetItem()
    {
        if (_pool.Count == 0) CreateItem(); // 생성해준다.

        T itemGO = _pool.Dequeue();
        itemGO.Active(true);
        return itemGO;
    }

    public void DisableItem(T itemGO)
    {
        itemGO.SetParent(transform);
        itemGO.Active(false);
        _pool.Enqueue(itemGO);
    }
}
