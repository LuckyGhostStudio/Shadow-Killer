using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button newGameButton;
    private Button continueButton;
    private Button authorButton;
    private Button quitButton;

    private void Awake()
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueButton = transform.GetChild(2).GetComponent<Button>();
        authorButton = transform.GetChild(3).GetComponent<Button>();
        quitButton = transform.GetChild(4).GetComponent<Button>();

        newGameButton.onClick.AddListener(NewGame);
        continueButton.onClick.AddListener(ContinueGame);
        authorButton.onClick.AddListener(AboutAuthor);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void NewGame()
    {
        PlayerPrefs.DeleteAll();    //删除数据
        //转换到第一个场景
        SceneController.Instance.TransitonToFirstLevel();
    }

    private void ContinueGame()
    {
        //读取进度，转换场景
        if (SaveManager.Instance.SceneName != null) 
            SceneController.Instance.TransitionToLoadLevel();   //加载之前保存的场景
    }

    private void AboutAuthor()
    {
        //查看作者
    }

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }
}
