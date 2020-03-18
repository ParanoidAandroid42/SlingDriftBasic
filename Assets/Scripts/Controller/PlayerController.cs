using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    public float speed = 1;

    private GameObject _player;
    private bool _onTap = false;
    private bool _move = false;
    
    private GameObject _activeTarget;

    float speedForce = 0.0003f;
    float driftFactor = 0.92f;

    void Awake()
    {
        InitProperties();
    }

    void StartGame()
    {
        _move = true;
        _player.GetComponent<Rigidbody2D>().isKinematic = false;
        EventManager.TriggerEvent(EventManager.Listener.RoadSpawn.ToString());
    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }
    Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
    }

    void GameOver()
    {
        _onTap = false;
        _move = false;
        _player.GetComponent<Rigidbody2D>().isKinematic = true;
        DOTween.Kill(_player);
    }

    void InitProperties()
    {
        _player = gameObject;
        InitEvents();
    }

    private void InitEvents()
    {
        EventManager.StartListening(EventManager.Listener.StartGame.ToString(), StartGame);
        EventManager.StartListening(EventManager.Listener.GameOver.ToString(), GameOver);
        EventManager.StartListening(EventManager.Listener.StartAnimation.ToString(), PlayInitAnimation);
    }

    private void PlayInitAnimation()
    {
        _player.transform.localPosition = new Vector3(0, -0.97f, -1);
        _player.transform.rotation = Quaternion.Euler(0,0,0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_onTap && _move)
        {
            _onTap = true;
        }

        if (_move)
        {
            OnMove();
        }

        if (Input.GetMouseButtonUp(0) && _move)
        {
            _onTap = false;
        }
    }

    void OnMove()
    {
        Rigidbody2D rb = _player.GetComponent<Rigidbody2D>();
        rb.velocity = ForwardVelocity() + RightVelocity() * driftFactor;
        rb.AddForce(transform.up * speedForce);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        EventManager.TriggerEvent(EventManager.Listener.GameOver.ToString());
    }
}
