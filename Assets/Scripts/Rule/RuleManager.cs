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

    public CardRuleData ruleData;
    private float zPos = 0f;
    private bool isGameStart;
    public Transform[] trashTrs;

    private WaitForSeconds ws1 = new WaitForSeconds(0.8f);
    private WaitForSeconds ws2 = new WaitForSeconds(0.3f);

    private void Awake()
    {
        allCardList = new List<CardScript>(transform.GetComponentsInChildren<CardScript>());
    }

    private void Start()
    {
        allCardList.ForEach(x=>
        {
            x.SetSprite();
            x.transform.localPosition = new Vector3(Random.Range(ruleData.mixX[0],ruleData.mixX[1]),ruleData.mixY, 0);
        });

        isReady = true;

        ResetGame();
    }

    private void MixDeck()
    {
        
    }

    private void DrawCard()
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

        Sequence seq = DOTween.Sequence();

        for(int i=0; i<allCardList.Count; i++)
        {
            seq.Append(allCardList[i].transform.DOLocalMove(allCardList[i].orgPrs.position, 0.06f));
        }
        seq.Play();

        Shuffle();

        yield return new WaitForSeconds(4.5f);

        for (int i = 0; i < trashTrs.Length; i++)
        {
            trashCardList.Add(deckCardList[0]);
            deckCardList[0].transform.DOLocalMove(trashTrs[i].localPosition,0.5f);
            deckCardList[0].transform.DOScale(ruleData.trashCardScale,0.5f);
            
            yield return ws1;
            deckCardList[0].RotateCard();
            deckCardList.RemoveAt(0);
            yield return ws2;
        }
    }
}
