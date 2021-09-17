using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    // 카드 스프라이트를 받아온다.
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    public int currentIndex = 0;

    void Start()
    {
        GetCardValues();
    }

    //여기를 수정해야합니다.
   void GetCardValues()
    {
        int num = 0;
        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            num %= 13;
            if (num == 13) continue;
           // if (num == 11 && num == 12 && num == 0)
           // {
           //     num = 10;
           // }
            
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
    }

    //카드를 섞음.

    public void Shuffle()
    {
        for(int i = cardSprites.Length - 1; i >0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length - 1) + 1;
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;
        }
    }


    // 딜러 카드도 똑같이 배분.
    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex++]);
        currentIndex++;
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
