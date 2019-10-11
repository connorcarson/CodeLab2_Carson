using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class BlackJackHand : MonoBehaviour {

	public Text total1;
	public Text total2;
	public float xOffset;
	public float yOffset;
	public GameObject viewingBase;
	public GameObject handBase1;
	public GameObject handBase2;
	public GameObject newCardObject;
	public DeckOfCards.Card newCard;
	public int handVals;
	
	private bool placingNewCard;
	private List<GameObject> cards = new List<GameObject>();
	private List<Button> handButtons = new List<Button>();
	
	protected DeckOfCards deck;
	protected List<DeckOfCards.Card> hand1;
	protected List<DeckOfCards.Card> hand2;
	bool stay = false;

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
		DealMeIn(hand1, handBase1, total1);
		DealMeIn(hand1, handBase1, total1);
		DealMeIn(hand2, handBase2, total2);
		DealMeIn(hand2, handBase2, total2);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void DealMeIn(List<DeckOfCards.Card> currentHand, GameObject handBase, Text total){
		if(!stay){
			DeckOfCards.Card card = deck.DrawCard();

			GameObject cardObj = Instantiate(Resources.Load("prefab/Card")) as GameObject;
			cards.Add(cardObj);

			ShowCard(card, cardObj, handBase, currentHand.Count);

			currentHand.Add(card);

			ShowValue(currentHand, total);
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

	void SelectHandToHit(Button button)
	{
		ActivateButtons(false);
		switch (button.GetComponentsInParent<Transform>()[1].name)
		{
			case "PlayerFirstHand":
				newCardObject.transform.SetParent(handBase1.transform);
				newCardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset + hand1.Count * 110, yOffset);
				hand1.Add(newCard);
				ShowValue(hand1, total1);
				break;
			case "PlayerSecondHand":
				newCardObject.transform.SetParent(handBase2.transform);
				newCardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset + hand2.Count * 110, yOffset);
				hand2.Add(newCard);
				ShowValue(hand2, total2);
				break;
			default:
				Debug.Log("You did not select a viable hand.");
				break;
		}
		newCardObject.GetComponent<Button>().onClick.AddListener(delegate { SelectHandToHit(button); });
		handButtons.Add(newCardObject.GetComponent<Button>());
		cards.Add(newCardObject);
	}

	void ActivateButtons(bool placingNewCard)
	{
		foreach (var button in handButtons)
		{
			if (placingNewCard) button.interactable = true;
			else button.interactable = false;
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

	protected virtual void ShowValue(List<DeckOfCards.Card> currentHand, Text currentTotal){
		handVals = GetHandValue(currentHand);
			
		currentTotal.text = "Total: " + handVals;

		if(handVals > 21){
			GameObject.Find("BlackJackManager").GetComponent<BlackJackManager>().PlayerBusted();
		}
	}

	public int GetHandValue(List<DeckOfCards.Card> currentHand){
		BlackJackManager manager = GameObject.Find("BlackJackManager").GetComponent<BlackJackManager>();

		return manager.GetHandValue(currentHand);
	}
}
