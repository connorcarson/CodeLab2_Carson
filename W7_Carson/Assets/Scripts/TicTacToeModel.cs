using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeModel : MonoBehaviour
{
    private Cell[,] cells = new Cell[2,2];
    private bool _isXTurn;
    
    void Start()
    {
        _isXTurn = true;
    }

    public void PlacePiece(int gridX, int gridY, bool isX)
    {
        cells[gridX, gridY] = new Cell(isX);
    }

    public bool VerticalWin()
    {
        for (int x = 0; x < 3; x++)
        {
            if (cells[x, 0].gamepiece == cells[x, 1].gamepiece || cells[x, 1].gamepiece == cells[x, 2].gamepiece)
            {
                Debug.Log(cells[x, 0].gamepiece + " wins!");
                return true;
            }
        }
        return false;
    }

    public bool HorizontalWin()
    {
        for (int y = 0; y < 3; y++)
        {
            if (cells[0, y].gamepiece == cells[1, y].gamepiece || cells[1, y].gamepiece == cells[2, y].gamepiece)
            {
                Debug.Log(cells[0, y].gamepiece + " wins!");
                return true;
            }
        }
        return false;
    }
}
