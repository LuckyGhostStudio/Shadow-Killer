using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;  //Player������

    public CinemachineVirtualCamera followCamera;   //����Player����� 

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();     //���й۲���

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);    //���س���ʱ������this
    }

    /// <summary>
    /// ��Player������ע�ᵽGameManeger
    /// </summary>
    /// <param name="player">Player����</param>
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;

        followCamera = FindObjectOfType<CinemachineVirtualCamera>();    //���ҳ����е�CinemachineVirtualCamera

        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform;    //�������ΪPlayer
        }
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

    /// <summary>
    /// �����ڣ�Player������
    /// </summary>
    /// <returns></returns>
    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == DestinationTag.ENTER) return item.transform;     //Ŀ���TagΪENTER
        }

        return null;
    }
}
