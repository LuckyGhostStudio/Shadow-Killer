using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;  //Player的数据

    public CinemachineVirtualCamera followCamera;   //跟随Player的相机 

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();     //所有观察者

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);    //加载场景时不销毁this
    }

    /// <summary>
    /// 将Player的数据注册到GameManeger
    /// </summary>
    /// <param name="player">Player数据</param>
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;

        followCamera = FindObjectOfType<CinemachineVirtualCamera>();    //查找场景中的CinemachineVirtualCamera

        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform;    //跟随对象为Player
        }
    }

    /// <summary>
    /// 将实现IEndGameObserver接口的Enemy添加到广播列表
    /// </summary>
    /// <param name="observer"></param>
    public void AddObservers(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    //移除列表
    public void RemoveObservers(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    /// <summary>
    /// 通知所有Enemy GameOver执行EndNotify
    /// </summary>
    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        //暂停游戏等其他操作
        //跳出游戏结束界面
    }

    /// <summary>
    /// 获得入口：Player出生点
    /// </summary>
    /// <returns></returns>
    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == DestinationTag.ENTER) return item.transform;     //目标点Tag为ENTER
        }

        return null;
    }
}
