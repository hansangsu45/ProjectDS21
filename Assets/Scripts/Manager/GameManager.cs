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
    //public Dictionary<short, StageCastle> idToCastle = new Dictionary<short, StageCastle>();
    [SerializeField] private short maxViewStage=4; //���� ������ �������� '����'�ؼ� �� ������������ �� �ܰ�(��)���� ��������
    //�� ���� ���߿� �����
    //public static string castleInfo;  
    public static string mainInfo;

    public SceneType scType;
    public GameObject touchEffectPrefab, soundPrefab;
    public GameObject[] soldierPrefabs;
    public GameObject[] chiefPrefabs;

    [SerializeField] private float nxSpeed = 1f;

    public Dictionary<short, GameObject> idToSoldier = new Dictionary<short, GameObject>();
    public Dictionary<short, GameObject> idToChief = new Dictionary<short, GameObject>();

    private List<Vector3> startPos = new List<Vector3>();
    private List<Vector3> startRot = new List<Vector3>();

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

        if (scType == SceneType.MAIN)
        {
            short i;

            GameObject o = Instantiate(soldierPrefabs[0]);
            List <Transform> trList = new List<Transform>(o.GetComponentsInChildren<Transform>());
            trList.RemoveAt(0);
            for(i=0; i<trList.Count; i++)
            {
                startPos.Add(trList[i].localPosition);
                startRot.Add(trList[i].localRotation.eulerAngles);
            }
            Destroy(o);

            for(i=0; i<soldierPrefabs.Length; i++)
                idToSoldier.Add(i, soldierPrefabs[i]);
            for (i = 0; i < chiefPrefabs.Length; i++)
                idToChief.Add(i, chiefPrefabs[i]);
        }
    }

    private void CreatePool() //Ǯ ����
    {
        PoolManager.CreatePool<TouchEffect>(touchEffectPrefab, transform, 40);
        PoolManager.CreatePool<SoundPrefab>(soundPrefab, transform, 12);
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
            /*for (int i = 0; i < stageBtns.Count; i++)
            {
                //stageBtns[i].stageCastle = saveData.userInfo.GetStage(stageBtns[i].stageCastle.id) ?? saveData.userInfo.CreateCastleInfo(stageBtns[i].stageCastle);
                //idToCastle.Add(stageBtns[i].stageCastle.id, stageBtns[i].stageCastle);

                if ((stageBtns[i].stageCastle.isClear && stageBtns[i].stageCastle.id != saveData.userInfo.clearId)
                    || stageBtns[i].stageCastle.id > saveData.userInfo.clearId + maxViewStage)
                {
                    stageBtns[i].gameObject.SetActive(false);
                }
            }*/

            for(int i=0; i<stageBtns.Count; i++)
            {
                if(stageBtns[i].stageID<saveData.userInfo.clearId)
                {

                }
            }

            TimeSpan ts = new TimeSpan();

            ts = DateTime.Now - Convert.ToDateTime(saveData.userInfo.quitDate);
            saveData.userInfo.currentSilver += (long)ts.TotalMinutes * saveData.userInfo.cropSilver;
        }
    }

    private void Update()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        UIManager.Instance.FadeInOut(false);  
        
        Save();

        PoolManager.ClearItem<TouchEffect>();
        PoolManager.ClearItem<SoundPrefab>();
        if (scType == SceneType.MAIN)
        {
            PoolManager.ClearItem<Soldier>();
            PoolManager.ClearItem<Chief>();
        }

        StartCoroutine(ChangeDelay(sceneName));
    }

    IEnumerator ChangeDelay(string _name)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(_name);
    }

    public void GameSpeedUP(float speed=-1f) //ī�� ���� ���
    {
        if (speed<0)
        {
            Time.timeScale = 1;
            return;
        }

        Time.timeScale = speed;
    }

    public void ResetSoldier(Transform[] trList) //�� �� �����ϸ� ����� Transform ���� �̻������� ���� ������ �ʱ�ȭ�� ���������.
    {
        for(int i=1; i<trList.Length; i++)
        {
            trList[i].localPosition = startPos[i - 1];
            trList[i].localRotation = Quaternion.Euler(startRot[i - 1]);
        }
    }

    //���������� �Ѿ�� ���� ���� �� �Լ��� ȣ���ؼ� �� ���� �� �� ������ �����ְ� 'Save'�Լ��� ȣ���ؾ��Ѵ�. (�� �Ѿ�� ���� Ǯ ������ �ؾ���)
    public void CInfoToJson(CastleInfo ci) => saveData.battleInfo.enemyCastle = ci;  
    public void MInfoToJson(long cost) => saveData.battleInfo.myCastle = new MainInfo(cost);
    //�� ���� �����
    //public void CInfoToJson(CastleInfo ci) => castleInfo = JsonUtility.ToJson(ci); 
    public void MInfoToJson(long s, Sprite cSpr, short sold, short ch) => mainInfo = JsonUtility.ToJson(new MainInfo(s,cSpr,sold,ch)); //�� ���� ���� ����
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
