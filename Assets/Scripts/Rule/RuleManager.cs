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

    public CanvasGroup jqkDecidePanel;
    [SerializeField] private Image[] jqkImgs;
    [SerializeField] private Text[] jqkTexts;

    public CardRuleData ruleData;
    private float zPos = 0f;
    private bool isGameStart;
    public Transform[] trashTrs;

    private WaitForSeconds ws1 = new WaitForSeconds(0.8f);
    private WaitForSeconds ws2 = new WaitForSeconds(0.3f);
    
    public PRS orgCardPRS;

    private void Awake()
    {
        Transform t = transform.GetChild(0);
        orgCardPRS = new PRS(t.localPosition, t.localRotation, t.localScale);
        allCardList = new List<CardScript>(transform.GetComponentsInChildren<CardScript>());

        for (int i = 0; i < jqkImgs.Length; i++) jqkImgs[i].sprite = ruleData.backSprite;
    }

    private IEnumerator Start()
    {
        allCardList.ForEach(x=>
        {
            x.SetSprite();
            x.transform.localPosition = new Vector3(Random.Range(ruleData.mixX[0],ruleData.mixX[1]),ruleData.mixY, 0);
        });

        isReady = true;

        yield return DecideJQK();

        ResetGame();
    }

    private IEnumerator DecideJQK()
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.loadingPanel.gameObject.SetActive(false);
        jqkDecidePanel.DOFade(1, 0.4f);

        for(int i=0; i<jqkImgs.Length; i++)
        {
            yield return ws2;
            int ran = Random.Range(40, 61);
            int num = 1;

            for(int j=0; j<ran; j++)
            {
                yield return new WaitForSeconds(0.1f);  //나중에 점점 텍스트 변화하는 속도 줄여나갈거임. 일단은 고정치로
                num = ++num % 10 + 1;
                jqkTexts[i].text = num.ToString();
            }

            Sequence seq = DOTween.Sequence();
            seq.Append( jqkImgs[i].transform.DORotate(new Vector3(0, -90, 0), 0.12f));
            seq.AppendCallback(() =>
            {
                jqkImgs[i].sprite = ruleData.jqkSpr[i];
                jqkImgs[i].transform.DORotate(Vector3.zero, 0.12f);
            });
            allCardList.FindAll(x => (int)x.jqk == i + 1).ForEach(y => y.Value = num);
            yield return ws1;
        }

        jqkDecidePanel.DOFade(0, 0.4f);
    }

    private void DrawCard(bool isPlayer)
    {

    }

    private void ResetGame()
    {
        isGameStart = false;
        StartCoroutine(StartGame());
    }

    private void Shuffle()
    {
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
            seq.Append(allCardList[i].transform.DOLocalMove(orgCardPRS.position, 0.06f));
        }
        seq.Play();

        Shuffle();

        yield return new WaitForSeconds(4.5f);

        for (int i = 0; i < trashTrs.Length; i++)
        {
            trashCardList.Add(deckCardList[0]);
            Transform t = deckCardList[0].transform;
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, -0.01f);
            t.DOLocalMove(trashTrs[i].localPosition,0.5f);
            t.DOScale(ruleData.trashCardScale,0.5f);
            
            yield return ws1;
            deckCardList[0].RotateCard();
            deckCardList.RemoveAt(0);
            yield return ws2;
        }
    }
}
