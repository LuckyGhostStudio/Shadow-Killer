using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ŀ�ĵ��ǩ
/// </summary>
public enum DestinationTag
{
    ENTER,  //�������
    A,
    B,
    C
}

public class TransitionDestination : MonoBehaviour
{
    public DestinationTag destinationTag;
}
