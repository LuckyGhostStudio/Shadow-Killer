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

    private GameObject quitPanel;       //Quitȷ�����
    private Button quitCancelButton;
    private Button quitOKButton;

    private GameObject newGamePanel;       //NewGameȷ�����
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

        //��Ӽ����¼�
        //�ĸ���Ҫ��ť
        newGameButton.onClick.AddListener(OpenNewGamePanel);
        continueButton.onClick.AddListener(ContinueGame);
        authorButton.onClick.AddListener(AboutAuthor);
        quitButton.onClick.AddListener(OpenQuitPanel);
        //ȷ�� ȡ����ť
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
