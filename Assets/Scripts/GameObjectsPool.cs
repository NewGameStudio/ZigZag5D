using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsPool
{
    private GameObject _source;
    private Stack<GameObject> _instances;

    public GameObjectsPool(GameObject source, int instanceCount = 0)
    {
        _source = source;

        _instances = new Stack<GameObject>();

        for (int i = 0; i < instanceCount; i++)
            CreateInstance();
    }

    public GameObject Instantiate()
    {
        if (_instances.Count == 0)
            CreateInstance();

        GameObject instance = _instances.Pop();

        instance.SetActive(true);

        return instance;
    }

    public void Destroy(GameObject go)
    {
        go.SetActive(false);

        _instances.Push(go);
    }

    private void CreateInstance()
    {
        GameObject instance = Object.Instantiate(_source);

        instance.SetActive(false);

        _instances.Push(instance);
    }
}
