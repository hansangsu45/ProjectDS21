using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoSingleton<CastleManager>
{
    private IEnumerator CastleCrop;
    private WaitForSeconds ws = new WaitForSeconds(60);
    private GameManager gameMng;

    //[HideInInspector] public List<StageBtn> clearStages = new List<StageBtn>();

    private void Awake()
    {
        CastleCrop = CastleCo();
    }

    private void Start()
    {
        gameMng = GameManager.Instance;
        StartCoroutine(CastleCrop);
    }

    private IEnumerator CastleCo()
    { 
        while (true)
        {
            yield return ws;

            if (gameMng.savedData.userInfo.currentSilver < gameMng.savedData.userInfo.maxSilver)
            {
                gameMng.savedData.userInfo.currentSilver += gameMng.savedData.userInfo.cropSilver;
                
                if(gameMng.savedData.userInfo.currentSilver > gameMng.savedData.userInfo.maxSilver)
                {
                    gameMng.savedData.userInfo.currentSilver = gameMng.savedData.userInfo.maxSilver;
                }
            }
        }
    }
}
