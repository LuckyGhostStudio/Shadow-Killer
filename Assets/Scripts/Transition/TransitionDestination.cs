using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 目的点标签
/// </summary>
public enum DestinationTag
{
    ENTER,  //场景入口
    A,
    B,
    C
}

public class TransitionDestination : MonoBehaviour
{
    public DestinationTag destinationTag;
}
