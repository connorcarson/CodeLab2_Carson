using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {
	private GameManagerScript _gameManager;
	private MoveTokensScript _moveManager;
	private GameObject _selected = null;
	private Camera _cam;
	private Vector2 pos1;
	private Vector2 pos2;

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
					pos1 = _gameManager.GetPositionOfTokenInGrid(_selected);
					IndicateTokenMatches(selectedColor, indicatedColor);
				} else {
					pos2 = _gameManager.GetPositionOfTokenInGrid(tokenCollider.gameObject);
					IndicateTokenMatches(defaultColor, defaultColor);					
					//if the absolute value of the tokens' positions is 3
					if((int)(Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y)) == 3
						//and the tokens are not at the same position on either the x or y axis
						&& (int)pos1.x != (int)pos2.x && (int)pos1.y != (int)pos2.y){
						//then logically, it must be a "knight's move" and you should move those tokens!
						_moveManager.SetupTokenExchange(_selected, pos1, tokenCollider.gameObject, pos2, true);
						_gameManager.movesLeft--;
						_gameManager.playHasBegun = true;
					}

					_selected = null;
				}
			}
		}

	}

	public void IndicateTokenMatches(Color32 token1Color, Color32 token2Color)
	{ 
		_selected.GetComponent<SpriteRenderer>().color = token1Color;
		for (int x = 0; x < _gameManager.gridWidth; x++)
		{
			for (int y = 0; y < _gameManager.gridHeight; y++)
			{
				if ((int) (Mathf.Abs(pos1.x - x) + Mathf.Abs(pos1.y - y)) == 3
				    && (int) pos1.x != x && (int) pos1.y != y)
				{
					_gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().color = token2Color;
				}
			}
		}
	}

	public int CommentFunc(int x, int y){
		return x - y;
	}
}
