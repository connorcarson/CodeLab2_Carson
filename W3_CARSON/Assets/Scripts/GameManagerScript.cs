﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

	public bool playHasBegun;
	public int movesLeft;
	public int score;
	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;
	public TextMeshProUGUI movesUI;
	public TextMeshProUGUI scoreUI;
	public TextMeshProUGUI finalScore;
	public GameObject gameOverPanel;

	protected MatchManagerScript matchManager;
	protected InputManagerScript inputManager;
	protected RepopulateScript repopulateManager;
	protected MoveTokensScript moveTokenManager;

	public GameObject grid;
	public  GameObject[,] gridArray;
	protected Object[] tokenTypes;
	GameObject selected;

	public int MovesLeft
	{
		get
		{
			return movesLeft;
		}
		set
		{
			movesLeft = value;
			//movesUI.text = "Moves Left: " + movesLeft;
		}
	}

	public int Score
	{
		get
		{
			return score;
		}
		set
		{
			score = value;
			//scoreUI.text = "Score: " + score;
		}
	}

	public virtual void Start () {
		tokenTypes = (Object[])Resources.LoadAll("Tokens/");
		gridArray = new GameObject[gridWidth, gridHeight];
		MakeGrid();
		matchManager = GetComponent<MatchManagerScript>();
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
	}

	public virtual void Update(){
		//update moves left
		movesUI.text = "Moves Left: " + MovesLeft;
		//update score
		scoreUI.text = "Score: " + Score;
		
		if(!GridHasEmpty()){
			if(matchManager.GridHasMatch()){
				matchManager.RemoveMatches();
			} else {
				inputManager.SelectToken();
			}
		} else {
			if(!moveTokenManager.move){
				moveTokenManager.SetupTokenMove();
			}
			if(!moveTokenManager.MoveTokensToFillEmptySpaces()){
				repopulateManager.AddNewTokensToRepopulateGrid();
			}
		}

		if (Input.GetKey(KeyCode.R))
		{
			SceneManager.LoadScene(0);
		}

		if (MovesLeft <= 0)
		{
			GameOver();
		}
	}

	void MakeGrid() {
		grid = new GameObject("TokenGrid");
		for(var x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
			}
		}
	}

	protected virtual bool GridHasEmpty(){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == null){
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
		finalScore.text = "Score: " + Score;
	}
}