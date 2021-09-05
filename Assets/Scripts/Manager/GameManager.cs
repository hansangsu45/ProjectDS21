using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SaveData saveData;
    public SaveData savedData { get { return saveData; } }
    private string savedJson, filePath;
    private readonly string saveFileName_1 = "SaveFile01";

    public List<StageBtn> stageBtns = new List<StageBtn>();
    public Dictionary<short, StageCastle> idToCastle = new Dictionary<short, StageCastle>();

    public string GetFilePath(string fileName) => string.Concat(Application.persistentDataPath, "/", fileName);

    private void Awake()
    {
        filePath = GetFilePath(saveFileName_1);
        saveData = new SaveData();
        Load();
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
        for(int i=0; i<stageBtns.Count; i++)
        {
            stageBtns[i].stageCastle = saveData.userInfo.GetStage(stageBtns[i].stageCastle.id) ?? saveData.userInfo.CreateCastleInfo(stageBtns[i].stageCastle);
            idToCastle.Add(stageBtns[i].stageCastle.id, stageBtns[i].stageCastle);
        }
    }

    private void Update()
    {
        
    }



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
}
