using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : Singleton<SceneUI>
{
    [Header("Player UI")]
    private Image healthBar;            //血条
    private Image expBar;               //经验条
    private Text healthText;            //血量Text
    //private Text expText;               //经验Text
    private Text coinNumberText;        //Coin数量Text
    private Text coinNumberTextShadow;  //Coin数量Text的Shadow
    private Text levelText;             //等级Text

    [Header("Pause UI")]
    private Button pauseButton;         //暂停按钮
    private GameObject pausePanel;      //暂停菜单

    private Button mainMenuButton;      //回到主菜单按钮
    private Button restartButton;       //重新开始按钮：从上次存档开始
    private Button backButton;          //继续按钮

    [Header("Defeat UI")]
    private GameObject defeatPanel;     //战败界面
    private Text coinNumberText2;        //Coin数量Text
    private Text coinNumberTextShadow2;  //Coin数量Text的Shadow
    private Button mainMenuButton2;      //回到主菜单按钮
    private Button restartButton2;       //重新开始按钮：从上次存档开始

    private GameObject tipsDialogPanel;     //提示对话框

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

        //添加监听事件
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

        //添加监听事件
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
    /// 战败
    /// </summary>
    public void Defeat()
    {
        coinNumberText2.text = GameManager.Instance.playerStats.characterData.coinNumber.ToString();
        coinNumberTextShadow2.text = coinNumberText.text;

        //yield return new WaitForSeconds(0);

        defeatPanel.SetActive(true);    //战败界面

        //SaveManager.Instance.SavePlayerData();  //保存数据
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    private void Pause()
    {
        //TODO:添加暂停场景逻辑
        isPause = !isPause;
        pausePanel.SetActive(isPause);
    }

    private void GoMainMenu()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
        defeatPanel.SetActive(false);

        SceneController.Instance.TransitionToMain();    //回到主场景
    }

    /// <summary>
    /// 重新开始：回到最近存档状态
    /// </summary>
    private void Restart()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
        defeatPanel.SetActive(false);

        //读取进度，转换场景
        if (SaveManager.Instance.SceneName != null)
            SceneController.Instance.TransitionToLoadLevel();   //加载之前保存的场景
    }

    /// <summary>
    /// 返回游戏
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
    /// 更新Coin数量Text
    /// </summary>
    private void UpdateCoinNumber()
    {
        coinNumberText.text = GameManager.Instance.playerStats.characterData.coinNumber.ToString();
        coinNumberTextShadow.text = coinNumberText.text;
    }
}
