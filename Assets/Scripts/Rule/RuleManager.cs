using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleManager : MonoBehaviour
{
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
   // public Button betBtn;

    private int standClicks = 0;

    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    public Text mainText;
    public Text scoreText;
    public Text dealerScoreText;
    public Text betsText;
    //public Text cashText;
    public Text standBtnText;

    public GameObject hideCard;

    //int pot = 0;


    void Start()
    {
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        // ���ñ�� �̿��.
        //betBtn.onClick.AddListener(() => BetClicked());


    }

    //������ Ŭ������ ����̴�. ����2��, �÷��̾�� 1���� ������ �Ŀ� ���� Value���� �ؽ�Ʈ�� ǥ������.
    private void DealClicked()
    {
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        playerScript.ResetHand();
        dealerScript.ResetHand();
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        scoreText.text = "CARD : " + playerScript.handValue.ToString();
        dealerScoreText.text = "DEALER CARD : " + dealerScript.handValue.ToString();
        hideCard.GetComponent<Renderer>().enabled = true;
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";
        //pot = 40;
        //betsText.text = "Bets: $" + pot.ToString();

        // ���� �ϴ°� ���� �ʿ����.
        //playerScript.AdjustMoney(-20);
        //cashText.text = "$" + playerScript.GetMoney().ToString();
    }
    // �÷��̾ HIT�� �Ѵ�. ī�带 ���� ������.
    private void HitClicked()
    {
        //if(playerScript.GetCard() <= 10)
        //{
            playerScript.GetCard();
            if (playerScript.handValue > 20) RoundOver();
            scoreText.text = "CARD : " + playerScript.handValue.ToString();
        //}
    }
    // ���带 ������. �̶�, ����� �и� �����ϰ�, Ȯ��Ű�� ������ ��� ����
    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standBtnText.text = "Ȯ��";
    }
    // ������ ī�尡 16�� ���� ���� ��쿡 16 �̻��� ���� �ɶ� ���� ������ ���̰� �ȴ�.
    private void HitDealer()
    {
        while(dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "DEALER CARD " + dealerScript.handValue.ToString();
            // if (playerScript.handValue > 20) RoundOver();
        }
    }
    // �÷��̾, ������ ����Ʈ�϶� ���� �������˴ϴ�.
    void RoundOver()
    {
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;

        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        // �÷��̾�� ������ �Ѵ� ����Ʈ�� ���º��ε� ��ĥ����.
        if(playerBust && dealerBust)
        {
            mainText.text = "��� ����Ʈ�Դϴ�. ������ϼ���.";
            roundOver = true;
        }
        else if(playerBust && playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "������ �¸��߽��ϴ�.";
        }
        else if(dealerBust && playerScript.handValue < dealerScript.handValue)
        {
            mainText.text = "����� �¸��߽��ϴ�.";
        }
        else if(playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "���� �����մϴ� ���º�";
            playerScript.ResetHand();
            dealerScript.ResetHand();
            roundOver = true;
        }
        else if (!playerBust && !dealerBust && playerScript.handValue < dealerScript.handValue)
        {
            mainText.text = "������ �¸��߽��ϴ�.";
        }
        else if (!playerBust && !dealerBust && playerScript.handValue > playerScript.handValue)
        {
            mainText.text = "�÷��̾ �¸��߽��ϴ�.";
        }
        else
        {
            roundOver = false;
        }

        //������ ������� �ʱ�ȭ. ���۹�ư������ �ٽý���.
        if (roundOver)
        {
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            //cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
        }
    }

    // �̰Ŵ� ���ù�ư�ε� �ʿ�����ϴ�.
    //void BetClicked()
    //{
    //    Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
    //    int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
    //    playerScript.AdjustMoney(-intBet);
    //    cashText.text = "$" + playerScript.GetMoney().ToString();
    //    pot += (intBet * 2);
    //    betsText.text = "Bets:  $" + pot.ToString();
    //}

}
