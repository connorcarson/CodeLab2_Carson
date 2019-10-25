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
        if (Input.GetKey(KeyCode.Alpha1))
        {
            model.PlacePiece(0, 2);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            model.PlacePiece(1, 2);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            model.PlacePiece(2, 2);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            model.PlacePiece(0, 1);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            model.PlacePiece(1, 1);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            model.PlacePiece(2, 1);
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            model.PlacePiece(0, 0);
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            model.PlacePiece(1, 0);
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            model.PlacePiece(2, 0);
        }
    }
}
