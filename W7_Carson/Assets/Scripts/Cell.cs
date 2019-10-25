using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable] 
public class Cell
{
    public bool isX;
    public char gamepiece = ' ';

    public Cell(bool isX)
    {
        this.isX = isX;
        if (isX)
        {
            gamepiece = 'X';
        }
        else
        {
            gamepiece = 'O';
        }
    }

    public Cell()
    {
        //empty constructor
    }
}
