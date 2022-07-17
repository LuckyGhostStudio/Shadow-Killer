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

    private GameObject quitPanel;       //Quit确认面板
    private Button quitCancelButton;
    private Button quitOKButton;

    private GameObject newGamePanel;       //NewGame确认面板
    private Button newGameCancelButton;
    private Button newGameOKButton;

    private void Awake()
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueButton = transform.GetChild(2).GetComponent<Button>();
        authorButton = transform.GetChild(3).GetComponent<Button>();
        quitButton = transform.GetChild(4).GetComponent<Button>();

        quitPanel = transform.GetChild(6).gameObject;
        quitCancelButton = transform.GetChild(6).GetChild(1).GetComponent<Button>();
        quitOKButton = transform.GetChild(6).GetChild(2).GetComponent<Button>();

        newGamePanel = transform.GetChild(7).gameObject;
        newGameCancelButton = transform.GetChild(7).GetChild(1).GetComponent<Button>();
        newGameOKButton = transform.GetChild(7).GetChild(2).GetComponent<Button>();

        //添加监听事件
        //四个主要按钮
        newGameButton.onClick.AddListener(OpenNewGamePanel);
        continueButton.onClick.AddListener(ContinueGame);
        authorButton.onClick.AddListener(AboutAuthor);
        quitButton.onClick.AddListener(OpenQuitPanel);
        //确认 取消按钮
        quitCancelButton.onClick.AddListener(CloseQuitPanel);
        quitOKButton.onClick.AddListener(QuitGame);

        newGameCancelButton.onClick.AddListener(CloseNewGamePanel);
        newGameOKButton.onClick.AddListener(NewGame);
    }

    private void OpenNewGamePanel()
    {
        newGamePanel.SetActive(true);
    }

    private void CloseNewGamePanel()
    {
        newGamePanel.SetActive(false);
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

    private void OpenQuitPanel()
    {
        quitPanel.SetActive(true);
    }

    private void CloseQuitPanel()
    {
        quitPanel.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }
}
