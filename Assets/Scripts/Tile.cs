using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject targetCellGameObject;
    private Cell _targetCell;
    private Vector3 _moveDirection;
    private bool _doMove = false;
    private float _moveSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {
        _targetCell = targetCellGameObject.GetComponent<Cell>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _doMove = true;
        }

        if (_doMove)
        {
            _moveDirection = new Vector3(-1, 0, 0);

            MoveTile();

            
        }
        
    }


    public void CommandMoveTile(Cell targetCell, Direction direction)
    {
        _targetCell = targetCell;
        _moveDirection = DirectionToVector3(direction);
        _doMove = true;
    }

    public void MoveTile()
    {
        gameObject.transform.position += _moveDirection * _moveSpeed * Time.deltaTime;

        if (gameObject.transform.position.x < _targetCell.gameObject.transform.position.x)
        {
            gameObject.transform.position = new Vector3(_targetCell.gameObject.transform.position.x,
                                                        gameObject.transform.position.y,
                                                        gameObject.transform.position.z);
            _doMove = false;
        }
    }

    public Vector3 DirectionToVector3(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3(0, 0, -1);
            case Direction.Right:
                return new Vector3(-1, 0, 0);
            case Direction.Down:
                return new Vector3(0, 0, 1);
            default:  // case: Left
                return new Vector3(1, 0, 0);
        }
    }



}
