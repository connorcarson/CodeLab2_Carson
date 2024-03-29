﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManagerScript : MonoBehaviour
{
	public bool isPlayersTurn = true;
	public bool playHasBegun;
	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;
	public GameObject gameOverPanel;
	public ChessPiece.PieceColor playerColor, opponentColor;
	public int movesLeft, playerScore, opponentScore;
	public TextMeshProUGUI movesText, playerScoreText, opponentScoreText, finalScoreText;

	private MatchManagerScript matchManager;
	private InputManagerScript inputManager;
	private RepopulateScript repopulateManager;
	private MoveTokensScript moveTokenManager;
	private OpponentController opponentController;

	public GameObject grid;
	public  GameObject[,] gridArray;
	private Object[] _tokenTypes;
	//private GameObject _selected;
	
	public int MovesLeft
	{
		get { return movesLeft; }
		set { movesLeft = value; }
	}

	public int PlayerScore
	{
		get { return playerScore; } 
		set { playerScore = value; }
	}

	public int OpponentScore
	{
		get { return opponentScore; }
		set { opponentScore = value; }
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
		_tokenTypes = Resources.LoadAll("ChessPieces/");
		gridArray = new GameObject[gridWidth, gridHeight];
		MakeGrid();

		if (playerColor == ChessPiece.PieceColor.Black)
		{
			opponentColor = ChessPiece.PieceColor.White;
		}
		else
		{
			opponentColor = ChessPiece.PieceColor.Black;
		}

		matchManager = GetComponent<MatchManagerScript>();
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
		opponentController = GetComponent<OpponentController>();
	}

	public virtual void Update(){
		//update moves left
		movesText.text = "Moves Left: " + MovesLeft;
		//update your score
		playerScoreText.text = "Your Score: " + PlayerScore;
		//update their score
		opponentScoreText.text = "Their Score: " + OpponentScore;

		switch (currentState)
		{
			case GameState.BoardSettled:
				if (isPlayersTurn) currentState = GameState.PlayerMakingMove;
				else {currentState = GameState.OpponentMakingMove;}
				break;
			case GameState.PlayerMakingMove:
				inputManager.SelectToken();
				if (matchManager.GridHasMatch())
				{
					//isPlayersTurn = false;
					movesLeft--;
					currentState = GameState.RemovingMatches;
				}
				break;
			case GameState.OpponentMakingMove:
				StartCoroutine(opponentController.OpponentMove());
				currentState = GameState.CheckingMatch;
				break;
			case GameState.CheckingMatch:
				if (matchManager.GridHasMatch())
				{
					//isPlayersTurn = true;
					currentState = GameState.RemovingMatches;
				}
				else if (!moveTokenManager.move) currentState = GameState.OpponentMakingMove;
				break;
			case GameState.RemovingMatches:
				matchManager.RemoveMatches();
				if (GridHasEmpty() && !moveTokenManager.MoveTokensToFillEmptySpaces())
				{
					currentState = GameState.FillingBoard;
				}
				break;
			case GameState.FillingBoard:
				if(!moveTokenManager.move){
					moveTokenManager.SetupTokenMove();
				}
				if (!moveTokenManager.MoveTokensToFillEmptySpaces()) {
					repopulateManager.AddNewTokensToRepopulateGrid();
				}
				if (matchManager.GridHasMatch()) currentState = GameState.RemovingMatches;
				if (!GridHasEmpty() && !matchManager.GridHasMatch())
				{
					isPlayersTurn = !isPlayersTurn;
					currentState = GameState.BoardSettled;
				}
				break;
		}

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
	
	public ChessPiece.PieceType GetPieceType(GameObject piece)
	{
		return piece.GetComponent<ChessPiece>().myType;
	}

	public void AddTokenToPosInGrid(int x, int y, GameObject parent){
		Vector3 position = GetWorldPositionFromGridPosition(x, y);
		GameObject token = 
			Instantiate(_tokenTypes[Random.Range(0, _tokenTypes.Length)], 
			            position, 
			            Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		gridArray[x, y] = token;
	}

	public void GameOver()
	{
		gameOverPanel.SetActive(true);
		finalScoreText.text = "Score: " + PlayerScore;
	}
}
