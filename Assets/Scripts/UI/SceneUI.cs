using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : MonoBehaviour
{
    [Header("Player UI")]
    private Image healthBar;            //Ѫ��
    private Image expBar;               //������
    private Text healthText;            //Ѫ��Text
    //private Text expText;               //����Text
    private Text coinNumberText;        //Coin����Text
    private Text coinNumberTextShadow;  //Coin����Text��Shadow
    private Text levelText;             //�ȼ�Text

    [Header("Other UI")]
    private Button pauseButton;         //��ͣ��ť
    private GameObject pausePanel;     //��ͣ�˵�

    private Button mainMenuButton;      //�ص����˵���ť
    private Button restartButton;       //���¿�ʼ��ť�����ϴδ浵��ʼ
    private Button backButton;      //������ť

    private bool isPause;
    
    void Awake()
    {
        //Player��һЩ UI
        healthBar = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        healthText = transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Text>();
        expBar = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Image>();
        //expText = transform.GetChild(1).GetChild(3).GetComponent<Text>();
        coinNumberText = transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<Text>();
        coinNumberTextShadow = transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>();
        levelText = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();

        //��ͣ���UI
        pauseButton = transform.GetChild(1).GetComponent<Button>();
        pausePanel = transform.GetChild(2).gameObject;

        mainMenuButton = transform.GetChild(2).GetChild(1).GetComponent<Button>();
        restartButton = transform.GetChild(2).GetChild(2).GetComponent<Button>();
        backButton = transform.GetChild(2).GetChild(3).GetComponent<Button>();

        //��Ӽ����¼�
        pauseButton.onClick.AddListener(Pause);

        mainMenuButton.onClick.AddListener(GoMainMenu);
        restartButton.onClick.AddListener(Restart);
        backButton.onClick.AddListener(BackGame);
    }

    void Update()
    {
        UpdateHealth();
        UpdateExp();
        UpdateLevel();
        UpdateCoinNumber();
    }

    /// <summary>
    /// ��ͣ��Ϸ
    /// </summary>
    private void Pause()
    {
        //TODO:�����ͣ�����߼�
        isPause = !isPause;
        pausePanel.SetActive(isPause);
    }

    private void GoMainMenu()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
        SceneController.Instance.TransitionToMain();    //�ص�������
    }

    /// <summary>
    /// ���¿�ʼ���ص�����浵״̬
    /// </summary>
    private void Restart()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
        //��ȡ���ȣ�ת������
        if (SaveManager.Instance.SceneName != null)
            SceneController.Instance.TransitionToLoadLevel();   //����֮ǰ����ĳ���
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    private void BackGame()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
    }

    private void UpdateHealth()
    {
        healthBar.fillAmount = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthText.text = GameManager.Instance.playerStats.CurrentHealth.ToString() + "/" + GameManager.Instance.playerStats.MaxHealth.ToString();
    }

    private void UpdateExp()
    {
        expBar.fillAmount = (float)GameManager.Instance.playerStats.characterData.currentExp / GameManager.Instance.playerStats.characterData.baseExp;
        //expText.text = GameManager.Instance.playerStats.characterData.currentExp.ToString() + "/" + GameManager.Instance.playerStats.characterData.baseExp.ToString();
    }

    private void UpdateLevel()
    {
        levelText.text = GameManager.Instance.playerStats.characterData.currentLevel.ToString();
    }

    /// <summary>
    /// ����Coin����Text
    /// </summary>
    private void UpdateCoinNumber()
    {
        coinNumberText.text = GameManager.Instance.playerStats.characterData.coinNumber.ToString();
        coinNumberTextShadow.text = coinNumberText.text;
    }
}
