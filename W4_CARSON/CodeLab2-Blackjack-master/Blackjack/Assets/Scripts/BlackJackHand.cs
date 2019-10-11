using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//NOTE (Connor):
//There's A LOT of stuff breaking. I'm also aware this is probably some of the jankiest code I've ever written
//and that adding a million parameters to every function is a bad plan in the longterm. I think I made my mod
//too ambitious and so I slapped together a lot of bad code in an attempt to make the thing work... Currently
//the game does deal two hands to the player and two to the dealer. It's evaluating a lot of stuff correctly,
//but you'll see that it is also very broken...

public class BlackJackHand : MonoBehaviour {
	
	public float xOffset;
	public float yOffset;

	public GameObject newCardObject;
	public DeckOfCards.Card newCard;
	
	public Transform viewingBase;
	public Transform handBase1, handBase2;
	public List<DeckOfCards.Card> hand1, hand2;
	public Text total1, total2;
	public int handVal1, handVal2;
	public bool hand1Busted, hand2Busted;

	private bool placingNewCard;
	public List<GameObject> cards = new List<GameObject>();
	private readonly List<Button> handButtons = new List<Button>();
	
	protected DeckOfCards deck;
	public bool stay = false;

	// Use this for initialization
	void Start () {
		SetupHand();
		
		foreach (var card in cards)
		{
			Button button = card.GetComponent<Button>();
			button.onClick.AddListener(delegate { SelectHandToHit(button); });
			handButtons.Add(button);
		}
	}

	protected virtual void SetupHand(){
		deck = GameObject.Find("Deck").GetComponent<DeckOfCards>();
		hand1 = new List<DeckOfCards.Card>();
		hand2 = new List<DeckOfCards.Card>();
		DealMeIn(hand1, handBase1, total1, handVal1);
		DealMeIn(hand1, handBase1, total1, handVal1);
		DealMeIn(hand2, handBase2, total2, handVal2);
		DealMeIn(hand2, handBase2, total2, handVal2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DealMeIn(List<DeckOfCards.Card> currentHand, Transform handBase, Text total, int currentHandValue){
		if(!stay){
			DeckOfCards.Card card = deck.DrawCard();

			GameObject cardObj = Instantiate(Resources.Load("prefab/Card")) as GameObject;
			cards.Add(cardObj);

			ShowCard(card, cardObj, handBase, currentHand.Count);

			currentHand.Add(card);

			ShowValue(currentHand, handBase, total, currentHandValue);
		}
	}

	public void HitMe()
	{
		if(!stay){
			newCard = deck.DrawCard();

			newCardObject = Instantiate(Resources.Load("prefab/Card")) as GameObject;

			ShowCard(newCard, newCardObject, viewingBase, 1);

			ActivateButtons(true);
		}
	}

	protected virtual void SelectHandToHit(Button button)
	{
		ActivateButtons(false);
		switch (button.GetComponentsInParent<Transform>()[1].name)
		{
			case "PlayerFirstHand":
				newCardObject.transform.SetParent(handBase1.transform);
				newCardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset + hand1.Count * 55, yOffset);
				hand1.Add(newCard);
				ShowValue(hand1, handBase1, total1, handVal1);
				break;
			case "PlayerSecondHand":
				newCardObject.transform.SetParent(handBase2.transform);
				newCardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset + hand2.Count * 55, yOffset);
				hand2.Add(newCard);
				ShowValue(hand2, handBase2, total2, handVal2);
				break;
			default:
				Debug.Log("You did not select a viable hand.");
				break;
		}
		newCardObject.GetComponent<Button>().onClick.AddListener(delegate { SelectHandToHit(button); });
		handButtons.Add(newCardObject.GetComponent<Button>());
		cards.Add(newCardObject);
		
		BustCheck();
	}

	void ActivateButtons(bool placingNewCard)
	{
		foreach (var button in handButtons)
		{
			if (placingNewCard) button.interactable = true;
			else button.interactable = false;
		}
	}

	void BustCheck()
	{
		if (GetHandValue(hand1) > 21)
		{
			hand1Busted = true;
		}
		if (GetHandValue(hand2) > 21)
		{
			hand2Busted = true;
		}
		
		if (hand1Busted && hand2Busted)
		{
			GameObject.Find("BlackJackManager").GetComponent<BlackJackManager>().PlayerBusted();
		}
	}
	
	protected void ShowCard(DeckOfCards.Card card, GameObject cardObj, Transform handBase, int pos){
		cardObj.name = card.ToString();

		cardObj.transform.SetParent(handBase);
		cardObj.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
		cardObj.GetComponent<RectTransform>().anchoredPosition = 
			new Vector2(
				xOffset + pos * 55, 
				yOffset);

		cardObj.GetComponentInChildren<Text>().text = deck.GetNumberString(card);
		cardObj.GetComponentsInChildren<Image>()[1].sprite = deck.GetSuitSprite(card);
	}

	protected virtual void ShowValue(List<DeckOfCards.Card> currentHand, Transform handBase, Text currentTotal, int currentHandVal){
		currentHandVal = GetHandValue(currentHand);
			
		currentTotal.text = "Total: " + currentHandVal;
	}

	public int GetHandValue(List<DeckOfCards.Card> currentHand){
		BlackJackManager manager = GameObject.Find("BlackJackManager").GetComponent<BlackJackManager>();

		return manager.GetHandValue(currentHand);
	}
}
