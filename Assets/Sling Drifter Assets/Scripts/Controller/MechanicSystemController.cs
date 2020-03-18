using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MechanicSystemController : MonoBehaviour
{
    public float spawnTime = 5.0f;
    public enum Direction { 
        Up,
        Left,
        Down,
        Right
    }

    public Dictionary<string, Vector2> roadsScale;

    private Direction _currentDirection = Direction.Up;
    private Direction _previousDirection = Direction.Left;
    private Vector2 _currentPosition;

    public GameObject player;

    void Awake()
    {
        _currentPosition = new Vector2(0, 10f);
        InitProperties();
    }

    void InitProperties()
    {
        roadsScale = new Dictionary<string, Vector2>();
        roadsScale.Add(Direction.Up + "" + Direction.Left, new Vector2(3, 3.2f));
        roadsScale.Add(Direction.Up + "" + Direction.Right, new Vector2(-3, 3.2f));
        roadsScale.Add(Direction.Down + "" + Direction.Left, new Vector2(3, -3.2f));
        roadsScale.Add(Direction.Down + "" + Direction.Right, new Vector2(-3, -3.2f));
        InitEvents();
    }

    private void InitEvents()
    {
        EventManager.StartListening(EventManager.Listener.RoadSpawn.ToString(), RoadSpawn);
        EventManager.StartListening(EventManager.Listener.StartGame.ToString(), StartGame);
        EventManager.StartListening(EventManager.Listener.GameOver.ToString(), GameOver);
    }

    public void RoadSpawn()
    {
        switch (_currentDirection)
        {
            case Direction.Up:
                CreateHorizontalRoad(player.transform.position);
                break;
            case Direction.Left:
                CreateVectoralRoad(player.transform.position);
                break;
            case Direction.Down:
                CreateHorizontalRoad(player.transform.position);
                break;
            case Direction.Right:
                CreateVectoralRoad(player.transform.position);
                break;
        }
    }

    void CreateHorizontalRoad(Vector3 playerPosition)
    {
        float moveX = 0f;
        _previousDirection = _currentDirection;

        if (MakeChoice() == 0)
        {
            //left
            _currentDirection = Direction.Left;
            moveX = -3.83f;
        }
        else
        {
            //right 
            _currentDirection = Direction.Right;
            moveX = 3.83f;
        }

        GameObject obj = PoolerManager.Instance.SpawnPoolTag("Turn", new Vector3(_currentPosition.x, _currentPosition.y, 0));
        obj.transform.localScale = roadsScale[_previousDirection+""+_currentDirection];
        _currentPosition.x += moveX;
        PoolerManager.Instance.SpawnPoolTag(_currentDirection.ToString(), new Vector3(_currentPosition.x, _currentPosition.y, 0));
        _currentPosition.x += moveX;
        PoolerManager.Instance.SpawnPoolTag(_currentDirection.ToString(), new Vector3(_currentPosition.x, _currentPosition.y, 0));
        _currentPosition.x += moveX;
    }

    void CreateVectoralRoad(Vector3 playerPosition)
    {
        float moveY = 0f;
        _previousDirection = _currentDirection;

        if (MakeChoice() == 0)
        {
            //up
            _currentDirection = Direction.Up;
            moveY = 4;
        }
        else
        {
            //down
            _currentDirection = Direction.Down;
            moveY = -4f;
        }

        GameObject obj = PoolerManager.Instance.SpawnPoolTag("Turn", new Vector3(_currentPosition.x, _currentPosition.y, 0));
        obj.transform.localScale = -roadsScale[_currentDirection+""+_previousDirection];
        _currentPosition.y += moveY;
        PoolerManager.Instance.SpawnPoolTag(_currentDirection.ToString(), new Vector3(_currentPosition.x, _currentPosition.y, 0));
        _currentPosition.y += moveY;
        PoolerManager.Instance.SpawnPoolTag(_currentDirection.ToString(), new Vector3(_currentPosition.x, _currentPosition.y, 0));
        _currentPosition.y += moveY;
    }

    int MakeChoice()
    {
        int random = Random.Range(0,2);
        return random;
    }

    void StartGame()
    {
    }

    void GameOver()
    {
        _currentPosition = new Vector2(0, 10f);
        _currentDirection = Direction.Up;
        _previousDirection = Direction.Left;
    }
}
