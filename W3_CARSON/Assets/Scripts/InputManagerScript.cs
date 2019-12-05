using System;
using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {
	private GameManagerScript _gameManager;
	private MoveTokensScript _moveManager;
	private GameObject _selected = null;
	private Camera _cam;
	private Vector2 pos1;
	private Vector2 pos2;
	private string currentPieceType;

	public Color32 defaultColor;
	public Color32 selectedColor;
	public Color32 indicatedColor;
	
	public virtual void Start () {
		_moveManager = GetComponent<MoveTokensScript>();
		_gameManager = GetComponent<GameManagerScript>();
		_cam = Camera.main;
	}

	public virtual void SelectToken(){
		if (Input.GetMouseButtonDown(0)){
			Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
			
			Collider2D tokenCollider = Physics2D.OverlapPoint(mousePos);
			if(tokenCollider != null){
				if(_selected == null){
					_selected = tokenCollider.gameObject;
					currentPieceType = _gameManager.GetPieceType(_selected);
					pos1 = _gameManager.GetPositionOfTokenInGrid(_selected);
					IndicateValidMoves(currentPieceType, selectedColor, indicatedColor);
				} else {
					
					pos2 = _gameManager.GetPositionOfTokenInGrid(tokenCollider.gameObject);
					IndicateValidMoves(currentPieceType, defaultColor, defaultColor);					
					//if the absolute value of the tokens' positions is 3
					/*if((int)(Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y)) == 3
						//and the tokens are not at the same position on either the x or y axis
						&& (int)pos1.x != (int)pos2.x && (int)pos1.y != (int)pos2.y){
						//then logically, it must be a "knight's move" and you should move those tokens!
						_moveManager.SetupTokenExchange(_selected, pos1, tokenCollider.gameObject, pos2, true);
						_gameManager.movesLeft--;
						_gameManager.playHasBegun = true;
						_gameManager.isPlayersTurn = false;
					}*/

					_selected = null;
				}
			}
		}
	}

	public void IndicateValidMoves(string pieceType, Color32 token1Color, Color32 token2Color)
	{ 
		_selected.GetComponent<SpriteRenderer>().color = token1Color;
		for (int x = 0; x < _gameManager.gridWidth; x++)
		{
			for (int y = 0; y < _gameManager.gridHeight; y++)
			{
				switch (pieceType)
				{
					case "q":
						if(IsValidQueenMove(x, y)) _gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().color = token2Color;
						break;
					case "k":
						if(IsValidKingMove(x, y)) _gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().color = token2Color;
						break;
					case "b":
						if(IsValidBishopMove(x, y)) _gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().color = token2Color;
						break;
					case "n":
						if(IsValidKnightMove(x, y)) _gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().color = token2Color;
						break;
					case "r":
						if(IsValidRookMove(x, y)) _gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().color = token2Color;
						break;
					case "p":
						if(IsValidPawnMove(x, y)) _gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().color = token2Color;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}

	private void StopIndicatingValidMoves()
	{
		
	}

	private bool IsValidQueenMove(int x, int y)
	{
		if (IsValidBishopMove(x, y) || IsValidRookMove(x, y)) return true;
		return false;
	}

	private bool IsValidKingMove(int x, int y)
	{
		if (x == (int) pos1.x && y == (int) (pos1.y + 1) ||
		    x == (int) pos1.x && y == (int) (pos1.y - 1) ||
		    y == (int) pos1.y && x == (int) (pos1.x + 1) ||
		    y == (int) pos1.y && x == (int) (pos1.x - 1) || 
		    (x == (int) pos1.x + 1 || x == (int) pos1.x - 1) && 
		    (y == (int) pos1.y + 1 || y == (int) pos1.y - 1))
			return true;
		return false;
	}
	private bool IsValidBishopMove(int x, int y)
	{
		for (int i = 1; i < 7; i++)
		{ 
			if((x == (int) pos1.x + i || x == (int) pos1.x - i) && 
			   (y == (int) pos1.y + i || y == (int) pos1.y - i)) 
				return true;
		}
		return false;
	}
	private bool IsValidKnightMove(int x, int y)
	{
		if ((int) (Mathf.Abs(pos1.x - x) + Mathf.Abs(pos1.y - y)) == 3 && (int) pos1.x != x && (int) pos1.y != y) return true;
		return false;
	}

	private bool IsValidRookMove(int x, int y)
	{
		if (Mathf.Abs(x) == (int) Mathf.Abs(pos1.x) && Mathf.Abs(y) != (int) Mathf.Abs(pos1.y) || 
		    Mathf.Abs(x) != (int) Mathf.Abs(pos1.x) && Mathf.Abs(y) == (int) Mathf.Abs(pos1.y)) 
			return true;
		return false;
	}

	private bool IsValidPawnMove(int x, int y)
	{
		if (Mathf.Abs(x) == (int) Mathf.Abs(pos1.x) && y == (int) (pos1.y + 1) ||
		    Mathf.Abs(x) == (int) Mathf.Abs(pos1.x) && y == (int) (pos1.y - 1))
			return true;
		return false;
	}
	
	public int CommentFunc(int x, int y){
		return x - y;
	}
}
