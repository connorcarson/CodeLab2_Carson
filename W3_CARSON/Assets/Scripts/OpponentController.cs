using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    private GameManagerScript _gameManager;
    private Vector2 pos1;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            FindPossibleMatches();
        }
    }

    public void FindPossibleMatches()
    {
        List<Vector2[]> horizontalMatches = new List<Vector2[]>();
        List<Vector2[]> verticalMatches = new List<Vector2[]>();
        Dictionary<Vector2[], int> possibleMatches = new Dictionary<Vector2[], int>();
        
        for (int x = 0; x < _gameManager.gridWidth; x++)
        {
            for (int y = 0; y < _gameManager.gridHeight; y++)
            {
                var toCheck = _gameManager.gridArray[x, y];
                pos1 = _gameManager.GetPositionOfTokenInGrid(toCheck);

                foreach (var move in ValidMoves(pos1))
                {
                    if (InGrid(move))
                    {
                        //if there's a match, add that move to a list of possible moves
                        if (GridHasHorizontalMatch(pos1, move))
                        {
                            horizontalMatches.Add(new[] {pos1, move});

                            if (GridHasVerticalMatch(pos1, move))
                            {
                                verticalMatches.Add(new[] {pos1, move});
                            }
                        }

                    }
                }

                var matchLength = 0;

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
        
        //find match with longest match length
        if (possibleMatches != null)
        {
            Debug.Log("match count: " + possibleMatches.Count);
            var bestMatch = possibleMatches.Values.Max();
            Debug.Log(bestMatch);
        }

        _gameManager.isPlayersTurn = true;
        //make (one of) the move(s) with the longest possible match
    }

    bool InGrid(Vector2 move)
    {
        if (move.x > 0 && move.x < _gameManager.gridWidth - 2 && move.y > 0 && move.y < _gameManager.gridHeight - 2) return true;
        return false;
    }
    Vector2[] ValidMoves(Vector2 pos1)
    {
        Vector2[] validMoves = new[]
        {
            new Vector2(pos1.x + 2, pos1.y + 1), 
            new Vector2(pos1.x + 2, pos1.y - 1),
            new Vector2(pos1.x - 2, pos1.y + 1),
            new Vector2(pos1.x - 2, pos1.y - 1),
            new Vector2(pos1.x + 1, pos1.y + 2),
            new Vector2(pos1.x - 1, pos1.y + 2), 
            new Vector2(pos1.x + 1, pos1.y - 2),
            new Vector2(pos1.x - 1, pos1.y - 2),
        };
        return validMoves;
    }
    
    public bool GridHasHorizontalMatch(Vector2 pos1, Vector2 pos2){
        //Debug.Log((int)pos1.x  + ", " + (int)pos1.y);
        GameObject token1 = _gameManager.gridArray[(int)pos1.x, (int)pos1.y];
        //Debug.Log((int)pos2.x + 1 + ", " + (int)pos2.y);
        GameObject token2 = _gameManager.gridArray[(int)pos2.x + 1, (int)pos2.y];
        //Debug.Log((int)pos2.x + 2 + ", " + (int)pos2.y);
        GameObject token3 = _gameManager.gridArray[(int)pos2.x + 2, (int)pos2.y];

        if(token1 != null && token2 != null && token3 != null){
            SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
            SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
            SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
            return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
        } else {
            return false;
        }
    }
    
    public bool GridHasVerticalMatch(Vector2 pos1, Vector2 pos2){
        //Debug.Log((int)pos1.x  + ", " + (int)pos1.y);
        GameObject token1 = _gameManager.gridArray[(int)pos1.x, (int)pos1.y];
        //Debug.Log((int)pos2.x  + ", " + (int)pos2.y + 1);
        GameObject token2 = _gameManager.gridArray[(int)pos2.x, (int)pos2.y + 1];
        //Debug.Log((int)pos2.x  + ", " + (int)pos2.y + 2);
        GameObject token3 = _gameManager.gridArray[(int)pos2.x, (int)pos2.y + 2];

        if(token1 != null && token2 != null && token3 != null){
            SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
            SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
            SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
            return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
        } else {
            return false;
        }
    }
    
    private int _GetHorizontalMatchLength(Vector2 pos1, Vector2 pos2){
        int matchLength = 1;
		
        GameObject first = _gameManager.gridArray[(int)pos1.x, (int)pos1.y];

        if(first != null){
            SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
            for(int i = (int)pos2.x + 1; i < _gameManager.gridWidth; i++){
                GameObject other = _gameManager.gridArray[i, (int)pos2.y];

                if(other != null){
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    if(sr1.sprite == sr2.sprite){
                        matchLength++;
                    } else {
                        break;
                    }
                } else {
                    break;
                }
            }
            
            for(int i = (int)pos2.x - 1; i >= 0; i--){
                GameObject other = _gameManager.gridArray[i, (int)pos2.y];

                if(other != null){
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    if(sr1.sprite == sr2.sprite){
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
    
    private int _GetVerticalMatchLength(Vector2 pos1, Vector2 pos2) {
        int matchLength = 1;

        GameObject first = _gameManager.gridArray[(int)pos1.x, (int)pos1.y];

        if (first != null) {
            SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();

            for (int i = (int)pos2.y + 1; i < _gameManager.gridHeight; i++) {
                GameObject other = _gameManager.gridArray[(int)pos2.x, i];

                if (other != null) {
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    if (sr1.sprite == sr2.sprite) {
                        matchLength++;
                    } else {
                        break;
                    }
                } else {
                    break;
                }
            }
            
            for (int i = (int)pos2.y - 1; i >= 0; i--) {
                GameObject other = _gameManager.gridArray[(int)pos2.x, i];

                if (other != null) {
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    if (sr1.sprite == sr2.sprite) {
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
