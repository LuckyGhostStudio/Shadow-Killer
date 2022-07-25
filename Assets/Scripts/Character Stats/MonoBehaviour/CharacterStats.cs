using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour     //角色数据统计
{
    public CharacterData templateData;      //模板数据

    public CharacterData characterData;     //角色数据

    public AttackData attackData;           //攻击数据

    [HideInInspector]
    public bool isCritical;                //是否暴击

    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
    }

    #region Read from Data
    //获得和设置characterData的字段值
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
    /// 计算实际伤害值
    /// </summary>
    /// <param name="attacker">攻击者</param>
    /// <param name="defender">防守者</param>
    public int TakeDamage(CharacterStats attacker, CharacterStats defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence, 0);    //攻击-防御，防御大于攻击则取0
        defender.CurrentHealth = Mathf.Max(defender.CurrentHealth - damage, 0);   //当前血量-伤害，小于0则取0
        //TODO:Update UI
        //TODO：经验Update

        if(defender.CurrentHealth <= 0)     //防守者死亡
        {
            attacker.characterData.UpdateExp(defender.characterData.killPoint); //攻击者获得防守者经验点数 增加经验
        }

        return damage;
    }

    /// <summary>
    /// 计算当前的伤害值
    /// </summary>
    /// <returns></returns>
    public int CurrentDamage()
    {
        float damage = Random.Range(attackData.minDamage, attackData.maxDamage);    //最小值和最大值之间随机取值

        if (isCritical)
        {
            damage *= attackData.criticalMultiplier;
            Debug.Log("暴击伤害:" + damage);
        }

        return (int)damage;
    }
    #endregion

    #region
    public void EquipWeapon(ItemData weapon)
    {
        //更换武器：更换动画

        //更改攻击属性值
        attackData.ApplyWeaponData(weapon.weaponData);
    }
    #endregion
}
