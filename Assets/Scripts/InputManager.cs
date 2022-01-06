using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject TitleScreen;

    private GameManager GAME_MANAGER;
    private MovementManager MOVEMENT_MANAGER;

    // Start is called before the first frame update
    void Start()
    {
        GAME_MANAGER = gameObject.GetComponent<GameManager>();
        MOVEMENT_MANAGER = gameObject.GetComponent<MovementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TitleScreen.activeSelf || GAME_MANAGER.GamePaused || !GAME_MANAGER.ReadyForUserInput)
        {
            // game is in title screen or paused or not ready for next user input -> no action possible
            return;
        }

        if (Input.GetKeyDown("up"))
        {
            GAME_MANAGER.ReadyForUserInput = false;
            MOVEMENT_MANAGER.CommandShiftTiles(Direction.Up);
            GAME_MANAGER.NextTurn();
        }
        else if (Input.GetKeyDown("right"))
        {
            GAME_MANAGER.ReadyForUserInput = false;
            MOVEMENT_MANAGER.CommandShiftTiles(Direction.Right);
            GAME_MANAGER.NextTurn();
        }
        else if (Input.GetKeyDown("down"))
        {
            GAME_MANAGER.ReadyForUserInput = false;
            MOVEMENT_MANAGER.CommandShiftTiles(Direction.Down);
            GAME_MANAGER.NextTurn();
        }
        else if (Input.GetKeyDown("left"))
        {
            GAME_MANAGER.ReadyForUserInput = false;
            MOVEMENT_MANAGER.CommandShiftTiles(Direction.Left);
            GAME_MANAGER.NextTurn();
        }
    }

}
