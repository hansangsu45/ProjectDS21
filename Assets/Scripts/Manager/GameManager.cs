using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;
using DG.Tweening;

public enum SceneType
{
    LOBBY,
    MAIN
}

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SaveData saveData;
    public SaveData savedData { get { return saveData; } }
    private string savedJson, filePath;
    private readonly string saveFileName_1 = "SaveFile01";

    public int screenWidth = 2960, screenHeight = 1440;
    public bool isReady = false;

    public List<StageBtn> stageBtns = new List<StageBtn>();
    public Dictionary<short, StageCastle> idToCastle = new Dictionary<short, StageCastle>();
    [SerializeField] private short maxViewStage=4; //이제 깨야할 스테이지 '포함'해서 그 스테이지부터 몇 단계(개)까지 보여줄지

    public SceneType scType;

    public string GetFilePath(string fileName) => string.Concat(Application.persistentDataPath, "/", fileName);

    private void Awake()
    {
        filePath = GetFilePath(saveFileName_1);
        saveData = new SaveData();
        Load();
        InitData();
        CreatePool();
        isReady = true;
    }

    private void InitData()
    {
        Screen.SetResolution(screenWidth, screenHeight, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;


    }

    private void CreatePool()
    {
        //풀링할 옵젝들 소환
    }

    private void Start()
    {
        
    }

    public void SaveData()
    {
        //saveData.userInfo.stageCastles.ForEach(x => x.quitDate = DateTime.Now.ToString());
        saveData.userInfo.quitDate = DateTime.Now.ToString();
    }

    public void Save()
    {
        SaveData();

        savedJson = JsonUtility.ToJson(saveData);
        byte[] bytes = Encoding.UTF8.GetBytes(savedJson);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(filePath, code);
    }

    public void Load()
    {
        if (File.Exists(filePath))
        {
            string code = File.ReadAllText(filePath);
            byte[] bytes = Convert.FromBase64String(code);
            savedJson = Encoding.UTF8.GetString(bytes);
            saveData = JsonUtility.FromJson<SaveData>(savedJson);
        }

        SetData();
    }

    public void SetData()
    {
        if (scType == SceneType.LOBBY)
        {
            for (int i = 0; i < stageBtns.Count; i++)
            {
                stageBtns[i].stageCastle = saveData.userInfo.GetStage(stageBtns[i].stageCastle.id) ?? saveData.userInfo.CreateCastleInfo(stageBtns[i].stageCastle);
                idToCastle.Add(stageBtns[i].stageCastle.id, stageBtns[i].stageCastle);

                if ((stageBtns[i].stageCastle.isClear && stageBtns[i].stageCastle.id != saveData.userInfo.clearId)
                    || stageBtns[i].stageCastle.id > saveData.userInfo.clearId + maxViewStage)
                {
                    stageBtns[i].gameObject.SetActive(false);
                }
            }

            TimeSpan ts = new TimeSpan();

            try
            {
                ts = DateTime.Now - Convert.ToDateTime(saveData.userInfo.quitDate);
                saveData.userInfo.currentSilver += (long)ts.TotalMinutes * saveData.userInfo.cropSilver;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    private void Update()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        //페이드 아웃

        Save();

        //씬 넘어갈 때의 각종 처리(풀 삭제 등)

        SceneManager.LoadScene(sceneName);
    }

    #region OnApplication
    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            Save();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            Save();
        }
    }
    #endregion
}
