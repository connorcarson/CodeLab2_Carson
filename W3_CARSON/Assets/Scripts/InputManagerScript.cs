using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {
	private GameManagerScript _gameManager;
	private MoveTokensScript _moveManager;
	private GameObject _selected = null;
	private Camera _cam;

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
				} else {
					Vector2 pos1 = _gameManager.GetPositionOfTokenInGrid(_selected);
					Vector2 pos2 = _gameManager.GetPositionOfTokenInGrid(tokenCollider.gameObject);

					if((int)(Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y)) == 3
						&& (int)pos1.x != (int)pos2.x && (int)pos1.y != (int)pos2.y){
						_moveManager.SetupTokenExchange(_selected, pos1, tokenCollider.gameObject, pos2, true);
					}

					_selected = null;
				}
			}
		}

	}

	public int CommentFunc(int x, int y){
		return x - y;
	}
}
