using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class JointController : MonoBehaviour
{
    public LineRenderer line;
    private GameObject _player;
    float distance = 4.5f;
    private FixedJoint2D _joint;

    private Rigidbody2D _playerBody;
    private bool _active = false;
    private bool _init = false;

    public static bool horizontal = false;
    public GameObject target;

    private bool _gameOver = false;

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _joint = GetComponent<FixedJoint2D>();
        _playerBody = _player.GetComponent<Rigidbody2D>();
        _active = false;
        EventManager.StartListening(EventManager.Listener.GameOver.ToString(), GameOver);
        EventManager.StartListening(EventManager.Listener.StartGame.ToString(), StartGame);
    }

    void StartGame()
    {
        _gameOver = false;
    }

    void GameOver()
    {
        DOTween.Kill(_player);
        _joint.connectedBody = null;
        _gameOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        float dictanceOffset = Vector2.Distance(transform.position, _player.transform.position);

        if (distance > dictanceOffset && _gameOver == false)
        {
            gameObject.GetComponent<SpriteRenderer>().DOFade(1, .3f);
            if (Input.GetMouseButton(0) && !_active)
            {
                if (!_init)
                {
                    gameObject.GetComponent<SpriteRenderer>().DOFade(1, .3f);
                    EventManager.TriggerEvent(EventManager.Listener.RoadSpawn.ToString());
                    JointController.horizontal = !horizontal;
                    _init = true;
                    OnRotate();
                }
                else
                {
                    line.gameObject.SetActive(true);
                    line.SetPosition(0, _player.transform.position);
                    line.SetPosition(1, transform.position);
                    _joint.connectedBody = _playerBody;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                DOTween.Kill(gameObject);
                line.gameObject.SetActive(false);
                _joint.connectedBody = null;
                gameObject.GetComponent<SpriteRenderer>().DOFade(.3f, .3f);
                _active = true;
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().DOFade(.3f, .3f);
            _joint.connectedBody = null;
        }
    }

    void OnRotate()
    {
        float z = transform.rotation.z;
        float inc = 90;
        DOTween.Kill(_player);


        if (target.transform.localScale.x < 0 && target.transform.localScale.y > 0)
            inc = -90;
        else if (target.transform.localScale.x > 0 && target.transform.localScale.y < 0)
            inc = -90;
        else if (target.transform.localScale.x > 0 && target.transform.localScale.y > 0)
            inc = 90;
        else if (target.transform.localScale.x < 0 && target.transform.localScale.y < 0)
            inc = 90;


        if (JointController.horizontal)
        {
            z += inc;
            transform.DORotate(new Vector3(0, 0, z), .5f);
        }
        else
        {
            z -= inc;
            transform.DORotate(new Vector3(0, 0, z), .5f);
        }
    }
}
