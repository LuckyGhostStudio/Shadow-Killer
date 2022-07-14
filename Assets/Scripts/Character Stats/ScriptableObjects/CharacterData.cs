using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Data", menuName = "Character Stats/Basic Data")]    //创建鼠标右键菜单项，点击创建Data文件
public class CharacterData : ScriptableObject
{
    [Header("States Info")]

    public int maxHealth;
    public int currentHealth;
    public int baseDefence;     //基础防御
    public int currentDefence;  //当前防御

    public float runSpeed;
    public float jumpForce;
    public int maxAirJumpNum;        //最大可跳跃次数（n段跳）
    public float rollSpeed;
    public float climbSpeed;

    [Header("Kill Point")]

    public int killPoint;   //被击杀可获得点数

    [Header("Level")]

    public int currentLevel;    //当前等级
    public int maxLevel;        //最大等级
    public int baseExp;         //基础经验值：达到该经验值可升级
    public int currentExp;      //当前经验值
    public float levelBuff;     //每个等级的基础经验值的增加倍率的增量

    public float levelMultiplier    //每个等级数据的增加倍率
    {
        get{ return 1 + (currentLevel - 1) * levelBuff; }
    }

    /// <summary>
    /// 更新经验值
    /// </summary>
    /// <param name="point">获得点数</param>
    public void UpdateExp(int point)
    {
        currentExp += point;

        if(currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// 升级
    /// </summary>
    private void LevelUp()
    {
        //所有想提升的数据方法----------

        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);  //等级+1 在0到最大等级之间
        baseExp += (int)(baseExp * levelMultiplier);    //基础经验值增加
        
        maxHealth = (int)(maxHealth * levelMultiplier); //最大血量增加
        currentHealth = maxHealth;  //回血

        Debug.Log("Level:" + currentLevel + "Health:" + maxHealth);
    }
}
