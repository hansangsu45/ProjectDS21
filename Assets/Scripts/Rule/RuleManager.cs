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
        // 베팅기능 이용시.
        //betBtn.onClick.AddListener(() => BetClicked());


    }

    //시작을 클릭했을 경우이다. 딜러2장, 플레이어에게 1장을 나눠준 후에 계산된 Value값을 텍스트로 표시해줌.
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

        // 배팅 하는것 딱히 필요없다.
        //playerScript.AdjustMoney(-20);
        //cashText.text = "$" + playerScript.GetMoney().ToString();
    }
    // 플레이어가 HIT를 한다. 카드를 한장 가져옴.
    private void HitClicked()
    {
        //if(playerScript.GetCard() <= 10)
        //{
            playerScript.GetCard();
            if (playerScript.handValue > 20) RoundOver();
            scoreText.text = "CARD : " + playerScript.handValue.ToString();
        //}
    }
    // 라운드를 끝낸다. 이때, 상대의 패를 공개하고, 확정키를 누르면 대결 시작
    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standBtnText.text = "확정";
    }
    // 딜러의 카드가 16을 넘지 않을 경우에 16 이상의 수가 될때 까지 한장을 더뽑게 된다.
    private void HitDealer()
    {
        while(dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "DEALER CARD " + dealerScript.handValue.ToString();
            // if (playerScript.handValue > 20) RoundOver();
        }
    }
    // 플레이어나, 딜러가 버스트일때 라운드 오버가됩니다.
    void RoundOver()
    {
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;

        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        // 플레이어랑 딜러가 둘다 버스트면 무승부인데 고칠거임.
        if(playerBust && dealerBust)
        {
            mainText.text = "모두 버스트입니다. 재배팅하세요.";
            roundOver = true;
        }
        else if(playerBust && playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "딜러가 승리했습니다.";
        }
        else if(dealerBust && playerScript.handValue < dealerScript.handValue)
        {
            mainText.text = "당신이 승리했습니다.";
        }
        else if(playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "값이 동일합니다 무승부";
            playerScript.ResetHand();
            dealerScript.ResetHand();
            roundOver = true;
        }
        else if (!playerBust && !dealerBust && playerScript.handValue < dealerScript.handValue)
        {
            mainText.text = "딜러가 승리했습니다.";
        }
        else if (!playerBust && !dealerBust && playerScript.handValue > playerScript.handValue)
        {
            mainText.text = "플레이어가 승리했습니다.";
        }
        else
        {
            roundOver = false;
        }

        //게임이 끝날경우 초기화. 시작버튼누르면 다시시작.
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

    // 이거는 베팅버튼인데 필요없습니다.
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
