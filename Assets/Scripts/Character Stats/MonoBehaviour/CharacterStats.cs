using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour     //��ɫ����ͳ��
{
    public CharacterData templateData;      //ģ������

    public CharacterData characterData;     //��ɫ����

    public AttackData attackData;           //��������

    [HideInInspector]
    public bool isCritical;                //�Ƿ񱩻�

    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
    }

    #region Read from Data
    //��ú�����characterData���ֶ�ֵ
    public int MaxHealth
    {
        get { return characterData != null ? characterData.maxHealth : 0; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { return characterData != null ? characterData.currentHealth : 0; }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { return characterData != null ? characterData.baseDefence : 0; }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { return characterData != null ? characterData.currentDefence : 0; }
        set { characterData.currentDefence = value; }
    }
    #endregion

    #region Character Combat
    /// <summary>
    /// ����ʵ���˺�ֵ
    /// </summary>
    /// <param name="attacker">������</param>
    /// <param name="defender">������</param>
    public int TakeDamage(CharacterStats attacker, CharacterStats defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence, 0);    //����-�������������ڹ�����ȡ0
        defender.CurrentHealth = Mathf.Max(defender.CurrentHealth - damage, 0);   //��ǰѪ��-�˺���С��0��ȡ0
        //TODO:Update UI
        //TODO������Update

        if(defender.CurrentHealth <= 0)     //����������
        {
            attacker.characterData.UpdateExp(defender.characterData.killPoint); //�����߻�÷����߾������ ���Ӿ���
        }

        return damage;
    }

    /// <summary>
    /// ���㵱ǰ���˺�ֵ
    /// </summary>
    /// <returns></returns>
    public int CurrentDamage()
    {
        float damage = Random.Range(attackData.minDamage, attackData.maxDamage);    //��Сֵ�����ֵ֮�����ȡֵ

        if (isCritical)
        {
            damage *= attackData.criticalMultiplier;
            Debug.Log("�����˺�:" + damage);
        }

        return (int)damage;
    }
    #endregion

    #region
    public void EquipWeapon(ItemData weapon)
    {
        //������������������

        //���Ĺ�������ֵ
        attackData.ApplyWeaponData(weapon.weaponData);
    }
    #endregion
}
