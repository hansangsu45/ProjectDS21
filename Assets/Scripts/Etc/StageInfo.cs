using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageInfo : MonoBehaviour
{
    public GameObject[] panels;
    public Button[] buttons;
    public Button fightButton;

    public void Open(int index)//, int EnemyHp ,//int EnemyCardMaxValue)
    {
        panels[index].SetActive(true);
        //상대 성 체력 설정
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
        SceneManager.LoadScene("Main");
    }
}
