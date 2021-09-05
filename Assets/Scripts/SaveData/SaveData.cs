using System.Collections.Generic;
using System;

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

    public int hp;  //���� �ڽ� ���� ü��
    public int maxHp;  //�ִ� ü��

    public long gold = 0;  //��ȭ
   
    public long cropSilver;  //�д� ���̴� ��ȭ
    public long currentSilver;  //���� ������ �ִ� ��ȭ�� ��
    public long maxSilver;  //�ִ�� ���� �� �ִ� ��ȭ ��

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