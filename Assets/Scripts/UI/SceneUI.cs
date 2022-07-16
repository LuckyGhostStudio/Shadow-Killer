using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : MonoBehaviour
{
    private Image healthBar;            //血条
    private Image expBar;               //经验条
    private Text healthText;            //血量Text
    //private Text expText;               //经验Text
    private Text coinNumberText;        //Coin数量Text
    private Text coinNumberTextShadow;  //Coin数量Text的Shadow
    private Text levelText;             //等级Text

    private Button pauseButton;         //暂停按钮
    private GameObject pausePanel;     //暂停菜单

    private bool isPause;
    
    void Awake()
    {
        healthBar = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        healthText = transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Text>();
        expBar = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Image>();
        //expText = transform.GetChild(1).GetChild(3).GetComponent<Text>();
        coinNumberText = transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<Text>();
        coinNumberTextShadow = transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>();
        levelText = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();

        pauseButton = transform.GetChild(1).GetComponent<Button>();
        pausePanel = transform.GetChild(2).gameObject;

        pauseButton.onClick.AddListener(Pause);
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
        isPause = !isPause;
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
