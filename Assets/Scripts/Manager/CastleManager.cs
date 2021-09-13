using System.Collections;
using UnityEngine;

public class CastleManager : MonoSingleton<CastleManager>
{
    private IEnumerator CastleCrop;
    private WaitForSeconds ws = new WaitForSeconds(60);
    private GameManager gameMng;

    private void Awake()
    {
        CastleCrop = CastleCo();
    }

    private void Start()
    {
        gameMng = GameManager.Instance;
        LimitSilver();
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
                LimitSilver();
            }
        }
    }

    private void LimitSilver()
    {
        if (gameMng.savedData.userInfo.currentSilver > gameMng.savedData.userInfo.maxSilver)
        {
            gameMng.savedData.userInfo.currentSilver = gameMng.savedData.userInfo.maxSilver;
        }
    }
}
