using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /// <summary>
    /// 将Player的数据注册到GameManeger
    /// </summary>
    /// <param name="player">Player数据</param>
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;
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
}
