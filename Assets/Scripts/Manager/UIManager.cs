using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    public CanvasGroup loadingPanel;  //로딩창

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        _Input();
    }

    void _Input()
    {
        if(Input.GetKeyDown(KeyCode.Escape))  //뒤로가기
        {

        }
    }
}
