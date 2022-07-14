using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /// <summary>
    /// ��Player������ע�ᵽGameManeger
    /// </summary>
    /// <param name="player">Player����</param>
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }

    /// <summary>
    /// ��ʵ��IEndGameObserver�ӿڵ�Enemy��ӵ��㲥�б�
    /// </summary>
    /// <param name="observer"></param>
    public void AddObservers(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    //�Ƴ��б�
    public void RemoveObservers(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    /// <summary>
    /// ֪ͨ����Enemy GameOverִ��EndNotify
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
        //��ͣ��Ϸ����������
        //������Ϸ��������
    }
}
