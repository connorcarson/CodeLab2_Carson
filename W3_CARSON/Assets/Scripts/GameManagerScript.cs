﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManagerScript : MonoBehaviour
{
	public bool isPlayersTurn = true;
	public bool playHasBegun;
	public int movesLeft;
	public int yourScore;
	public int theirScore;
	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;
	public TextMeshProUGUI movesUI;
	public TextMeshProUGUI yourScoreUI;
	public TextMeshProUGUI theirScoreUI;
	public TextMeshProUGUI finalScore;
	public GameObject gameOverPanel;

	private MatchManagerScript matchManager;
	private InputManagerScript inputManager;
	private RepopulateScript repopulateManager;
	private MoveTokensScript moveTokenManager;
	private OpponentController opponentController;

	public GameObject grid;
	public  GameObject[,] gridArray;
	private Object[] tokenTypes;
	private GameObject selected;

	public int MovesLeft
	{
		get { return movesLeft; }
		set { movesLeft = value; }
	}

	public int YourScore
	{
		get { return yourScore; } 
		set { yourScore = value; }
	}

	public int TheirScore
	{
		get { return theirScore; }
		set { theirScore = value; }
	}

	public enum GameState
	{
		PlayerMakingMove,
		OpponentMakingMove,
		CheckingMatch,
		RemovingMatches,
		FillingBoard,
		BoardSettled
	}

	public GameState currentState;
	
	public virtual void Start () {
		tokenTypes = Resources.LoadAll("ChessPieces/");
		gridArray = new GameObject[gridWidth, gridHeight];
		MakeGrid();
		
		matchManager = GetComponent<MatchManagerScript>();
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
		opponentController = GetComponent<OpponentController>();
	}

	public virtual void Update(){
		//update moves left
		movesUI.text = "Moves Left: " + MovesLeft;
		//update your score
		yourScoreUI.text = "Your Score: " + YourScore;
		//update their score
		theirScoreUI.text = "Their Score: " + TheirScore;

		
		switch (currentState)
		{
			case GameState.BoardSettled:
				if (!GridHasEmpty() && isPlayersTurn) currentState = GameState.PlayerMakingMove;
				else if (!GridHasEmpty() && !isPlayersTurn) currentState = GameState.OpponentMakingMove;
				break;
			case GameState.PlayerMakingMove:
				inputManager.SelectToken();
				currentState = GameState.CheckingMatch;
				break;
			case GameState.OpponentMakingMove:
				StartCoroutine(opponentController.OpponentMove());
				
				currentState = GameState.CheckingMatch;
				break;
			case GameState.CheckingMatch:
				if (matchManager.GridHasMatch())
				{
					currentState = GameState.RemovingMatches;
				}
				else if (isPlayersTurn && !moveTokenManager.move) currentState = GameState.PlayerMakingMove;
				else if (!isPlayersTurn && !moveTokenManager.move) currentState = GameState.OpponentMakingMove;
				break;
			case GameState.RemovingMatches:
				matchManager.RemoveMatches();
				if (GridHasEmpty() && !moveTokenManager.MoveTokensToFillEmptySpaces())
				{
					isPlayersTurn = !isPlayersTurn;
					currentState = GameState.FillingBoard;
				}
				break;
			case GameState.FillingBoard:
				if(!moveTokenManager.move){
					moveTokenManager.SetupTokenMove();
				}
				repopulateManager.AddNewTokensToRepopulateGrid();
				if (!moveTokenManager.MoveTokensToFillEmptySpaces()) currentState = GameState.BoardSettled;
				break;
		}
		
		/*switch (currentState)
		{
			case GameState.BoardSettled:
				if (!GridHasEmpty() && isPlayersTurn) currentState = GameState.PlayerMakingMove;
				else if (!GridHasEmpty() && !isPlayersTurn) currentState = GameState.OpponentMakingMove;
				break;
			case GameState.PlayerMakingMove:
				inputManager.SelectToken();
				currentState = GameState.CheckingMatch;
				break;
			case GameState.OpponentMakingMove:
				StartCoroutine(opponentController.OpponentMove());
				
				currentState = GameState.CheckingMatch;
				break;
			case GameState.CheckingMatch:
				if (matchManager.GridHasMatch())
				{
					currentState = GameState.RemovingMatches;
				}
				else if (isPlayersTurn && !moveTokenManager.move) currentState = GameState.PlayerMakingMove;
				else if (!isPlayersTurn && !moveTokenManager.move) currentState = GameState.OpponentMakingMove;
				break;
			case GameState.RemovingMatches:
				matchManager.RemoveMatches();
				if (GridHasEmpty() && !moveTokenManager.MoveTokensToFillEmptySpaces())
				{
					isPlayersTurn = !isPlayersTurn;
					currentState = GameState.FillingBoard;
				}
				break;
			case GameState.FillingBoard:
				if(!moveTokenManager.move){
					moveTokenManager.SetupTokenMove();
				}
				repopulateManager.AddNewTokensToRepopulateGrid();
				if (!moveTokenManager.MoveTokensToFillEmptySpaces()) currentState = GameState.BoardSettled;
				break;
		}*/
		
		/*if(!GridHasEmpty())
		{
			if (moveTokenManager.lerpPercent >= 1)
			{
				moveTokenManager.move = false;
			}
			if(matchManager.GridHasMatch()){
				matchManager.RemoveMatches();
			} else if (isPlayersTurn && !moveTokenManager.move){
				inputManager.SelectToken();
			} else if(!isPlayersTurn && !moveTokenManager.move) {
				StartCoroutine(opponentController.OpponentMove());
				isPlayersTurn = true;
			}
		} 
		else {
			if(!moveTokenManager.move){
				moveTokenManager.SetupTokenMove();
			}
			if(!moveTokenManager.MoveTokensToFillEmptySpaces()){
				repopulateManager.AddNewTokensToRepopulateGrid();
			}
		}*/

		if (Input.GetKey(KeyCode.R))
		{
			SceneManager.LoadScene(0);
		}

		if (MovesLeft <= 0 && !moveTokenManager.move)
		{
			GameOver();
		}
	}

	private void MakeGrid() {
		grid = new GameObject("TokenGrid");
		
		for(var x = 0; x < gridWidth; x++){
			for(var y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
			}
		}
	}

	private bool GridHasEmpty(){
		for(var x = 0; x < gridWidth; x++){
			
			for(var y = 0; y < gridHeight ; y++){
				
				if(gridArray[x, y] == null) {
					return true;
				}
			}
		}

		return false;
	}

	public Vector2 GetPositionOfTokenInGrid(GameObject token){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == token){
					return(new Vector2(x, y));
				}
			}
		}
		return new Vector2();
	}
		
	public Vector2 GetWorldPositionFromGridPosition(int x, int y){
		return new Vector2(
			(x - gridWidth/2) * tokenSize,
			(y - gridHeight/2) * tokenSize);
	}
	
	public string GetPieceType(GameObject piece)
	{
		return piece.GetComponent<ChessPiece>().pieceType;
	}

	public void AddTokenToPosInGrid(int x, int y, GameObject parent){
		Vector3 position = GetWorldPositionFromGridPosition(x, y);
		GameObject token = 
			Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
			            position, 
			            Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		gridArray[x, y] = token;
	}

	public void GameOver()
	{
		gameOverPanel.SetActive(true);
		finalScore.text = "Score: " + YourScore;
	}
}
