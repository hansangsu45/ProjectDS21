using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SaveData 
{
    public UserInfo userInfo = new UserInfo();
    public Option option = new Option();
}

[Serializable]
public class UserInfo
{
    public bool isFirstStart = true;  //�� ������ ó�� �����ߴ���

    public short leadership = 21; //��ַ� (�� ���� ������ ������ų �� �ִ� �ִ� ���� ��)
    public short clearId; //Ŭ������ �ܰ��� ���� ���� �ܰ�

    public int hp = 20;  //���� �ڽ� ���� ü�� 
    public int maxHp=20;  //�ִ� ü�� 

    public long gold = 0;  //��ȭ 
   
    public long cropSilver;  //'��'�� '���̴�' ��ȭ 
    public long currentSilver;  //���� '�׿��ִ�' ��ȭ�� �� 
    public long maxSilver;  //�ִ�� '���� �� �ִ�' ��ȭ �� 
    public long silver; //���� '������' �ִ� ��ȭ�� ��

    public string quitDate;  //���� ���� ��¥

    public List<StageCastle> stageCastles = new List<StageCastle>(); //ó�� ������ �� ���� ����


    public StageCastle GetStage(short id) => stageCastles.Find(x => x.id == id);

    public StageCastle CreateCastleInfo(StageCastle sc)
    {
        stageCastles.Add(new StageCastle(sc));
        return sc;
    }
}

[Serializable]
public class Option
{
    //0f~1f
    public float bgmSize = 0.5f;
    public float soundEffectSize = 0.7f;
}

[Serializable]
public class StageCastle
{
    public short id;  //�ܰ�

    public bool isOpen;
    public bool isClear = false;

    public StageCastle() { }
    public StageCastle(StageCastle sc)
    {
        id = sc.id;
        isOpen = sc.isOpen;
        isClear = sc.isClear;
    }

    #region �ּ�
    /*public short level = 1;  //��ȭ ��ġ
    public short maxLevel;

    //needTimeForCrop�ʿ� �� ���� crop��ŭ gold����. maxCrop�� �����ϸ� ��Ȯ�ϱ� ������ �� �� ����
    public float needTimeForCrop;  //����: �� 
    public long crop; //�� �� ������ ������ ���̴� ��
    public long currentCrop;
    public long maxCrop;

    public string quitDate;

    public StageCastle(short id, bool open, short maxLv, float needTime, long crop, long maxCrop)
    {
        this.id = id;
        this.isOpen = open;
        this.maxLevel = maxLv;
        this.needTimeForCrop = needTime;
        this.crop = crop;
        this.maxCrop = maxCrop;
    }

    public StageCastle(StageCastle sc)
    {
        this.id = sc.id;
        this.isOpen = sc.isOpen;
        this.maxLevel = sc.maxLevel;
        this.needTimeForCrop = sc.needTimeForCrop;
        this.crop = sc.crop;
        this.maxCrop = sc.maxCrop; 
    }*/
    #endregion
}

[Serializable]
public class CastleInfo
{
    public int hp = 20; 
    public short leaderShip = 21;  //���� ��ַ� (�� ���� '�ʰ�'�ϸ� ����)
    public short minLeaderShip = 16;  //���� �ּ� ī�� ������ �� ���̴�
    public Sprite castleSprite;  //�� ��������Ʈ
    public short soldier;  // �Ϲ� ����
    public short chief;  //1��1 �ϱ��� ���� 

    public long rewardSilver;  //��ȭ ����
    public long rewardGold;  //��ȭ ����
}

[Serializable]
public class MainInfo
{
    public long silver; //�������
    public Sprite castleSprite;  //�� ��������Ʈ
    public short soldier;  // �Ϲ� ����
    public short chief;  //1��1 �ϱ��� ���� 

    public MainInfo(long s, Sprite cSpr, short sold, short ch)
    {
        silver = s;
        castleSprite = cSpr;
        soldier = sold;
        chief = ch;
    }
}

[Serializable]
public class TrashCardUI
{
    public int trashCnt;  //�ش� ���� ���� ����
    public Text trashCountTxt; //�ش� ���� ���� ������ ��Ÿ���� �ؽ�Ʈ UI

    public bool[] isTrashShape; // L: 4   //�ش� ����� ī�尡 ����������
    public Image[] shapeImageList;  // L: 4   //Shape���������� �����Ѵ�� �Ȱ��� �������缭 �̹����� ����ִ´�.

    public void UpdateUI(int shape = -1)
    {
        if (shape == -1) //reset
        {
            trashCnt = 0;
            trashCountTxt.text = "0";
            for (int i = 0; i < isTrashShape.Length; i++) isTrashShape[i] = false;
            for (int i = 0; i < shapeImageList.Length; i++) shapeImageList[i].gameObject.SetActive(false);

            return;
        }

        shapeImageList[shape].gameObject.SetActive(true);
        trashCountTxt.text = trashCnt.ToString();
    }
}