using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : MonoBehaviour
{
    [Header("Player UI")]
    private Image healthBar;            //血条
    private Image expBar;               //经验条
    private Text healthText;            //血量Text
    //private Text expText;               //经验Text
    private Text coinNumberText;        //Coin数量Text
    private Text coinNumberTextShadow;  //Coin数量Text的Shadow
    private Text levelText;             //等级Text

    [Header("Other UI")]
    private Button pauseButton;         //暂停按钮
    private GameObject pausePanel;     //暂停菜单

    private Button mainMenuButton;      //回到主菜单按钮
    private Button restartButton;       //重新开始按钮：从上次存档开始
    private Button backButton;      //继续按钮

    private bool isPause;
    
    void Awake()
    {
        //Player的一些 UI
        healthBar = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        healthText = transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Text>();
        expBar = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Image>();
        //expText = transform.GetChild(1).GetChild(3).GetComponent<Text>();
        coinNumberText = transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<Text>();
        coinNumberTextShadow = transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>();
        levelText = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();

        //暂停相关UI
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
    }

    void Update()
    {
        UpdateHealth();
        UpdateExp();
        UpdateLevel();
        UpdateCoinNumber();
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
        SceneController.Instance.TransitionToMain();    //回到主场景
    }

    /// <summary>
    /// 重新开始：回到最近存档状态
    /// </summary>
    private void Restart()
    {
        isPause = false;
        pausePanel.SetActive(isPause);
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
