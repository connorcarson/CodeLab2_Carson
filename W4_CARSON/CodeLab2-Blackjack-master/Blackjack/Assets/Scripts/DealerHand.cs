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
		
	protected override void ShowValue(List<DeckOfCards.Card> currentHand, Text currentTotal){

		if(hand1.Count > 1){
			if(!reveal){
				handVals = hand1[1].GetCardValue();

				currentTotal.text = "Dealer: " + handVals + " + ???";
			} else {
				handVals = GetHandValue(currentHand);

				currentTotal.text = "Dealer: " + handVals;

				BlackJackManager manager = GameObject.Find("BlackJackManager").GetComponent<BlackJackManager>();

				if(handVals > 21){
					manager.DealerBusted();
				} else if(!DealStay(handVals)){
					Invoke("DealMeIn", 1);
				} else {
					BlackJackHand playerHand = GameObject.Find("Player Hand Value").GetComponent<BlackJackHand>();

					if(handVals < playerHand.handVals){
						manager.PlayerWin();
					} else {
						manager.PlayerLose();
					}
				}
			}
		}
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

		ShowValue(hand1, total1);
		ShowValue(hand2, total2);
	}

	public void HideCard(GameObject handBase)
	{
		GameObject cardOne = handBase.transform.GetChild(0).gameObject;
		cardOne.GetComponentInChildren<Text>().text = "";
		cardOne.GetComponentsInChildren<Image>()[0].sprite = cardBack;
		cardOne.GetComponentsInChildren<Image>()[1].enabled = false;
	}
}
