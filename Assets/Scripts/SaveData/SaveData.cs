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
    public bool isFirstStart = true;  //이 게임을 처음 시작했는지

    public short leadership = 21; //통솔력 (한 번에 전투에 참여시킬 수 있는 최대 병사 수)

    public int hp;  //현재 자신 성의 체력
    public int maxHp;  //최대 체력

    public long gold = 0;  //금화
   
    public long cropSilver;  //분당 쌓이는 은화
    public long currentSilver;  //현재 가지고 있는 은화의 양
    public long maxSilver;  //최대로 가질 수 있는 은화 양

    public string quitDate;  //게임 종료 날짜

    public List<StageCastle> stageCastles = new List<StageCastle>(); //처음 시작할 때 전부 들어옴


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
    public short id;  //단계

    public bool isOpen;
    public bool isClear = false;

    public StageCastle() { }
    public StageCastle(StageCastle sc)
    {
        id = sc.id;
        isOpen = sc.isOpen;
        isClear = sc.isClear;
    }

    #region 주석
    /*public short level = 1;  //강화 수치
    public short maxLevel;

    //needTimeForCrop초에 한 번씩 crop만큼 gold들어옴. maxCrop에 도달하면 수확하기 전까지 더 안 쌓임
    public float needTimeForCrop;  //단위: 초 
    public long crop; //한 번 적립될 때마다 쌓이는 돈
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