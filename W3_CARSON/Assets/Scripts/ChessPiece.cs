using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class ChessPiece : MonoBehaviour
{
    /*public enum PieceType
    {
        King,
        Queen,
        Knight,
        Bishop,
        Rook,
        Pawn
    }

    public PieceType myType;
    
    public bool isMoving { get; private set; }*/
    
    //piece type is one of KQNBRP
    public string pieceType;

    /*public int x, y;
    
    public void MoveToPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            // move towards X, y
            // if close enough, stop moving, set position to XY, and set ismoving to false
        }
    }*/
}
