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

    public void Open(int index)
    {
        panels[index].SetActive(true);
    }

    public void Close(int index)
    {
        panels[index].SetActive(false);
    }

    public void Start()
    {
        fightButton.gameObject.SetActive(false);
        buttons[0].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);

        });
        buttons[1].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
        buttons[2].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
        buttons[3].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
        buttons[4].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
        buttons[5].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
        buttons[6].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
        buttons[7].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
        buttons[8].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
        buttons[9].onClick.AddListener(delegate
        {
            fightButton.gameObject.SetActive(true);
        });
    }

    public void Fight()
    {
        SceneManager.LoadScene("Main");
    }
}
