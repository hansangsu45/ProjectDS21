using UnityEngine;
using System.Collections.Generic;

public class SoldierSpawner : MonoBehaviour
{
    public Transform spawnTr; //병사 옵젝들 담을 부모 옵젝. 나중에 병사들 움직일 때 걍 이것을 움직이면 됨

    public float[] spawnLocalYArr;  //병사들이 소환될 때의 소환될 좌표의 y값들
    public float fallY = -8.73f;  //통솔력 초과로 밑으로 떨어질 때의 화면 밖 타겟좌표의 y값
    public float xInterval;  //병사들 사이의 X좌표를 기준으로 한 간격
    public float firstX, spawnX;  //각각 병사가 맨처음에 있을 위치 X값과 병사가 화면밖에서 소환 될 때의 위치의 x값

    [SerializeField] private List<Soldier> soldierList = new List<Soldier>();
    private int index = 0;
    private Vector2 playersStartPos;

    public float soldierRotSpeed = 115f;
    public float fallSpeed = 10f;  //떨어지는 속도

    private void Start()
    {
        GameObject soldier = GameManager.Instance.idToSoldier[RuleManager.Instance.MyCastle.soldier];
        PoolManager.CreatePool<Soldier>(soldier, spawnTr, 25);
        playersStartPos = spawnTr.position;
    }

    public void SpawnMySoldiers(int count) //병사를 count만큼 소환
    {
        for(int i=0; i<count; i++)
        {
            Soldier soldier = PoolManager.GetItem<Soldier>();
            soldierList.Add(soldier);
            float x = ( (soldierList.Count-1) / spawnLocalYArr.Length) * xInterval;
            soldier.InitSet(-1, new Vector2(spawnX, spawnLocalYArr[index]), new Vector2(firstX-x,spawnLocalYArr[index]) );
            index = ++index % spawnLocalYArr.Length;
        }
    }

    public void ResetData(bool isAnimation)
    {
        index = 0;
        spawnTr.position = playersStartPos;
        if (!isAnimation)  //전투가 끝나고 다음판 시작할 때 해줌
        {
            soldierList.ForEach(x => x.gameObject.SetActive(false));
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
}
