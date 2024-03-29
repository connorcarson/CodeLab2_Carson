﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeModel : MonoBehaviour
{
    public GameObject gamepieceHolder;
    
    private Cell[,] cells = new Cell[3,3];
    private bool _isXTurn;
    private char winningPlayer;

    void Start()
    {
        _isXTurn = true;
        Debug.Log(cells.Length);
        
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                cells[x, y] = new Cell();
            }
        }
    }

    private void Update()
    {
        if (HasWon())
        {
            Debug.Log(winningPlayer + " wins!");
        }

        if (CatsGame())
        {
            Debug.Log("It's a tie!");
        }
    }

    public void PlacePiece(int gridX, int gridY)
    {
        if (cells[gridX, gridY].gamepiece != '-') return;
        
        cells[gridX, gridY] = new Cell(_isXTurn);
        
        var gamepiece = Instantiate(Resources.Load<GameObject>("Prefabs/" + cells[gridX, gridY].gamepiece), gamepieceHolder.transform);
        gamepiece.transform.position = new Vector3(gridX * 3f, gridY * 3f, 0f);
        
        _isXTurn = !_isXTurn;
        Debug.Log("|" + cells[0, 2].gamepiece + "|" + cells[1, 2].gamepiece + "|" + cells[2, 2].gamepiece + "|\n|" + 
                      cells[0, 1].gamepiece + "|" + cells[1, 1].gamepiece + "|" + cells[2, 1].gamepiece + "|\n|" + 
                      cells[0, 0].gamepiece + "|" + cells[1, 0].gamepiece + "|" + cells[2, 0].gamepiece + "|");
    }

    public bool VerticalWin()
    {
        for (int x = 0; x < 2; x++)
        {
            if (cells[x, 0].gamepiece == 'X' && cells[x, 1].gamepiece == 'X' && cells[x, 2].gamepiece == 'X')
            {
                winningPlayer = 'X';
                return true;
            }

            if (cells[x, 0].gamepiece == 'O' && cells[x, 1].gamepiece == 'O' && cells[x, 2].gamepiece == 'O')
            {
                winningPlayer = 'O';
                return true;
            }
        }
        return false;
    }

    public bool HorizontalWin()
    {
        for (int y = 0; y < 2; y++)
        {
            if (cells[0, y].gamepiece == 'X' && cells[1, y].gamepiece == 'X' && cells[2, y].gamepiece == 'X')
            {
                winningPlayer = 'X';
                return true;
            }
            if (cells[0, y].gamepiece == 'O' && cells[1, y].gamepiece == 'O' && cells[2, y].gamepiece == 'O')
            {
                winningPlayer = 'O';
                return true;
            }
        }
        return false;
    }

    public bool DiagonalWin()
    {
        if (cells[0, 0].gamepiece == 'X' && cells[1, 1].gamepiece == 'X' && cells[2, 2].gamepiece == 'X')
        {
            winningPlayer = 'X';
            return true;
        }
        if (cells[0, 0].gamepiece == 'O' && cells[1, 1].gamepiece == 'O' && cells[2, 2].gamepiece == 'O')
        {
            winningPlayer = 'O';
            return true;
        }
        if (cells[0, 2].gamepiece == 'X' && cells[1, 1].gamepiece == 'X' && cells[2, 0].gamepiece == 'X')
        {
            winningPlayer = 'X';
            return true;
        }
        if (cells[0, 2].gamepiece == 'O' && cells[1, 1].gamepiece == 'O' && cells[2, 0].gamepiece == 'O')
        {
            winningPlayer = 'O';
            return true;
        }
        
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
                if (cells[x, y].gamepiece == '-')
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    public bool HasWon()
    {
        if (VerticalWin() || HorizontalWin() || DiagonalWin()) return true;
        return false;
    }
}
