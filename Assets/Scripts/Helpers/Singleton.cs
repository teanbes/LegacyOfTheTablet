using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    static T _instance;

    public static T Instance
    {
        get 
        {
            if (_instance) 
                _instance = FindAnyObjectByType<T>();

            if (!_instance)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                _instance = obj.AddComponent<T>();
            }
            return _instance;   
        }
    }

    protected virtual void Awake()
    {
        if (!_instance)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
}
