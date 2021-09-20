using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SoldierSpawner : MonoBehaviour
{
    public Transform[] castleTr;
    public Transform[] spawnTr; //병사 옵젝들 담을 부모 옵젝. 나중에 병사들 움직일 때 걍 이것을 움직이면 됨
    public Transform playersTarget; //병사들 전체를 움직일 때의 타겟 위치
    public GameObject dust;
    public Transform[] removeTrs; //10개 정도
    private Vector3 dustScale;

    public float[] spawnLocalYArr;  //병사들이 소환될 때의 소환될 좌표의 y값들
    public float fallY = -8.73f;  //통솔력 초과로 밑으로 떨어질 때의 화면 밖 타겟좌표의 y값
    public float xInterval;  //병사들 사이의 X좌표를 기준으로 한 간격
    public float[] firstX, spawnX;  //각각 병사가 맨처음에 있을 위치 X값과 병사가 화면밖에서 소환 될 때의 위치의 x값

    [SerializeField] private List<Soldier> soldierList = new List<Soldier>(), enemySoldierList = new List<Soldier>();
    private int index = 0;
    private Vector2[] playersStartPos = new Vector2[2];
    private bool bFighting=false;
    private bool isWin;
    private bool isDraw;
    private int runBool;
    private int atkTrigger;

    public float soldierRotSpeed = 115f;
    public float fallSpeed = 10f;  //떨어지는 속도

    private void Start()
    {
        GameObject soldier = GameManager.Instance.idToSoldier[RuleManager.Instance.MyCastle.soldier];
        PoolManager.CreatePool<Soldier>(soldier, transform, 25);
        playersStartPos[0] = spawnTr[0].position;
        playersStartPos[1] = spawnTr[1].position;
        runBool = Animator.StringToHash("run");
        atkTrigger = Animator.StringToHash("attack");
        dustScale = dust.transform.localScale;
        dust.transform.localScale = Vector3.zero;
    }

    public void SpawnMySoldiers(int count) //병사를 count만큼 소환
    {
        for(int i=0; i<count; i++)
        {
            Soldier soldier = PoolManager.GetItem<Soldier>();
            soldierList.Add(soldier);
            soldier.transform.parent = spawnTr[0];
            float x = ( (soldierList.Count-1) / spawnLocalYArr.Length) * xInterval;
            soldier.InitSet(-1, new Vector2(spawnX[0], spawnLocalYArr[index]), new Vector2(firstX[0]-x,spawnLocalYArr[index]) );
            index = ++index % spawnLocalYArr.Length;
        }
    }

    public void ResetData(bool isAnimation)
    {
        index = 0;
        spawnTr[0].position = playersStartPos[0];
        spawnTr[1].position = playersStartPos[1];
        bFighting = false;
        if (!isAnimation)  //전투가 끝나고 다음판 시작할 때 해줌
        {
            soldierList.ForEach(x => x.gameObject.SetActive(false));
            enemySoldierList.ForEach(x => x.gameObject.SetActive(false));
            enemySoldierList.Clear();
        }
        else  //통솔력을 넘어서 병사들 떨어지는 애니메이션 주고 사라지게 함
        {
            soldierList.ForEach(x =>
            {
                x.rotSpeed = soldierRotSpeed;
                x.Fall(new Vector3(x.transform.localPosition.x, fallY), fallSpeed);
            });
        }
        soldierList.Clear();
    }

    public void BattleStart(int enemyCount)
    {
        bFighting = true;
        RuleManager.Instance.camMove.target = spawnTr[0];
        soldierList.ForEach(x => x.ani.SetBool(runBool, true));

        if (soldierList.Count == enemyCount) isDraw = true;
        else isWin = soldierList.Count > enemyCount ? true : false;

        index = 0;
        for (int i = 0; i < enemyCount; i++)
        {
            Soldier soldier = PoolManager.GetItem<Soldier>();
            enemySoldierList.Add(soldier);
            soldier.transform.parent = spawnTr[1];
            float x = ((enemySoldierList.Count - 1) / spawnLocalYArr.Length) * xInterval;
            soldier.InitSet(1, new Vector2(firstX[1] + x, spawnLocalYArr[index]), Vector2.zero, false);
            soldier.ani.SetBool(runBool, true);
            index = ++index % spawnLocalYArr.Length;
        }

        Sequence seq = DOTween.Sequence();
        if (enemyCount > 0 && soldierList.Count > 0)
        {
            seq.Append(spawnTr[0].DOMove(playersTarget.position, 2.7f));
            spawnTr[1].DOMove(playersTarget.position, 1.4f);
            seq.InsertCallback(2.8f, () => { dust.SetActive(true); dust.transform.DOScale(dustScale, 0.3f); });
            seq.AppendCallback(() => StartCoroutine(BattleCo())).Play();
        }
        else if(enemyCount==0 && soldierList.Count > 0)
        {
            StartCoroutine(BattleCo());
        }
        else if(enemyCount>0 && soldierList.Count==0)
        {
            StartCoroutine(BattleCo());
        }
        else if(enemyCount==0 && soldierList.Count==0)
        {
            spawnTr[1].position = playersTarget.position;
            spawnTr[0].DOMove(playersTarget.position, 1.8f);
        }
    }

    private IEnumerator BattleCo()
    {
        while(enemySoldierList.Count>0 && soldierList.Count>0)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 0.7f));

            enemySoldierList[0].Fall(GetRemoveTrm(false).position, fallSpeed);
            enemySoldierList[0].rotSpeed = -soldierRotSpeed;
            soldierList[0].Fall(GetRemoveTrm(true).position, fallSpeed);
            soldierList[0].rotSpeed = soldierRotSpeed;

            enemySoldierList.RemoveAt(0);
            soldierList.RemoveAt(0);
        }

        dust.transform.DOScale(Vector3.zero, 0.25f);
        yield return new WaitForSeconds(0.5f);
        dust.SetActive(false);

        if(isDraw)
        {
            Debug.Log("무승부 일기토");
        }
        else
        {
            Transform winTr = isWin ? spawnTr[0] : spawnTr[1];
            RuleManager.Instance.camMove.target = winTr;
            winTr.DOMove(castleTr[isWin?1:0].position, 2.5f);
            yield return new WaitForSeconds(2.6f);

            List<Soldier> sList = isWin ? soldierList : enemySoldierList;
            sList.ForEach(x => x.ani.SetTrigger(atkTrigger));
            RuleManager.Instance.Damaged(isWin, sList.Count);
            yield return new WaitForSeconds(2.5f);

            ResetData(false);
        }
    }

    /*private int GetMinCount()
    {
        if(enemySoldierList.Count>soldierList.Count)
        {
            return enemySoldierList.Count;
        }
        else if(enemySoldierList.Count<=soldierList.Count)
        {
            return soldierList.Count;
        }
        return 0;
    }*/

    private Transform GetRemoveTrm(bool player) => removeTrs[Random.Range(player?0:5, player?5:removeTrs.Length)];
}
