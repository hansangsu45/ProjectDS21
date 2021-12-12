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
    public GameObject costPanel;
    public GameObject trueYesPanel;
    public Text trueMoney;

    public Action StartAction = null;

    public void Open(int index)//, int EnemyHp ,//int EnemyCardMaxValue)
    {
        panels[index].SetActive(true);
        fightButton.gameObject.SetActive(true);
    }  
    
    public void trueYesOpen()
    {
        trueYesPanel.SetActive(true);
        trueMoney.text = costInput.text;
    }
    public void trueYesClose()
    {
        trueYesPanel.SetActive(false);
    }

    public void Close(int index)
    {
        panels[index].SetActive(false);
    }

    public void Start()
    {
        fightButton.gameObject.SetActive(false);
        costPanel.gameObject.SetActive(false);
        trueYesPanel.SetActive(false);
    }

    public void FightOpen()
    {
        costPanel.gameObject.SetActive(true);
    }

    public void Fight()
    {
        long cost = long.Parse(costInput.text);

        if(cost>GameManager.Instance.savedData.userInfo.silver)
        {
            //가져갈 수 없음
            return;
        }

        GameManager.Instance.MInfoToJson(cost);
        PoolManager.ClearAll();

        GameManager.Instance.Save();
        SceneManager.LoadScene("Main");
    }
}
