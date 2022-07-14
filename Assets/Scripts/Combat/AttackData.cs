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
}
