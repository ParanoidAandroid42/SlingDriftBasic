using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject startText;
    public GameObject logo;
    public GameObject roadContainer;
    public GameObject cam;
    public GameObject gameOver;
    public Button retryButton;

    private bool _onTap = false;

    // Start is called before the first frame update
    void Awake()
    {
        InitProperties();
    }

    void InitProperties()
    {
        gameOver.SetActive(false);
        InitEvents();
        EventManager.TriggerEvent(EventManager.Listener.StartAnimation.ToString());
    }

    void InitEvents()
    {
        EventManager.StartListening(EventManager.Listener.GameOver.ToString(), GameOver);
        EventManager.StartListening(EventManager.Listener.StartAnimation.ToString(), PlayInitAnimation);
        retryButton.onClick.AddListener(() => Retry());
    }

    void Retry()
    {
        gameOver.SetActive(false);
        PoolerManager.Instance.SetDeActiveAll();
        EventManager.TriggerEvent(EventManager.Listener.StartAnimation.ToString());
    }

    void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayInitAnimation()
    {
        _onTap = false;
        Time.timeScale = 1;
        startText.SetActive(false);
        logo.SetActive(true);
        logo.transform.DOMoveX(0, 2).From(10).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            startText.SetActive(true);
            startText.transform.DOScale(new Vector3(.25f, .25f, .25f), .35f).SetLoops(-1, LoopType.Yoyo);
            _onTap = true;
        });
        startText.transform.position = new Vector3(0, 1, -1);
        roadContainer.transform.DOScale(new Vector3(1.8f, 1, 1), 2).From(new Vector3(1, 1, 1));
        cam.transform.DOLocalMove(new Vector3(0, 1, -9), 1).From(new Vector3(0, 0, -14));
    }

    void OnTap()
    {
        logo.transform.DOMoveX(10, 1).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            logo.SetActive(false);
            cam.transform.DOLocalMove(new Vector3(0, 5, -14), 1);
            EventManager.TriggerEvent(EventManager.Listener.StartGame.ToString());
        });
        roadContainer.transform.DOScale(new Vector3(1, 1, 1), .5f);
        startText.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _onTap)
        {
            _onTap = false;
            OnTap();
        }
    }
}
