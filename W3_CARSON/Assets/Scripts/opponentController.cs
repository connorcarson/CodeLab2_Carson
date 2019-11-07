using System.Collections;
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

    void LookForValidMove()
    {
        for (int x = 0; x < _gameManager.gridWidth; x++)
        {
            for (int y = 0; y < _gameManager.gridHeight; y++)
            {
                var toCheck = _gameManager.gridArray[x, y];
                pos1 = _gameManager.GetPositionOfTokenInGrid(toCheck);

                foreach (var move in ValidMoves(pos1))
                {
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
}
