using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private string sceneName = "level";

    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);    //��������ʱ������
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            Debug.Log("�����ѱ���");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
            Debug.Log("�����Ѽ���");
        }
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
        //TODO:��ӱ���Playerλ����Ϣ�������еĵ�����Ϣ����Ʒ��Ϣ��
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="data">Ҫ����Ķ�������</param>
    /// <param name="key">�˶���ļ�</param>
    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);    //תΪJson�ַ���
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);   //���浱ǰ��������
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="data">д�صĶ�������</param>
    /// <param name="key">��</param>
    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);    //��ȡkey��value��д��data
        }
    }
}
