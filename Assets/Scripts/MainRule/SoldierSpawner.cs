using UnityEngine;
using System.Collections.Generic;

public class SoldierSpawner : MonoBehaviour
{
    public Transform spawnTr; //���� ������ ���� �θ� ����. ���߿� ����� ������ �� �� �̰��� �����̸� ��

    public float[] spawnLocalYArr;  //������� ��ȯ�� ���� ��ȯ�� ��ǥ�� y����
    public float fallY = -8.73f;  //��ַ� �ʰ��� ������ ������ ���� ȭ�� �� Ÿ����ǥ�� y��
    public float xInterval;  //����� ������ X��ǥ�� �������� �� ����
    public float firstX, spawnX;  //���� ���簡 ��ó���� ���� ��ġ X���� ���簡 ȭ��ۿ��� ��ȯ �� ���� ��ġ�� x��

    [SerializeField] private List<Soldier> soldierList = new List<Soldier>();
    private int index = 0;
    private Vector2 playersStartPos;

    public float soldierRotSpeed = 115f;
    public float fallSpeed = 10f;  //�������� �ӵ�

    private void Start()
    {
        GameObject soldier = GameManager.Instance.idToSoldier[RuleManager.Instance.MyCastle.soldier];
        PoolManager.CreatePool<Soldier>(soldier, spawnTr, 25);
        playersStartPos = spawnTr.position;
    }

    public void SpawnMySoldiers(int count) //���縦 count��ŭ ��ȯ
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
        if (!isAnimation)  //������ ������ ������ ������ �� ����
        {
            soldierList.ForEach(x => x.gameObject.SetActive(false));
        }
        else  //��ַ��� �Ѿ ����� �������� �ִϸ��̼� �ְ� ������� ��
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
