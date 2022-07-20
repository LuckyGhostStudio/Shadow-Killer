using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    public GameObject playerPrefab;

    public SceneFader sceneFaderPrefab; //场景渐入渐出Prefab

    private GameObject player;

    //private bool fadeFinished;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);    //加载场景时不销毁this
    }

    private void Start()
    {
        GameManager.Instance.AddObservers(this);    //将this添加到观察者列表
        //fadeFinished = true;
    }

    /// <summary>
    /// 传送到目标点
    /// </summary>
    /// <param name="transitionPoint">传送起始点</param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionType.SameScene:      //同场景
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag)); //开启协程：传送（当前场景名字，传送起点标签）：起点和目标点标签一致
                break;
            case TransitionType.DifferentScene: //不同场景
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    /// <summary>
    /// 传送
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="destinationTag">目标点标签</param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        //SceneFader sceneFader = Instantiate(sceneFaderPrefab);
        //yield return StartCoroutine(sceneFader.FadeOut());  //场景渐出效果

        //TODO:保存数据
        SaveManager.Instance.SavePlayerData();  //保存Player数据

        if (SceneManager.GetActiveScene().name != sceneName)    //不同场景传送
        {
            yield return SceneManager.LoadSceneAsync(sceneName);    //异步加载场景
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);   //生成Player

            SaveManager.Instance.LoadPlayerData();  //读取Player数据

           // yield return StartCoroutine(sceneFader.FadeIn());  //场景渐入效果

            yield break;
        }
        else  //相同场景
        {
            player = GameManager.Instance.playerStats.gameObject;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);  //设置Player位置和旋转

            //yield return StartCoroutine(sceneFader.FadeIn());  //场景渐入效果

            yield return null;
        }

       
    }

    /// <summary>
    /// 获得传送目标点
    /// </summary>
    /// <param name="destinationTag">目标点标签</param>
    /// <returns></returns>
    private TransitionDestination GetDestination(DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();  //查找所有TransitionDestination

        foreach (var entrance in entrances)
        {
            if (entrance.destinationTag == destinationTag) return entrance;     //与当前目标点标签匹配
        }

        return null;
    }

    /// <summary>
    /// 传送到主场景
    /// </summary>
    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    /// <summary>
    /// 加载关卡：继续游戏
    /// </summary>
    public void TransitionToLoadLevel()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));  //加载保存的数据中的场景
    }

    /// <summary>
    /// 传送到第一关
    /// </summary>
    public void TransitonToFirstLevel()
    {
        StartCoroutine(LoadLevel("Room"));  //加载Room场景
    }

    /// <summary>
    /// 加载关卡
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <returns></returns>
    IEnumerator LoadLevel(string sceneName)
    {
        //SceneFader sceneFader = Instantiate(sceneFaderPrefab);

        if (sceneName != "")
        {
            //yield return StartCoroutine(sceneFader.FadeOut());  //场景渐出效果

            yield return SceneManager.LoadSceneAsync(sceneName);    //加载场景
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);  //在出生点生成Player

            //保存数据
            SaveManager.Instance.SavePlayerData();

            //yield return StartCoroutine(sceneFader.FadeIn());  //场景渐入效果

            yield break;    //结束协程
        }
    }

    /// <summary>
    /// 加载主场景
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMain()
    {
        //SceneFader sceneFader = Instantiate(sceneFaderPrefab);

        //yield return StartCoroutine(sceneFader.FadeOut());  //场景渐出效果
        yield return SceneManager.LoadSceneAsync("MainMenu");   //加载主场景
        //yield return StartCoroutine(sceneFader.FadeIn());  //场景渐入效果

        yield break;
    }

    public void EndNotify() //游戏结束通知
    {
        //if (fadeFinished)
        //{
        //    fadeFinished = false;
        //    StartCoroutine(LoadMain());
        //}
    }
}
