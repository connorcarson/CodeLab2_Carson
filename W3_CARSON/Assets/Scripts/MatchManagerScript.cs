using UnityEngine;

public class MatchManagerScript : MonoBehaviour {
	
	private GameManagerScript _gameManager;

	public virtual void Start () {
		_gameManager = GetComponent<GameManagerScript>();
	}

	public virtual bool GridHasMatch(){
		bool match = false;
		
		for(int x = 0; x < _gameManager.gridWidth; x++){
			for(int y = 0; y < _gameManager.gridHeight ; y++){
				if(x < _gameManager.gridWidth - 2){
					match = match || GridHasHorizontalMatch(x, y);
				}

				if (y < _gameManager.gridHeight - 2)
				{
					match = match || GridHasVerticalMatch(x, y);
				}
			}
		}
		
		return match;
	}

	public bool GridHasHorizontalMatch(int x, int y){
		GameObject token1 = _gameManager.gridArray[x + 0, y];
		GameObject token2 = _gameManager.gridArray[x + 1, y];
		GameObject token3 = _gameManager.gridArray[x + 2, y];
		
		if(token1 != null && token2 != null && token3 != null){
			ChessPiece.PieceType type1 = token1.GetComponent<ChessPiece>().myType;
			ChessPiece.PieceType type2 = token2.GetComponent<ChessPiece>().myType;
			ChessPiece.PieceType type3 = token3.GetComponent<ChessPiece>().myType;
			
			return (type1 == type2 && type2 == type3);
		} else {
			return false;
		}
	}
	
	public bool GridHasVerticalMatch(int x, int y){
		GameObject token1 = _gameManager.gridArray[x, y + 0];
		GameObject token2 = _gameManager.gridArray[x, y + 1];
		GameObject token3 = _gameManager.gridArray[x, y + 2];
		
		if(token1 != null && token2 != null && token3 != null){
			ChessPiece.PieceType type1 = token1.GetComponent<ChessPiece>().myType;
			ChessPiece.PieceType type2 = token2.GetComponent<ChessPiece>().myType;
			ChessPiece.PieceType type3 = token3.GetComponent<ChessPiece>().myType;
			
			return (type1 == type2 && type2 == type3);
		} else {
			return false;
		}
	}

	private int _GetHorizontalMatchLength(int x, int y){
		int matchLength = 1;
		
		GameObject first = _gameManager.gridArray[x, y];

		if(first != null){
			ChessPiece.PieceType type1 = first.GetComponent<ChessPiece>().myType;
			
			for(int i = x + 1; i < _gameManager.gridWidth; i++){
				GameObject other = _gameManager.gridArray[i, y];

				if(other != null){
					ChessPiece.PieceType type2 = other.GetComponent<ChessPiece>().myType;

					if(type1 == type2){
						matchLength++;
					} else {
						break;
					}
				} else {
					break;
				}
			}
		}
		
		return matchLength;
	}

	private int _GetVerticalMatchLength(int x, int y) {
		int matchLength = 1;

		GameObject first = _gameManager.gridArray[x, y];

		if (first != null) {
			ChessPiece.PieceType type1 = first.GetComponent<ChessPiece>().myType;

			for (int i = y + 1; i < _gameManager.gridHeight; i++) {
				GameObject other = _gameManager.gridArray[x, i];

				if (other != null) {
					ChessPiece.PieceType type2 = other.GetComponent<ChessPiece>().myType;

					if (type1 == type2) {
						matchLength++;
					} else {
						break;
					}
				} else {
					break;
				}
			}
		}

		return matchLength;
	}

	public virtual int RemoveMatches(){
		int numRemoved = 0;

		for(int x = 0; x < _gameManager.gridWidth; x++){
			for(int y = 0; y < _gameManager.gridHeight ; y++){
				if(x < _gameManager.gridWidth - 2){

					int horizonMatchLength = _GetHorizontalMatchLength(x, y);

					if(horizonMatchLength > 2){

						for(int i = x; i < x + horizonMatchLength; i++){
							GameObject token = _gameManager.gridArray[i, y]; 
							Destroy(token);
							if (_gameManager.playHasBegun) {
								if (_gameManager.isPlayersTurn)
								{
									_gameManager.opponentScore++;
								}
								else
								{
									_gameManager.playerScore++;
								}
							}

							_gameManager.gridArray[i, y] = null;
						}
					}
				}

				if (y < _gameManager.gridHeight - 2)
				{

					int verticalMatchLength = _GetVerticalMatchLength(x, y);

					if (verticalMatchLength > 2)
					{
						for (int i = y; i < y + verticalMatchLength; i++)
						{
							GameObject token = _gameManager.gridArray[x, i];
							Destroy(token);
							if (_gameManager.playHasBegun) {
								_gameManager.playerScore++;
							}

							_gameManager.gridArray[x, i] = null;
							numRemoved++;
						}
						
					}
				}
			}
		}
		
		return numRemoved;
	}
}
