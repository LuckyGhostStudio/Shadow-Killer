using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    public GameObject playerPrefab;

    public SceneFader sceneFaderPrefab; //�������뽥��Prefab

    private GameObject player;

    //private bool fadeFinished;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);    //���س���ʱ������this
    }

    private void Start()
    {
        GameManager.Instance.AddObservers(this);    //��this��ӵ��۲����б�
        //fadeFinished = true;
    }

    /// <summary>
    /// ���͵�Ŀ���
    /// </summary>
    /// <param name="transitionPoint">������ʼ��</param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionType.SameScene:      //ͬ����
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag)); //����Э�̣����ͣ���ǰ�������֣���������ǩ��������Ŀ����ǩһ��
                break;
            case TransitionType.DifferentScene: //��ͬ����
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <param name="destinationTag">Ŀ����ǩ</param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        //SceneFader sceneFader = Instantiate(sceneFaderPrefab);
        //yield return StartCoroutine(sceneFader.FadeOut());  //��������Ч��

        //TODO:��������
        SaveManager.Instance.SavePlayerData();  //����Player����

        if (SceneManager.GetActiveScene().name != sceneName)    //��ͬ��������
        {
            yield return SceneManager.LoadSceneAsync(sceneName);    //�첽���س���
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);   //����Player

            SaveManager.Instance.LoadPlayerData();  //��ȡPlayer����

           // yield return StartCoroutine(sceneFader.FadeIn());  //��������Ч��

            yield break;
        }
        else  //��ͬ����
        {
            player = GameManager.Instance.playerStats.gameObject;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);  //����Playerλ�ú���ת

            //yield return StartCoroutine(sceneFader.FadeIn());  //��������Ч��

            yield return null;
        }

       
    }

    /// <summary>
    /// ��ô���Ŀ���
    /// </summary>
    /// <param name="destinationTag">Ŀ����ǩ</param>
    /// <returns></returns>
    private TransitionDestination GetDestination(DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();  //��������TransitionDestination

        foreach (var entrance in entrances)
        {
            if (entrance.destinationTag == destinationTag) return entrance;     //�뵱ǰĿ����ǩƥ��
        }

        return null;
    }

    /// <summary>
    /// ���͵�������
    /// </summary>
    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    /// <summary>
    /// ���عؿ���������Ϸ
    /// </summary>
    public void TransitionToLoadLevel()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));  //���ر���������еĳ���
    }

    /// <summary>
    /// ���͵���һ��
    /// </summary>
    public void TransitonToFirstLevel()
    {
        StartCoroutine(LoadLevel("Room"));  //����Room����
    }

    /// <summary>
    /// ���عؿ�
    /// </summary>
    /// <param name="sceneName">��������</param>
    /// <returns></returns>
    IEnumerator LoadLevel(string sceneName)
    {
        //SceneFader sceneFader = Instantiate(sceneFaderPrefab);

        if (sceneName != "")
        {
            //yield return StartCoroutine(sceneFader.FadeOut());  //��������Ч��

            yield return SceneManager.LoadSceneAsync(sceneName);    //���س���
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);  //�ڳ���������Player

            //��������
            SaveManager.Instance.SavePlayerData();

            //yield return StartCoroutine(sceneFader.FadeIn());  //��������Ч��

            yield break;    //����Э��
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMain()
    {
        //SceneFader sceneFader = Instantiate(sceneFaderPrefab);

        //yield return StartCoroutine(sceneFader.FadeOut());  //��������Ч��
        yield return SceneManager.LoadSceneAsync("MainMenu");   //����������
        //yield return StartCoroutine(sceneFader.FadeIn());  //��������Ч��

        yield break;
    }

    public void EndNotify() //��Ϸ����֪ͨ
    {
        //if (fadeFinished)
        //{
        //    fadeFinished = false;
        //    StartCoroutine(LoadMain());
        //}
    }
}
