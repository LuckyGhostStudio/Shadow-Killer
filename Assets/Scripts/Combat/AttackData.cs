using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Attack/Attack Data")]
public class AttackData : ScriptableObject
{
    public float attackRange;   //攻击范围
    public float coolDown;      //冷却时间
    public int maxDamage;       //最大伤害
    public int minDamage;       //最小伤害

    public float criticalMultiplier;      //暴击倍率
    public float criticalChance;        //暴击率

    /// <summary>
    /// 应用Weapon数据
    /// </summary>
    /// <param name="weapon">weapon的攻击数据</param>
    public void ApplyWeaponData(AttackData weapon)
    {
        attackRange = weapon.attackRange;
        coolDown = weapon.coolDown;
        maxDamage = weapon.maxDamage;
        minDamage = weapon.minDamage;
        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }
}
