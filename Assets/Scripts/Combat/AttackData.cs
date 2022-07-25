using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Attack/Attack Data")]
public class AttackData : ScriptableObject
{
    public float attackRange;   //������Χ
    public float coolDown;      //��ȴʱ��
    public int maxDamage;       //����˺�
    public int minDamage;       //��С�˺�

    public float criticalMultiplier;      //��������
    public float criticalChance;        //������

    /// <summary>
    /// Ӧ��Weapon����
    /// </summary>
    /// <param name="weapon">weapon�Ĺ�������</param>
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
