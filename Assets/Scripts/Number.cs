using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UniRx;

public class Number : MonoBehaviour
{
    private int curNumber;
    public int CurNumber
    {
        get { return curNumber; }
        set { curNumber = value; }
    }

    TMP_Text text;

    Tweener scale;
    Tweener pos;
    public void Init(int n)
    {
        curNumber = n;
        text = GetComponent<TMP_Text>();
        text.text = curNumber.ToString();

        scale = transform.DOShakeScale(5, 0.1f, 1, 90, false).SetLoops(-1);
        pos = transform.DOShakePosition(5, 0.12f, 1, 90, false, true).SetLoops(-1);
    }
    public void KillNumber()
    {
        GetComponent<BoxCollider>().enabled = false;
        scale.Kill();
        pos.Kill();
        transform.DOScale(Vector3.zero, 1);
        Observable.Timer(System.TimeSpan.FromSeconds(1)).Subscribe(x =>
        {
            Destroy(gameObject);
        });
    }
}
