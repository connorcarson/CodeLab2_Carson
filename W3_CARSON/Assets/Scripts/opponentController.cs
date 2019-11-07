﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opponentController : MonoBehaviour
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

    }

    private void FindPossibleMatches()
    {
        List<Vector2[]> possibleMoves = new List<Vector2[]>();
        
        for (int x = 0; x < _gameManager.gridWidth; x++)
        {
            for (int y = 0; y < _gameManager.gridHeight; y++)
            {
                var toCheck = _gameManager.gridArray[x, y];
                pos1 = _gameManager.GetPositionOfTokenInGrid(toCheck);

                foreach (var move in ValidMoves(pos1))
                {
                    if (GridHasHorizontalMatch(pos1, move))
                    {
                        possibleMoves.Add(new []{pos1, move});
                    }
                    if (GridHasVerticalMatch(pos1, move))
                    {
                        possibleMoves.Add(new []{pos1, move});
                    }
                    //if there's a match, add that move to a list of possible moves
                    //rank according to the length of the match
                    //make (one of) the move(s) with the longest possible match
                }
            }
        }
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
        GameObject token1 = _gameManager.gridArray[(int)pos1.x, (int)pos1.y];
        GameObject token2 = _gameManager.gridArray[(int)pos2.x + 1, (int)pos2.y];
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
        GameObject token1 = _gameManager.gridArray[(int)pos1.x, (int)pos1.y + 0];
        GameObject token2 = _gameManager.gridArray[(int)pos2.x, (int)pos1.y + 1];
        GameObject token3 = _gameManager.gridArray[(int)pos2.x, (int)pos1.y + 2];
		
        if(token1 != null && token2 != null && token3 != null){
            SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
            SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
            SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
            return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
        } else {
            return false;
        }
    }
}