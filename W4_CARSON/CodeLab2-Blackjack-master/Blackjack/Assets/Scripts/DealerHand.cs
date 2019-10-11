using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DealerHand : BlackJackHand {

	public Sprite cardBack;

	bool reveal;

	protected override void SetupHand(){
		base.SetupHand();

		HideCard(handBase1);
		HideCard(handBase2);

		reveal = false;
	}
		
	protected override void ShowValue(List<DeckOfCards.Card> currentHand, Transform handBase, Text currentTotal, int currentHandVal){

		if(currentHand.Count > 1){
			if(!reveal){
				currentHandVal = currentHand[1].GetCardValue();

				currentTotal.text = "Dealer: " + currentHandVal + " + ???";
			} else {
				currentHandVal = GetHandValue(currentHand);

				currentTotal.text = "Dealer: " + currentHandVal;

				BlackJackManager manager = GameObject.Find("BlackJackManager").GetComponent<BlackJackManager>();

				if(currentHandVal > 21){
					manager.DealerBusted();
				} else if(!DealStay(currentHandVal))
				{
					DealMeIn(currentHand, handBase, currentTotal, currentHandVal);
				} else {
					BlackJackHand playerHand = GameObject.Find("PlayerHandValue1").GetComponent<BlackJackHand>();

					if(currentHandVal < playerHand.handVal1 || currentHandVal < playerHand.handVal2){
						manager.PlayerWin();
					} else {
						manager.PlayerLose();
					}
				}
			}
		}
	}

	protected override void SelectHandToHit(Button button)
	{
		return;
	}
	
	protected virtual bool DealStay(int handVal){
		return handVal > 17;
	}

	public void RevealCard(){
		reveal = true;

		GameObject cardOneHandOne = handBase1.transform.GetChild(0).gameObject;
		GameObject cardOneHandTwo = handBase2.transform.GetChild(0).gameObject;

		cardOneHandOne.GetComponentsInChildren<Image>()[0].sprite = null;
		cardOneHandOne.GetComponentsInChildren<Image>()[1].enabled = true;
		cardOneHandTwo.GetComponentsInChildren<Image>()[0].sprite = null;
		cardOneHandTwo.GetComponentsInChildren<Image>()[1].enabled = true;

		ShowCard(hand1[0], cardOneHandOne, handBase1, 0);
		ShowCard(hand1[0], cardOneHandTwo, handBase2, 0);

		ShowValue(hand1, handBase1, total1, handVal1);
		ShowValue(hand2, handBase2, total2, handVal2);
	}

	public void HideCard(Transform handBase)
	{
		GameObject cardOne = handBase.GetChild(0).gameObject;
		cardOne.GetComponentInChildren<Text>().text = "";
		cardOne.GetComponentsInChildren<Image>()[0].sprite = cardBack;
		cardOne.GetComponentsInChildren<Image>()[1].enabled = false;
	}
}
