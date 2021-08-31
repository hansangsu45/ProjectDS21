using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoSingleton<CastleManager>
{
    private IEnumerator CastleCrop;
    private WaitForSeconds ws = new WaitForSeconds(1);
    private GameManager gameMng;

    [HideInInspector] public List<StageBtn> clearStages = new List<StageBtn>();

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
        int i;

        while (true)
        {
            for(i=0; i<clearStages.Count; ++i)
            {

            }

            yield return ws;
        }
    }
}
