using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : Singleton<SceneUI>
{
    [Header("Player UI")]
    private Image healthBar;            //Ѫ��
    private Image expBar;               //������
    private Text healthText;            //Ѫ��Text
    //private Text expText;               //����Text
    private Text coinNumberText;        //Coin����Text
    private Text coinNumberTextShadow;  //Coin����Text��Shadow
    private Text levelText;             //�ȼ�Text

    [Header("Pause UI")]
    private Button pauseButton;         //��ͣ��ť
    private GameObject pausePanel;      //��ͣ�˵�

    private Button mainMenuButton;      //�ص����˵���ť
    private Button restartButton;       //���¿�ʼ��ť�����ϴδ浵��ʼ
    private Button backButton;          //������ť

    [Header("Defeat UI")]
    private GameObject defeatPanel;     //ս�ܽ���
    private Text coinNumberText2;        //Coin����Text
    private Text coinNumberTextShadow2;  //Coin����Text��Shadow
    private Button mainMenuButton2;      //�ص����˵���ť
    private Button restartButton2;       //���¿�ʼ��ť�����ϴδ浵��ʼ

    private GameObject tipsDialogPanel;     //��ʾ�Ի���

    private bool isPause;
    public bool pause { get { return isPause; } }
    
    protected override void Awake()
    {
        base.Awake();

        //Player UI
        healthBar = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        healthText = transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Text>();
        expBar = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Image>();
        //expText = transform.GetChild(1).GetChild(3).GetComponent<Text>();
        coinNumberText = transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<Text>();
        coinNumberTextShadow = transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>();
        levelText = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();

        //Pause UI
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

        //Defeat UI
        defeatPanel = transform.GetChild(4).gameObject;
        mainMenuButton2 = defeatPanel.transform.GetChild(3).GetComponent<Button>();
        restartButton2 = defeatPanel.transform.GetChild(4).GetComponent<Button>();
        coinNumberText2 = defeatPanel.transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<Text>();
        coinNumberTextShadow2 = defeatPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>();

        //��Ӽ����¼�
        mainMenuButton2.onClick.AddListener(GoMainMenu);
        restartButton2.onClick.AddListener(Restart);

        tipsDialogPanel = transform.GetChild(3).gameObject;
    }

    void Update()
    {
        UpdateHealth();
        UpdateExp();
        UpdateLevel();
        UpdateCoinNumber();
    }

    public void SetTipsDialog(bool open)
    {
        tipsDialogPanel.SetActive(open);
    }

    /// <summary>
    /// ս��
    /// </summary>
    public void Defeat()
    {
        coinNumberText2.text = GameManager.Instance.playerStats.characterData.coinNumber.ToString();
        coinNumberTextShadow2.text = coinNumberText.text;

        //yield return new WaitForSeconds(0);

        defeatPanel.SetActive(true);    //ս�ܽ���

        //SaveManager.Instance.SavePlayerData();  //��������
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
        defeatPanel.SetActive(false);

        SceneController.Instance.TransitionToMain();    //�ص�������
    }

    /// <summary>
    /// ���¿�ʼ���ص�����浵״̬
    /// </summary>
    private void Restart()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
        defeatPanel.SetActive(false);

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
