using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum JQK
{
    NONE,
    J,
    Q,
    K
}

public class RuleManager : MonoSingleton<RuleManager>
{
    public bool isReady = false;

    public List<CardScript> allCardList;
    public PlayerScript player;
    public PlayerScript enemy;
    [SerializeField] private List<CardScript> trashCardList = new List<CardScript>();
    [SerializeField] private List<CardScript> deckCardList = new List<CardScript>();
    [SerializeField] private CastleInfo enemyCastle;
    [SerializeField] private MainInfo myCastle;
    public CameraMove camMove;

    private RaycastHit2D hit;

    public GameObject clickPrevObj;
    public CanvasGroup jqkDecidePanel, continuePanel, viewPanel;
    [SerializeField] private Image[] jqkImgs;
    [SerializeField] private Text[] jqkTexts;

    public CardRuleData ruleData;
    private float zPos = 0f;
    private bool isGameStart;  //reset후에 true
    private bool isMovable;  //뭔가 행동 가능?
    private bool isThrowing = false;  //내 카드 버리는 중임?
    private bool isMyTurn;  //내 턴인지
    private bool isCardTouch;  //카드 확대 가능?

    public Transform[] trashTrs;
    private Vector3 rot1 = new Vector3(0, -90, 0);

    private WaitForSeconds ws1 = new WaitForSeconds(0.8f);
    private WaitForSeconds ws2 = new WaitForSeconds(0.3f);
    private WaitForSeconds ws3 = new WaitForSeconds(0.1f);
    private WaitForSeconds ws4 = new WaitForSeconds(0.03f);

    public PRS orgCardPRS;

    [SerializeField] private Text PTotalTxt, ETotalTxt;
    [SerializeField] private Button drawBtn, stopBtn;
    [SerializeField] private Text moneyTxt, continueTxt;
    [SerializeField] private Image cardImg;
    [SerializeField] private Text[] leftUpJQKTexts;

    private void Awake()
    {
        Transform t = transform.GetChild(0);
        orgCardPRS = new PRS(t.localPosition, t.localRotation, t.localScale);
        allCardList = new List<CardScript>(transform.GetComponentsInChildren<CardScript>());

        for (int i = 0; i < jqkImgs.Length; i++) jqkImgs[i].sprite = ruleData.backSprite;
        continueTxt.text = string.Format("계속하기({0}은화 필요)", ruleData.resapwnSilver);
    }

    private void OnEnable()
    {
        //enemyCastle = JsonUtility.FromJson<CastleInfo>(GameManager.castleInfo);
        //myCastle = JsonUtility.FromJson<MainInfo>(GameManager.mainInfo);
        moneyTxt.text = myCastle.silver.ToString();
    }

    private IEnumerator Start()
    {
        allCardList.ForEach(x=>
        {
            x.SetSprite();
            x.transform.localPosition = new Vector3(Random.Range(ruleData.mixX[0],ruleData.mixX[1]),ruleData.mixY, 0);
        });  //모든 카드의 스프라이트를 백 스프라이트로 하고 위치를 위로 올려서 섞는 애니메이션을 준비한다

        isReady = true;

        yield return DecideJQK();

        ResetGame();
    }

    private IEnumerator DecideJQK()  //JQK랜덤으로 정하고 그에 따른 애니메이션 효과
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.loadingPanel.gameObject.SetActive(false);
        jqkDecidePanel.DOFade(1, 0.4f);  //JQK패널 등장

        for(int i=0; i<jqkImgs.Length; i++)
        {
            yield return ws2;
            int ran = Random.Range(20, 41);
            int num = 1;  //이 값이 for문을 돌고나서 랜덤값으로 지정될거임

            for(int j=0; j<ran; j++)
            {
                yield return new WaitForSeconds(0.07f);  //나중에 점점 텍스트 변화하는 속도 줄여나갈거임(시간 되면). 일단은 고정치로
                num = num % 10 + 1;
                jqkTexts[i].text = num.ToString();
            }

            Sequence seq = DOTween.Sequence();
            seq.Append( jqkImgs[i].transform.DORotate(rot1, 0.12f));  //카드 회전 효과
            seq.AppendCallback(() =>
            {
                jqkImgs[i].sprite = ruleData.jqkSpr[i];
                jqkImgs[i].transform.DORotate(Vector3.zero, 0.12f);
            }).Play();  //90도에서 스프라이트 변경하고 다시 0도로 회전
            allCardList.FindAll(x => (int)x.jqk == i + 1).ForEach(y => y.Value = num);  //카드 리스트에서 모든 J(혹은 Q나 K)를 찾고 그 값을 위에서 정한 랜덤값으로
            leftUpJQKTexts[i].text = num.ToString();
            yield return ws1;
        }

