﻿using UnityEngine;
using System;
using System.Collections;

public class MoveTokensScript : MonoBehaviour {

	private GameManagerScript gameManager;
	private MatchManagerScript matchManager;

	public bool move = false;

	public float lerpPercent;
	public float lerpSpeed;

	bool userSwap;

	private GameObject exchangeToken1, exchangeToken2;
	
	Vector2 exchangeGridPos1, exchangeGridPos2;

	//this runs at the start
	private void Start () {
		gameManager = GetComponent<GameManagerScript>();
		matchManager = GetComponent<MatchManagerScript>();
		lerpPercent = 0;
	}

	//this runs every frame
	private void Update () {
		if (!move) return;
		
		lerpPercent += lerpSpeed;
		
		if(lerpPercent >= 1){
			lerpPercent = 1;
		}
		if(exchangeToken1 != null){
			ExchangeTokens();
		}
	}

	public void SetupTokenMove(){
		move = true;
		lerpPercent = 0;
	}

	public void SetupTokenExchange(GameObject token1, Vector2 pos1,
	                               GameObject token2, Vector2 pos2, bool reversible){
		SetupTokenMove();

		exchangeToken1 = token1;
		exchangeToken2 = token2;

		exchangeGridPos1 = pos1;
		exchangeGridPos2 = pos2;

		this.userSwap = reversible;
	}

	private void ExchangeTokens(){
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1.x, (int)exchangeGridPos1.y);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2.x, (int)exchangeGridPos2.y);

		Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
		//Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);

		exchangeToken1.transform.position = movePos1;
		//exchangeToken2.transform.position = movePos2;

		if(lerpPercent >= 1){
			gameManager.gridArray[(int)exchangeGridPos2.x, (int)exchangeGridPos2.y] = exchangeToken1;

			if(!matchManager.GridHasMatch() && userSwap){
				SetupTokenExchange(exchangeToken1, exchangeGridPos2, exchangeToken2, exchangeGridPos1, false);
			} else {
				gameManager.gridArray[(int) exchangeGridPos1.x, (int) exchangeGridPos1.y] = null;
				var capturedToken = exchangeToken2;
				Destroy(capturedToken);
				exchangeToken1 = null;
				exchangeToken2 = null;

				move = false;
			}
		}
	}

	private void MoveTokenToEmptyPos(int startGridX, int startGridY,
	                                int endGridX, int endGridY,
	                                GameObject token){
	
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX, startGridY);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);

		Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);

		token.transform.position =	pos;

		if(lerpPercent >= 1){
			gameManager.gridArray[endGridX, endGridY] = token;
			gameManager.gridArray[startGridX, startGridY] = null;
		}
	}

	public bool MoveTokensToFillEmptySpaces(){
		var movedToken = false;

		for(var x = 0; x < gameManager.gridWidth; x++){
			for(var y = 1; y < gameManager.gridHeight ; y++)
			{
				if (!ReferenceEquals(gameManager.gridArray[x, y - 1], null)) continue;
				
				for(var pos = y; pos < gameManager.gridHeight; pos++){
					var token = gameManager.gridArray[x, pos];
					if (ReferenceEquals(token, null)) continue;
					MoveTokenToEmptyPos(x, pos, x, pos - 1, token);
					movedToken = true;
				}
			}
		}

		if(lerpPercent >= 1){
			move = false;
		}

		return movedToken;
	}
}
