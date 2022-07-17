using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Transform background1;
    private Transform background2;

    Transform left;
    Transform right;

    private Vector3 pos1;

    [SerializeField] private float speed;

    private void Awake()
    {
        background1 = transform.GetChild(0);
        background2 = transform.GetChild(1);

        left = background1;
        right = background2;
    }

    void Start()
    {
        pos1 = background1.position;
    }

    void Update()
    {
        if(left.position.x >= 3)    //左边背景向右移动到临界值
        {
            right.position = pos1 + new Vector3(-29.33f + 3, 0, 0);     //将右边的背景移动到左边 与左边的背景贴合

            //交换左右背景
            Transform temp = left;
            left = right;
            right = temp;
        }

        left.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        right.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
    }
}