        jqkDecidePanel.DOFade(0, 0.4f);  //JQK패널 꺼짐
        leftUpJQKTexts[0].transform.parent.parent.gameObject.SetActive(true);
    }

    public void DrawCard(bool isPlayer)  //드로우
    {
        if ( (isMyTurn && isMovable && deckCardList.Count>0) || (!isMyTurn && !isPlayer) )
        {
            isMovable = false;

            if (isPlayer)
            {
                if(isGameStart && myCastle.silver < ruleData.drawSilver)
                {
                    UIManager.Instance.OnSystemMsg("전투 비용이 부족합니다.");
                    isMovable = true;
                    return;
                }

                SortCardList(player, deckCardList[0]);
                if (isGameStart)
                {
                    myCastle.silver -= ruleData.drawSilver;
                    moneyTxt.text = myCastle.silver.ToString();
                }
            }
            else
            {
                SortCardList(enemy, deckCardList[0]);
            }

            deckCardList.RemoveAt(0);
        }
    }

    private void ResetGame()  //게임 리셋
    {
        clickPrevObj.SetActive(true);
        isGameStart = false;
        isMovable = false;
        isMyTurn = true;
        isCardTouch = true;
        stopBtn.interactable = true;
        StartCoroutine(StartGame());
    }

    private void Shuffle()  //셔플 함수
    {
        player.RemoveAllCard();
        enemy.RemoveAllCard();
        trashCardList.Clear();
        deckCardList.Clear();
        int i;

        for(i=0; i<33; ++i)
        {
            int r1 = Random.Range(0, allCardList.Count);
            int r2 = Random.Range(0, allCardList.Count);

            CardScript temp = allCardList[r1];
            allCardList[r1] = allCardList[r2];
            allCardList[r2] = temp;
        }
        for(i=0; i<allCardList.Count; i++)
        {
            deckCardList.Add(allCardList[i]);
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.5f);
        jqkDecidePanel.gameObject.SetActive(false);

        Sequence seq = DOTween.Sequence();

        for(int i=0; i<allCardList.Count; i++)
        {
            seq.Append(allCardList[i].transform.DOLocalMove(orgCardPRS.position, 0.05f));  //모든 카드가 덱으로
        }
        seq.Play();

        Shuffle();  

        yield return new WaitForSeconds(4);

        for (int i = 0; i < trashTrs.Length; i++)  //카드 6장 버리기
        {
            trashCardList.Add(deckCardList[0]);
            Transform t = deckCardList[0].transform;
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, -0.01f);
            t.DOLocalMove(trashTrs[i].localPosition,0.4f);
            t.DOScale(ruleData.trashCardScale,0.4f);
            
            yield return ws1;
            deckCardList[0].RotateCard();
            deckCardList.RemoveAt(0);
            yield return ws2;
        }

        {  //적 카드 한 장 뒤집은 상태로 가져온다
            Transform t2 = deckCardList[0].transform;
            t2.localPosition = new Vector3(t2.localPosition.x, t2.localPosition.y, -0.01f);
            t2.DOLocalMove(enemy.cardTrs[0].localPosition, 0.4f);
            t2.DOScale(ruleData.cardScale, 0.4f);

            yield return ws1;
            enemy.AddCard(deckCardList[0]);
            deckCardList.RemoveAt(0);
            yield return ws2;
        }

        isMovable = true;
        DrawCard(false);
        while (!isMovable) yield return null;
        DrawCard(true);
        while (!isMovable) yield return null;
        yield return new WaitForSeconds(1);
        clickPrevObj.SetActive(false);

        isGameStart = true;
    }

    private IEnumerator UpdateTotalUI(Text txt ,int target, int j)  //카드 총합 업데이트
    {
        int current = int.Parse(txt.text);
        for(int i=current; i!=target+j; i+=j)
        {
            yield return ws3;
            txt.text = i.ToString();
        }
        isMovable = !isThrowing;
        if(deckCardList.Count==0 && !isThrowing) StartCoroutine(DeckReShuffle());
    }

    private void SortCardList(PlayerScript ps, CardScript cs)  //카드를 추가하고 정렬한다
    {
        Transform t = cs.transform;
        t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, -0.01f);
        Sequence seq = DOTween.Sequence();
        ps.AddCard(cs);

        if (ps.trIdx>=ps.cardTrs.Length)
        {
            zPos = 0f;
            float x1 = ps.cardTrs[0].localPosition.x;
            float x2 = ps.cardTrs[ps.cardTrs.Length - 1].localPosition.x;
            seq.Append(t.DOScale(ruleData.cardScale, 0.4f));
            float y = ps.cardTrs[0].localPosition.y;

            for (int i = 0; i < ps.cardList.Count; i++)
            {
                zPos -= 0.01f;
                float x = Mathf.Lerp(x1, x2, (float)i / (ps.cardList.Count - 1));
                ps.cardList[i].transform.DOLocalMove(new Vector3(x, y, zPos), 0.4f);
            }
        }
        else
        {
            seq.Append(t.DOScale(ruleData.cardScale, 0.4f));

            for(int i=0; i<ps.cardList.Count; i++)
            {
                t.DOLocalMove(ps.cardTrs[i].localPosition, 0.4f);
            }
        }

        seq.AppendCallback(() =>
        {
            cs.RotateCard();
            if (ps.isMine) StartCoroutine(UpdateTotalUI(PTotalTxt, player.total, 1));
            else isMovable = true;
        }).Play();

        if(isMyTurn) CheckLeadership(ps);
    }

    private void CheckLeadership(PlayerScript ps, bool second=false)
    {
        if (ps.isMine && ps.total > GameManager.Instance.savedData.userInfo.leadership)
        {
            isMovable = false;
            StartCoroutine(ThrowCard(ps));
        }
        else if(!isMyTurn && ps.total > enemyCastle.leaderShip && second)
        {
            isMovable = false;
            StartCoroutine(ThrowCard(ps));
        }
        else if(deckCardList.Count==0 && !isMyTurn)
        {
            StartCoroutine(DeckReShuffle());
        }
    }

    private IEnumerator ThrowCard(PlayerScript ps)
    {
        isThrowing = true;
        float x1 = trashTrs[0].localPosition.x;
        float x2 = trashTrs[trashTrs.Length - 1].localPosition.x;
        float y = trashTrs[0].localPosition.y;
        zPos = 0f;
        int count = ps.cardList.Count;
        if (ps.isMine)
        {
            continuePanel.gameObject.SetActive(true);
            continuePanel.DOFade(1, 3);
        }
        isCardTouch = false;

        yield return new WaitForSeconds(2.5f);
        for (int i = count-1; i>=0; i--)  //카드 전부 버리기
        {
            trashCardList.Add(ps.cardList[i]);
            Transform t = ps.cardList[i].transform;

            t.DOScale(ruleData.trashCardScale, 0.3f);
            for(int j=0; j<trashCardList.Count; j++)
            {
                zPos -= 0.01f;
                trashCardList[j].transform.DOLocalMove(new Vector3(Mathf.Lerp(x1, x2, (float)j / (trashCardList.Count - 1)), y, zPos), 0.35f);
            }
            yield return ws1;
        }
        ps.RemoveAllCard();
        isThrowing = false;

        if (ps.isMine) StartCoroutine(UpdateTotalUI(PTotalTxt,0,-1));
        else
        {
            isMovable = true;
            if (deckCardList.Count == 0) StartCoroutine(DeckReShuffle());
        }
    }

    public void Stop()
    {
        if (!isMyTurn) return;

        isMyTurn = false;
        stopBtn.interactable = false;
        isCardTouch = false;

        if (continuePanel.gameObject.activeSelf)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(continuePanel.DOFade(0, 1));
            seq.AppendCallback(() => continuePanel.gameObject.SetActive(false));
            
            isCardTouch = true;
        }

        StartCoroutine(EnemyAI());
    }

    public void ContinueGame()
    {
        if(myCastle.silver<ruleData.resapwnSilver)
        {
            UIManager.Instance.OnSystemMsg("은화가 부족합니다.");
            return;
        }

        myCastle.silver -= ruleData.resapwnSilver;
        moneyTxt.text = myCastle.silver.ToString();

        Sequence seq = DOTween.Sequence();
        
        seq.Append( continuePanel.DOFade(0, 2) );
        seq.AppendCallback(() => continuePanel.gameObject.SetActive(false));
        seq.Play(); //이 줄은 빼도 시퀀스가 실행이 됨 (일단은 그냥 명시적으로 넣음)
        isCardTouch = true;
    }

    private IEnumerator DeckReShuffle()
    {
        isMovable = false;
        yield return ws2;
        int i;

        for(i=0; i<trashCardList.Count; i++)
        {
            deckCardList.Add(trashCardList[i]);
            trashCardList[i].transform.DOLocalMove(new Vector3(Random.Range(ruleData.mixX[0], ruleData.mixX[1]), ruleData.mixY, 0), 0.04f);
            trashCardList[i].RotateCard(false);
            yield return ws4;
        }

        trashCardList.Clear();
        yield return ws1;

        for (i = 0; i < deckCardList.Count; i++)
        {
            deckCardList[i].transform.DOLocalMove(orgCardPRS.position, 0.04f);
            deckCardList[i].transform.DOScale(orgCardPRS.scale, 0.03f);
            yield return ws4;
        }

        for(i = 0; i<25; ++i)
        {
            int r1 = Random.Range(0, deckCardList.Count);
            int r2 = Random.Range(0, deckCardList.Count);

            CardScript temp = deckCardList[r1];
            deckCardList[r1] = deckCardList[r2];
            deckCardList[r2] = temp;
        }

        for (i = 0; i < trashTrs.Length; i++)  //카드 6장 버리기
        {
            trashCardList.Add(deckCardList[0]);
            Transform t = deckCardList[0].transform;
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, -0.01f);
            t.DOLocalMove(trashTrs[i].localPosition, 0.4f);
            t.DOScale(ruleData.trashCardScale, 0.4f);

            yield return ws2;
            deckCardList[0].RotateCard();
            deckCardList.RemoveAt(0);
            yield return ws2;
        }

        isMovable = true;
    }

    private IEnumerator EnemyAI()
    {
        while (isThrowing) yield return null;

        while(enemy.total<enemyCastle.minLeaderShip)
        {
            yield return new WaitForSeconds(1.5f);

            CheckLeadership(enemy);
            yield return ws3;
            while (!isMovable) yield return null;

            DrawCard(false);
            yield return ws3;
            while (!isMovable) yield return null;
        }

        yield return ws1;
        enemy.cardList[0].RotateCard();

        yield return ws2;
        CheckLeadership(enemy, true);
        yield return ws3;
        while (!isMovable) yield return null;

        ETotalTxt.text = enemy.total.ToString();

        yield return new WaitForSeconds(.5f);

        if (cardImg.gameObject.activeSelf)
        {
            UIManager.Instance.ViewUI(0);
        }
        viewPanel.DOFade(0, 1.3f);
        allCardList.ForEach(x=>x.spriteRenderer.DOColor(ruleData.noColor,1.1f));

        yield return new WaitForSeconds(1.3f);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("Card"))
                {
                    Sprite spr = hit.transform.GetComponent<SpriteRenderer>().sprite;

                    if(spr != ruleData.backSprite && isCardTouch)  //앞면이라면
                    {
                        if (!cardImg.gameObject.activeSelf)
                        {
                            cardImg.sprite = spr;
                            UIManager.Instance.ViewUI(0);
                        }
                    }
                }
            }
        }
    }
}
