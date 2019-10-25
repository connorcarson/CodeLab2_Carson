using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TicTacToeModel model;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            model.PlacePiece(0, 2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            model.PlacePiece(1, 2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            model.PlacePiece(2, 2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            model.PlacePiece(0, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            model.PlacePiece(1, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            model.PlacePiece(2, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            model.PlacePiece(0, 0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            model.PlacePiece(1, 0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            model.PlacePiece(2, 0);
        }
    }
}
