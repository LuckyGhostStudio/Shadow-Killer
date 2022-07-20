using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Button")]
    private Button newGameButton;
    private Button continueButton;
    private Button authorButton;
    private Button quitButton;

    [Header("Quit UI")]
    private GameObject quitPanel;       //Quitȷ�����
    private Button quitCancelButton;
    private Button quitOKButton;

    [Header("NewGame UI")]
    private GameObject newGamePanel;       //NewGameȷ�����
    private Button newGameCancelButton;
    private Button newGameOKButton;

    [Header("Author UI")]
    private GameObject authorPanel;     //Author����
    private Button authorBackButton;

    private bool authorOpen;

    private void Awake()
    {
        //Main Button
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueButton = transform.GetChild(2).GetComponent<Button>();
        authorButton = transform.GetChild(3).GetComponent<Button>();
        quitButton = transform.GetChild(4).GetComponent<Button>();

        //Quit UI
        quitPanel = transform.GetChild(6).gameObject;
        quitCancelButton = transform.GetChild(6).GetChild(1).GetComponent<Button>();
        quitOKButton = transform.GetChild(6).GetChild(2).GetComponent<Button>();

        //NewGame UI
        newGamePanel = transform.GetChild(7).gameObject;
        newGameCancelButton = transform.GetChild(7).GetChild(1).GetComponent<Button>();
        newGameOKButton = transform.GetChild(7).GetChild(2).GetComponent<Button>();

        //Author UI
        authorPanel = transform.GetChild(8).gameObject;
        authorBackButton = authorPanel.transform.GetChild(4).GetComponent<Button>();

        //��Ӽ����¼�

        //Main Button
        //newGameButton.onClick.AddListener(OpenNewGamePanel);
        newGameButton.onClick.AddListener(NewGame);
        continueButton.onClick.AddListener(ContinueGame);
        authorButton.onClick.AddListener(AboutAuthor);
        quitButton.onClick.AddListener(OpenQuitPanel);

        //Quit UI
        quitCancelButton.onClick.AddListener(CloseQuitPanel);
        quitOKButton.onClick.AddListener(QuitGame);

        //NewGame UI
        newGameCancelButton.onClick.AddListener(CloseNewGamePanel);
        newGameOKButton.onClick.AddListener(NewGame);

        authorBackButton.onClick.AddListener(AboutAuthor);
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
        PlayerPrefs.DeleteAll();    //ɾ������
        //ת������һ������
        SceneController.Instance.TransitonToFirstLevel();
    }

    private void ContinueGame()
    {
        //��ȡ���ȣ�ת������
        if (SaveManager.Instance.SceneName != null) 
            SceneController.Instance.TransitionToLoadLevel();   //����֮ǰ����ĳ���
    }

    private void AboutAuthor()
    {
        //�鿴����
        authorOpen = !authorOpen;
        Debug.Log(authorOpen);
        authorPanel.SetActive(authorOpen);
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
        Debug.Log("�˳���Ϸ");
    }
}
