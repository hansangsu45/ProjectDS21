using UnityEngine;
using System.Collections.Generic;

public class SoldierSpawner : MonoBehaviour
{
    public Transform spawnTr;

    public float[] spawnLocalYArr;
    public float fallY = -8.73f;
    public float xInterval;
    public float firstX, spawnX;

    [SerializeField] private List<Soldier> soldierList = new List<Soldier>();
    private int index = 0;
    private Vector2 playersStartPos;

    public float soldierRotSpeed = 115f;
    public float fallSpeed = 10f;

    private void Start()
    {
        GameObject soldier = GameManager.Instance.idToSoldier[RuleManager.Instance.MyCastle.soldier];
        PoolManager.CreatePool<Soldier>(soldier, spawnTr, 25);
        playersStartPos = spawnTr.position;
    }

    public void SpawnMySoldiers(int count)
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
        if (!isAnimation)
        {
            soldierList.ForEach(x => x.gameObject.SetActive(false));
        }
        else
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
