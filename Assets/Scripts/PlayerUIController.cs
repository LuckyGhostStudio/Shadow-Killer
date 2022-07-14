using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    private Image healthBar;            //Ѫ��
    private Image magicBar;             //����
    private Text healthText;            //Ѫ��Text
    private Text coinNumberText;        //Coin����Text
    private Text coinNumberTextShadow;  //Coin����Text��Shadow
    private Text expText;               //����Text
    
    void Awake()
    {
        healthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        healthText = transform.GetChild(0).GetChild(3).GetComponent<Text>();
        magicBar = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        coinNumberText = transform.GetChild(3).GetChild(2).GetComponent<Text>();
        coinNumberTextShadow = transform.GetChild(3).GetChild(1).GetComponent<Text>();
        expText = transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        UpdateHealth();
        UpdateMagic();
        UpdateExp();
        UpdateCoinNumber();
    }

    private void UpdateHealth()
    {
        healthBar.fillAmount = (float)PlayerController.Instance.playerStats.CurrentHealth / PlayerController.Instance.playerStats.MaxHealth;
        healthText.text = PlayerController.Instance.playerStats.CurrentHealth.ToString() + "/" + PlayerController.Instance.playerStats.MaxHealth.ToString();
    }

    private void UpdateMagic()
    {

    }

    private void UpdateExp()
    {

    }

    /// <summary>
    /// ����Coin����Text
    /// </summary>
    private void UpdateCoinNumber()
    {
        coinNumberText.text = PlayerController.Instance.GetCoinNumber().ToString();
        coinNumberTextShadow.text = coinNumberText.text;
    }
}
