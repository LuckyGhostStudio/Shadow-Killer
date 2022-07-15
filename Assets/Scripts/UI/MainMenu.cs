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

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("�˳���Ϸ");
    }
}
