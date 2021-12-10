using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SaveData 
{
    public UserInfo userInfo = new UserInfo();
    public Option option = new Option();
    public BattleInfo battleInfo = new BattleInfo();
}

[Serializable]
public class UserInfo
{
    public bool isFirstStart = true;  //이 게임을 처음 시작했는지

    public short leadership = 21; //통솔력 (한 번에 전투에 참여시킬 수 있는 최대 병사 수)
    public short clearId = 0; //클리어한 단계중 가장 높은 단계

    public int hp = 20;  //현재 자신 성의 체력 
    public int maxHp=20;  //최대 체력 

    public long gold = 0;  //금화 
   
    public long cropSilver = 100;  //'분'당 '쌓이는' 은화 
    public long currentSilver = 0;  //현재 '쌓여있는' 은화의 양 
    public long maxSilver = 1000;  //최대로 '쌓일 수 있는' 은화 양 
    public long silver = 1000; //현재 '가지고' 있는 은화의 양

    public string quitDate;  //게임 종료 날짜

    //public short soldier;  // 일반 병사   일단 당장은 안씀
    //public short chief;  //1ㄷ1 일기토 병사      일단 당장은 안씀

    //public List<StageCastle> stageCastles = new List<StageCastle>(); //처음 시작할 때 전부 들어옴 (기획대로라면 굳이 이거 없어도 할 수는 있지만 뭐가 추가될지 모르니 일단 함)


    /*public StageCastle GetStage(short id) => stageCastles.Find(x => x.id == id);

    public StageCastle CreateCastleInfo(StageCastle sc)
    {
        stageCastles.Add(new StageCastle(sc));
        return sc;
    }*/
}

[Serializable]
public class Option
{
    //0f~1f
    public float bgmSize = 0.5f;
    public float soundEffectSize = 0.7f;
}

[Serializable]
public class BattleInfo
{
    public CastleInfo enemyCastle = new CastleInfo();
    public MainInfo myCastle = new MainInfo();
}

#region 주석1
/*[Serializable]
public class StageCastle  //적의 성들 (스테이지)  이것도 당연히 기획대로라면 없어도 할 수 있지만 뭐가 추가될지 모르니 일단 함
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
    *//*public short level = 1;  //강화 수치
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
    }*//*
    #endregion
}*/
#endregion

[Serializable]
public class CastleInfo  //적 성의 능력치 정보
{
    public short stageID; // n스테이지
    public int hp = 20; 
    public short leaderShip = 21;  //적의 통솔력 (이 값을 '초과'하면 디짐)
    public short minLeaderShip = 16;  //적의 최소 카드 총합은 이 값이다
    public Sprite castleSprite;  //성 스프라이트      일단 당장은 안씀
    public short soldier;  // 일반 병사   일단 당장은 안씀
    public short chief;  //1ㄷ1 일기토 병사      일단 당장은 안씀

    public long rewardSilver;  //은화 보상
    public long rewardGold;  //금화 보상
}

[Serializable]
public class MainInfo  //내 전투 정보
{
    public long silver; //전투비용
    public Sprite castleSprite;  //성 스프라이트
    public short soldier;  // 일반 병사
    public short chief;  //1ㄷ1 일기토 병사 

    public MainInfo() { }
    public MainInfo(long silver)
    {
        this.silver = silver;
    }
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
    public GameObject panel;

    public int trashCnt;  //해당 숫자 버린 개수
    public Text trashCountTxt; //해당 숫자 버린 개수를 나타내는 텍스트 UI

    //public bool[] isTrashShape; // L: 4   //해당 모양의 카드가 버려졌는지
    public Image[] shapeImageList = new Image[4];  //  //Shape열거형에서 나열한대로 똑같이 순서맞춰서 이미지를 집어넣는다.

    public void UpdateUI(int shape = -1)
    {
        if (shape == -1) //reset
        {
            trashCnt = 0;
            trashCountTxt.text = "0";
            //for (int i = 0; i < isTrashShape.Length; i++) isTrashShape[i] = false;
            for (int i = 0; i < shapeImageList.Length; i++) shapeImageList[i].gameObject.SetActive(false);

            return;
        }

        if (!panel.activeSelf) panel.SetActive(true);
        shapeImageList[shape].gameObject.SetActive(true);
        trashCountTxt.text = trashCnt.ToString();
    }
}