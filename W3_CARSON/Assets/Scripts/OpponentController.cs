using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class OpponentController : MonoBehaviour
{
    private GameManagerScript _gameManager;
    private MoveTokensScript _moveTokensScript;
    private Vector2 pos1;
    private Dictionary<Vector2[], int> possibleMatches;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GetComponent<GameManagerScript>();
        _moveTokensScript = GetComponent<MoveTokensScript>();
    }

    public void FindPossibleMatches()
    {
        List<Vector2[]> horizontalMatches = new List<Vector2[]>();
        List<Vector2[]> verticalMatches = new List<Vector2[]>();
        possibleMatches = new Dictionary<Vector2[], int>();
        
        for (int x = 0; x < _gameManager.gridWidth; x++)
        {
            for (int y = 0; y < _gameManager.gridHeight; y++)
            {
                var toCheck = _gameManager.gridArray[x, y];
                if (toCheck.GetComponent<ChessPiece>().myColor == _gameManager.opponentColor)
                {
                    pos1 = _gameManager.GetPositionOfTokenInGrid(toCheck);
                    var currentPieceType = _gameManager.GetPieceType(toCheck);
                
                    foreach (var move in ValidMoves(pos1, currentPieceType))
                    {
                        if (InGrid(move))
                        {
                            //if there's a match, add that move to a list of possible moves
                            if (GridHasHorizontalMatch(pos1, move))
                            {
                                horizontalMatches.Add(new[] {pos1, move});
                            }
                            if (GridHasVerticalMatch(pos1, move))
                            {
                                verticalMatches.Add(new[] {pos1, move});
                            }
                        }
                    }

                    //add possible match and its match length to our dictionary
                    foreach (var match in horizontalMatches)
                    {
                        if (possibleMatches.ContainsKey(match) == false)
                        {
                            possibleMatches.Add(match, _GetHorizontalMatchLength(match[0], match[1]));
                        }
                    }
                
                    foreach (var match in verticalMatches)
                    {
                        if (possibleMatches.ContainsKey(match) == false)
                        {
                            possibleMatches.Add(match, _GetVerticalMatchLength(match[0], match[1]));
                        }
                    }
                }
            }
        }

        foreach (var match in possibleMatches)
        {
            Debug.Log(match.Key[0] + " , " + match.Key[1] + ": " + match.Value);
        }
    }

    void MakeMatch()
    {
        if (possibleMatches.Count > 0)
        {
            Debug.Log("match count: " + possibleMatches.Count);
            var maxMatchLength = possibleMatches.Values.Max();
            Debug.Log(maxMatchLength);
            var bestMatch = possibleMatches.FirstOrDefault(x => x.Value == maxMatchLength).Key;
            _moveTokensScript.SetupTokenExchange(_gameManager.gridArray[(int)bestMatch[0].x, (int)bestMatch[0].y], bestMatch[0], 
                _gameManager.gridArray[(int)bestMatch[1].x, (int)bestMatch[1].y], bestMatch[1], true);
        }
    }

    public IEnumerator OpponentMove()
    {
        FindPossibleMatches();
        yield return new WaitForSeconds(Random.Range(0.5f, 3.0f));
        MakeMatch();
    }
    
    bool InGrid(Vector2 move)
    {
        if (move.x > 0 && move.x < _gameManager.gridWidth - 2 && move.y > 0 && move.y < _gameManager.gridHeight - 2) return true;
        return false;
    }
    Vector2[] ValidMoves(Vector2 pos1, ChessPiece.PieceType pieceType)
    {
        Vector2[] validMoves;
        
        switch (pieceType)
        {
            case ChessPiece.PieceType.Queen:
                validMoves = new[]
                {
                    #region Queen Moves
                    
                    new Vector2(pos1.x + 1, pos1.y),
                    new Vector2(pos1.x + 2, pos1.y),
                    new Vector2(pos1.x + 3, pos1.y),
                    new Vector2(pos1.x + 4, pos1.y),
                    new Vector2(pos1.x + 5, pos1.y),
                    new Vector2(pos1.x + 6, pos1.y),
                    new Vector2(pos1.x + 7, pos1.y),

                    new Vector2(pos1.x - 1, pos1.y),
                    new Vector2(pos1.x - 2, pos1.y),
                    new Vector2(pos1.x - 3, pos1.y),
                    new Vector2(pos1.x - 4, pos1.y),
                    new Vector2(pos1.x - 5, pos1.y),
                    new Vector2(pos1.x - 6, pos1.y),
                    new Vector2(pos1.x - 7, pos1.y),

                    new Vector2(pos1.x, pos1.y + 1),
                    new Vector2(pos1.x, pos1.y + 2),
                    new Vector2(pos1.x, pos1.y + 3),
                    new Vector2(pos1.x, pos1.y + 4),
                    new Vector2(pos1.x, pos1.y + 5),
                    new Vector2(pos1.x, pos1.y + 6),
                    new Vector2(pos1.x, pos1.y + 7),

                    new Vector2(pos1.x, pos1.y - 1),
                    new Vector2(pos1.x, pos1.y - 2),
                    new Vector2(pos1.x, pos1.y - 3),
                    new Vector2(pos1.x, pos1.y - 4),
                    new Vector2(pos1.x, pos1.y - 5),
                    new Vector2(pos1.x, pos1.y - 6),
                    new Vector2(pos1.x, pos1.y - 7),

                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x + 2, pos1.y + 2),
                    new Vector2(pos1.x + 3, pos1.y + 3),
                    new Vector2(pos1.x + 4, pos1.y + 4),
                    new Vector2(pos1.x + 5, pos1.y + 5),
                    new Vector2(pos1.x + 6, pos1.y + 6),
                    new Vector2(pos1.x + 7, pos1.y + 7),

                    new Vector2(pos1.x - 1, pos1.y - 1),
                    new Vector2(pos1.x - 2, pos1.y - 2),
                    new Vector2(pos1.x - 3, pos1.y - 3),
                    new Vector2(pos1.x - 4, pos1.y - 4),
                    new Vector2(pos1.x - 5, pos1.y - 5),
                    new Vector2(pos1.x - 6, pos1.y - 6),
                    new Vector2(pos1.x - 7, pos1.y - 7),

                    new Vector2(pos1.x + 1, pos1.y - 1),
                    new Vector2(pos1.x + 2, pos1.y - 2),
                    new Vector2(pos1.x + 3, pos1.y - 3),
                    new Vector2(pos1.x + 4, pos1.y - 4),
                    new Vector2(pos1.x + 5, pos1.y - 5),
                    new Vector2(pos1.x + 6, pos1.y - 6),
                    new Vector2(pos1.x + 7, pos1.y - 7),

                    new Vector2(pos1.x - 1, pos1.y + 1),
                    new Vector2(pos1.x - 2, pos1.y + 2),
                    new Vector2(pos1.x - 3, pos1.y + 3),
                    new Vector2(pos1.x - 4, pos1.y + 4),
                    new Vector2(pos1.x - 5, pos1.y + 5),
                    new Vector2(pos1.x - 6, pos1.y + 6),
                    new Vector2(pos1.x - 7, pos1.y + 7),

                    #endregion
                };
                break;
            case ChessPiece.PieceType.King:
                validMoves = new []
                {
                    #region King Moves
                    new Vector2(pos1.x + 1, pos1.y),
                    new Vector2(pos1.x - 1, pos1.y),
                    new Vector2(pos1.x, pos1.y + 1),
                    new Vector2(pos1.x, pos1.y - 1), 
                
                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x - 1, pos1.y - 1),
                    new Vector2(pos1.x + 1, pos1.y - 1),
                    new Vector2(pos1.x - 1, pos1.y + 1),
                    #endregion
                };
                break;
            case ChessPiece.PieceType.Knight:
                validMoves = new[]
                {
                    #region Knight Moves

                    new Vector2(pos1.x + 2, pos1.y + 1),
                    new Vector2(pos1.x + 2, pos1.y - 1),
                    new Vector2(pos1.x - 2, pos1.y + 1),
                    new Vector2(pos1.x - 2, pos1.y - 1),
                    new Vector2(pos1.x + 1, pos1.y + 2),
                    new Vector2(pos1.x - 1, pos1.y + 2),
                    new Vector2(pos1.x + 1, pos1.y - 2),
                    new Vector2(pos1.x - 1, pos1.y - 2),

                    #endregion
                };
                break;
            case ChessPiece.PieceType.Bishop:
                validMoves = new[]
                {
                    #region Bishop Moves

                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x + 2, pos1.y + 2),
                    new Vector2(pos1.x + 3, pos1.y + 3),
                    new Vector2(pos1.x + 4, pos1.y + 4),
                    new Vector2(pos1.x + 5, pos1.y + 5),
                    new Vector2(pos1.x + 6, pos1.y + 6),
                    new Vector2(pos1.x + 7, pos1.y + 7),

                    new Vector2(pos1.x - 1, pos1.y - 1),
                    new Vector2(pos1.x - 2, pos1.y - 2),
                    new Vector2(pos1.x - 3, pos1.y - 3),
                    new Vector2(pos1.x - 4, pos1.y - 4),
                    new Vector2(pos1.x - 5, pos1.y - 5),
                    new Vector2(pos1.x - 6, pos1.y - 6),
                    new Vector2(pos1.x - 7, pos1.y - 7),

                    new Vector2(pos1.x + 1, pos1.y - 1),
                    new Vector2(pos1.x + 2, pos1.y - 2),
                    new Vector2(pos1.x + 3, pos1.y - 3),
                    new Vector2(pos1.x + 4, pos1.y - 4),
                    new Vector2(pos1.x + 5, pos1.y - 5),
                    new Vector2(pos1.x + 6, pos1.y - 6),
                    new Vector2(pos1.x + 7, pos1.y - 7),

                    new Vector2(pos1.x - 1, pos1.y + 1),
                    new Vector2(pos1.x - 2, pos1.y + 2),
                    new Vector2(pos1.x - 3, pos1.y + 3),
                    new Vector2(pos1.x - 4, pos1.y + 4),
                    new Vector2(pos1.x - 5, pos1.y + 5),
                    new Vector2(pos1.x - 6, pos1.y + 6),
                    new Vector2(pos1.x - 7, pos1.y + 7),

                    #endregion
                };
                break;
            case ChessPiece.PieceType.Rook:
                validMoves = new[]
                {
                    #region Rook Moves

                    new Vector2(pos1.x + 1, pos1.y),
                    new Vector2(pos1.x + 2, pos1.y),
                    new Vector2(pos1.x + 3, pos1.y),
                    new Vector2(pos1.x + 4, pos1.y),
                    new Vector2(pos1.x + 5, pos1.y),
                    new Vector2(pos1.x + 6, pos1.y),
                    new Vector2(pos1.x + 7, pos1.y),

                    new Vector2(pos1.x - 1, pos1.y),
                    new Vector2(pos1.x - 2, pos1.y),
                    new Vector2(pos1.x - 3, pos1.y),
                    new Vector2(pos1.x - 4, pos1.y),
                    new Vector2(pos1.x - 5, pos1.y),
                    new Vector2(pos1.x - 6, pos1.y),
                    new Vector2(pos1.x - 7, pos1.y),

                    new Vector2(pos1.x, pos1.y + 1),
                    new Vector2(pos1.x, pos1.y + 2),
                    new Vector2(pos1.x, pos1.y + 3),
                    new Vector2(pos1.x, pos1.y + 4),
                    new Vector2(pos1.x, pos1.y + 5),
                    new Vector2(pos1.x, pos1.y + 6),
                    new Vector2(pos1.x, pos1.y + 7),

                    new Vector2(pos1.x, pos1.y - 1),
                    new Vector2(pos1.x, pos1.y - 2),
                    new Vector2(pos1.x, pos1.y - 3),
                    new Vector2(pos1.x, pos1.y - 4),
                    new Vector2(pos1.x, pos1.y - 5),
                    new Vector2(pos1.x, pos1.y - 6),
                    new Vector2(pos1.x, pos1.y - 7),

                    #endregion
                };
                break;
            case ChessPiece.PieceType.Pawn:
                validMoves = new[]
                {
                    #region Pawn Moves

                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x - 1, pos1.y + 1),

                    #endregion
                };
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return validMoves;
    }
    
    public bool GridHasHorizontalMatch(Vector2 piece1, Vector2 piece2){
        //Debug.Log((int)pos1.x  + ", " + (int)pos1.y);
        GameObject token1 = _gameManager.gridArray[(int)piece1.x, (int)piece1.y];
        //Debug.Log((int)pos2.x + 1 + ", " + (int)pos2.y);
        GameObject token2 = _gameManager.gridArray[(int)piece2.x + 1, (int)piece2.y];
        //Debug.Log((int)pos2.x + 2 + ", " + (int)pos2.y);
        GameObject token3 = _gameManager.gridArray[(int)piece2.x + 2, (int)piece2.y];

        if(token1 != null && token2 != null && token3 != null){
            ChessPiece.PieceType type1 = token1.GetComponent<ChessPiece>().myType;
            ChessPiece.PieceType type2 = token2.GetComponent<ChessPiece>().myType;
            ChessPiece.PieceType type3 = token3.GetComponent<ChessPiece>().myType;
			
            return (type1 == type2 && type2 == type3);
        } else {
            return false;
        }
    }
    
    public bool GridHasVerticalMatch(Vector2 piece1, Vector2 piece2){
        //Debug.Log((int)pos1.x  + ", " + (int)pos1.y);
        GameObject token1 = _gameManager.gridArray[(int)piece1.x, (int)piece1.y];
        //Debug.Log((int)pos2.x  + ", " + (int)pos2.y + 1);
        GameObject token2 = _gameManager.gridArray[(int)piece2.x, (int)piece2.y + 1];
        //Debug.Log((int)pos2.x  + ", " + (int)pos2.y + 2);
        GameObject token3 = _gameManager.gridArray[(int)piece2.x, (int)piece2.y + 2];

        if(token1 != null && token2 != null && token3 != null){
            ChessPiece.PieceType type1 = token1.GetComponent<ChessPiece>().myType;
            ChessPiece.PieceType type2 = token2.GetComponent<ChessPiece>().myType;
            ChessPiece.PieceType type3 = token3.GetComponent<ChessPiece>().myType;
			
            return (type1 == type2 && type2 == type3);
        } else {
            return false;
        }
    }
    
    private int _GetHorizontalMatchLength(Vector2 piece1, Vector2 piece2){
        int matchLength = 1;
		
        GameObject first = _gameManager.gridArray[(int)piece1.x, (int)piece1.y];

        if(first != null){
            ChessPiece.PieceType type1 = first.GetComponent<ChessPiece>().myType;
			
            for(int i = (int)piece2.x + 1; i < _gameManager.gridWidth; i++){
                GameObject other = _gameManager.gridArray[i, (int)piece2.y];

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
            
            for(int i = (int)piece2.x - 1; i >= 0; i--){
                GameObject other = _gameManager.gridArray[i, (int)piece2.y];

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
    
    private int _GetVerticalMatchLength(Vector2 piece1, Vector2 piece2) {
        int matchLength = 1;

        GameObject first = _gameManager.gridArray[(int)piece1.x, (int)piece1.y];

        if (first != null) {
            ChessPiece.PieceType type1 = first.GetComponent<ChessPiece>().myType;

            for (int i = (int)piece2.y + 1; i < _gameManager.gridHeight; i++) {
                GameObject other = _gameManager.gridArray[(int)piece2.x, i];

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
            
            for (int i = (int)piece2.y - 1; i >= 0; i--) {
                GameObject other = _gameManager.gridArray[(int)piece2.x, i];

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
}
