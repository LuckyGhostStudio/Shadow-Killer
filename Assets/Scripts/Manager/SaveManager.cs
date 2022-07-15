using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private string sceneName = "";

    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);    //场景加载时不销毁
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMain();    //回到主场景
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            Debug.Log("数据已保存");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
            Debug.Log("数据已加载");
        }
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="data">要保存的对象数据</param>
    /// <param name="key">此对象的键</param>
    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);    //转为Json字符串
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);   //保存当前场景名字
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="data">写回的对象数据</param>
    /// <param name="key">健</param>
    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);    //读取key的value，写入data
        }
    }
}
