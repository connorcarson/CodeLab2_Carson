﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class BlackJackHand : MonoBehaviour {

	public Text total;
	public float xOffset;
	public float yOffset;
	public GameObject viewingBase;
	public GameObject handBase1;
	public GameObject handBase2;
	public int handVals;
	public Button hitMe;
	
	protected DeckOfCards deck;
	protected List<DeckOfCards.Card> hand1;
	protected List<DeckOfCards.Card> hand2;
	bool stay = false;

	// Use this for initialization
	void Start () {
		SetupHand();
	}

	protected virtual void SetupHand(){
		deck = GameObject.Find("Deck").GetComponent<DeckOfCards>();
		hand1 = new List<DeckOfCards.Card>();
		hand2 = new List<DeckOfCards.Card>();
		DealMeIn(hand1, handBase1);
		DealMeIn(hand1, handBase1);
		DealMeIn(hand2, handBase2);
		DealMeIn(hand2, handBase2);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void DealMeIn(List<DeckOfCards.Card> currentHand, GameObject handBase){
		if(!stay){
			DeckOfCards.Card card = deck.DrawCard();

			GameObject cardObj = Instantiate(Resources.Load("prefab/Card")) as GameObject;

			ShowCard(card, cardObj, handBase, currentHand.Count);

			currentHand.Add(card);

			ShowValue();
		}
	}

	public void HitMe()
	{
		if(!stay){
			DeckOfCards.Card card = deck.DrawCard();

			GameObject cardObj = Instantiate(Resources.Load("prefab/Card")) as GameObject;

			ShowCard(card, cardObj, viewingBase, 1);
		}
	}

	protected void ShowCard(DeckOfCards.Card card, GameObject cardObj, GameObject handBase, int pos){
		cardObj.name = card.ToString();

		cardObj.transform.SetParent(handBase.transform);
		cardObj.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
		cardObj.GetComponent<RectTransform>().anchoredPosition = 
			new Vector2(
				xOffset + pos * 110, 
				yOffset);

		cardObj.GetComponentInChildren<Text>().text = deck.GetNumberString(card);
		cardObj.GetComponentsInChildren<Image>()[1].sprite = deck.GetSuitSprite(card);
	}

	protected virtual void ShowValue(){
		handVals = GetHandValue();
			
		total.text = "Player: " + handVals;

		if(handVals > 21){
			GameObject.Find("BlackJackManager").GetComponent<BlackJackManager>().PlayerBusted();
		}
	}

	public int GetHandValue(){
		BlackJackManager manager = GameObject.Find("BlackJackManager").GetComponent<BlackJackManager>();

		return manager.GetHandValue(hand1);
	}
}