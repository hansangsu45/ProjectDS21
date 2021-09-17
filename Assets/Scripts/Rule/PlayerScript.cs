using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{     
    public CardScript cardScript;
    public DeckScript deckScript;

    public int handValue = 0;

    private int money = 1000;

    public GameObject[] hand;
    public int cardIndex = 0;

    List<CardScript> aceList = new List<CardScript>();
    

    // 시작할때 카드 1장 들고 시작
    public void StartHand()
    {
        GetCard();
        //GetCard();
        
    }


    // 카드를 불러온다. 이때, 카드의 값을 계산함
    public int GetCard()
    {
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());

        hand[cardIndex].GetComponent<Renderer>().enabled = true;

        handValue += cardValue;

        if(cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }

        cardIndex++;
        return handValue;
    }

    // 이거는 솔직히 필요없긴함. Ace를 1로 계산할건지 11로 계산할건지 인데 .. 지워도됨
    /*public void AceCheck()
    {
        foreach (CardScript ace in aceList)
        {
            // a 어떻게할지 설정
            if (handValue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                ace.SetValue(11);
                handValue += 10;
            }
            else if (handValue > 21 && ace.GetValueOfCard() == 11)
            {
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }*/


    //요거는 베팅기능에 쓰이는거
    public void AdjustMoney(int amount)
    {
        money += amount;
    }
    // 요것도 지워도 됨.
    public int GetMoney()
    {
        return money;
    }

    //게임이 끝나고 리셋한다.
    public void ResetHand()
    {
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }

        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }

}
