using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    public int index = 0;
    public GameObject[] panels;
    public Button[] buttons;

    public void Open()
    {
        panels[index].SetActive(true);
    }

    public void Close()
    {
        panels[index].SetActive(false);
    }

    private void Update()
    {
        buttons[0].onClick.AddListener(delegate
        {
            index = 0;
            Open();
        });
        buttons[1].onClick.AddListener(delegate
        {
            index = 1;
            Open();
        });
        buttons[2].onClick.AddListener(delegate
        {
            index = 2;
            Open();
        });
        buttons[3].onClick.AddListener(delegate
        {
            index = 3;
            Open();
        });
        buttons[4].onClick.AddListener(delegate
        {
            index = 4;
            Open();
        });
        buttons[5].onClick.AddListener(delegate
        {
            index = 5;
            Open();
        });
        buttons[6].onClick.AddListener(delegate
        {
            index = 6;
            Open();
        });
        buttons[7].onClick.AddListener(delegate
        {
            index = 7;
            Open();
        });
        buttons[8].onClick.AddListener(delegate
        {
            index = 8;
            Open();
        });
        buttons[9].onClick.AddListener(delegate
        {
            index = 9;
            Open();
        });
        buttons[10].onClick.AddListener(delegate
        {
            index = 10;
            Open();
        });
        buttons[11].onClick.AddListener(delegate
        {
            index = 11;
            Open();
        });
        buttons[12].onClick.AddListener(delegate
        {
            index = 12;
            Open();
        });


    }
}
