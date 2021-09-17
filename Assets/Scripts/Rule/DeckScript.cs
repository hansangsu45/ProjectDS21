using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    // ī�� ��������Ʈ�� �޾ƿ´�.
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    public int currentIndex = 0;

    void Start()
    {
        GetCardValues();
    }

    //���⸦ �����ؾ��մϴ�.
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

    //ī�带 ����.

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


    // ���� ī�嵵 �Ȱ��� ���.
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
