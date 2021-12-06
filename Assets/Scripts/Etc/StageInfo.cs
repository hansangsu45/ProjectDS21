using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StageInfo : MonoSingleton<StageInfo>
{
    public GameObject[] panels;
    public Button[] buttons;
    public Button fightButton;
    public InputField costInput;

    public Action StartAction = null;

    public void Open(int index)//, int EnemyHp ,//int EnemyCardMaxValue)
    {
        panels[index].SetActive(true);
        fightButton.gameObject.SetActive(true);
    }   

    public void Close(int index)
    {
        panels[index].SetActive(false);
    }

    public void Start()
    {
        fightButton.gameObject.SetActive(false);
    }



    public void Fight()
    {
        long cost = long.Parse(costInput.text);

        if(cost>GameManager.Instance.savedData.userInfo.silver)
        {
            //가져갈 수 없음
            return;
        }

        PoolManager.ClearAll();

        GameManager.Instance.MInfoToJson(cost, null, 0, 0);

        StartAction?.Invoke();
        SceneManager.LoadScene("Main");
    }
}
