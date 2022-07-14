using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()      //子类中可调用和重写
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }

    public static bool IsInitialized    //获得instance是否为空
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()
    {
        //在当前对象销毁时，清空instance
        if(instance == this)
        {
            instance = null;
        }
    }
}
