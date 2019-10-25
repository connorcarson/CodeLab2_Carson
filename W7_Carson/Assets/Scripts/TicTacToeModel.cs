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

    public void PlacePiece(int gridX, int gridY)
    {
        cells[gridX, gridY] = new Cell(_isXTurn);
        _isXTurn = !_isXTurn;
    }

    public bool VerticalWin()
    {
        for (int x = 0; x < 3; x++)
        {
            if(cells[x, 0].gamepiece == cells[x, 1].gamepiece && cells[x, 1].gamepiece == cells[x, 2].gamepiece) return  true;
        }
        return false;
    }

    public bool HorizontalWin()
    {
        for (int y = 0; y < 3; y++)
        {
            if (cells[0, y].gamepiece == cells[1, y].gamepiece && cells[1, y].gamepiece == cells[2, y].gamepiece) return true;
        }
        return false;
    }

    public bool DiagonalWin()
    {
        if (cells[0, 0].gamepiece == cells[1, 1].gamepiece && cells[1, 1].gamepiece == cells[2, 2].gamepiece) return true;
        if (cells[0, 3].gamepiece == cells[1, 1].gamepiece && cells[1, 1].gamepiece == cells[3, 0].gamepiece) return true;
        return false;
    }
    
    public bool CatsGame()
    {
        if (VerticalWin()) return false;
        if (HorizontalWin()) return false;
        if (DiagonalWin()) return false;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (cells[x, y].gamepiece == ' ')
                {
                    return false;
                }
            }
        }
        return true;
    }
}
