using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    private WaitForSeconds lWs = new WaitForSeconds(.3f);

    public CanvasGroup loadingPanel;  //�ε�â(�� ���̵� ��/�ƿ��� ���� ���� ȭ��)
    public Ease[] eases;

    private IEnumerator Start()
    {
        while (!GameManager.Instance.isReady) yield return lWs;    //yield return null;

        if (GameManager.Instance.scType == SceneType.MAIN)
        {
            while (!RuleManager.Instance.isReady) yield return lWs;
        }

        FadeInOut(true);
    }

    private void Update()
    {
        _Input();
    }

    public void FadeInOut(bool fadeIn)
    {
        loadingPanel.DOFade(fadeIn ? 0 : 1, 1).SetEase(eases[0]);
    }

    void _Input()
    {
        if(Input.GetKeyDown(KeyCode.Escape))  //�ڷΰ���
        {

        }
    }
}
