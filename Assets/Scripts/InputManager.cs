using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameManager GameManager { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("esc"))
        {
            // open pause menu
            // TODO
        }

        if (Input.GetKeyDown("up"))
        {
            GameManager.Shift(Direction.Up);
            GameManager.NextTurn();
        }
        else if (Input.GetKeyDown("right"))
        {
            GameManager.Shift(Direction.Right);
            GameManager.NextTurn();
        }
        else if (Input.GetKeyDown("down"))
        {
            GameManager.Shift(Direction.Down);
            GameManager.NextTurn();
        }
        else if (Input.GetKeyDown("left"))
        {
            GameManager.Shift(Direction.Left);
            GameManager.NextTurn();
        }
    }

}
