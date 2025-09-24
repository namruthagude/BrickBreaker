using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action OnGameOver;
    public event Action OnGamePlaying;
    public event Action OnGameStateResetted;
    public event Action<int> OnLivesChanged;
    public event Action<int> OnBrickUpdated;

    public enum GameState
    {
        None,
        Playing,
        Reset,
        GameOver
    }

    public GameState state;
    public int Lives = 3;
    public int BricksCollected = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Ball.Instance.OnBallReachedBottom += Ball_OnBallReachedBottom;
        Ball.Instance.OnBallHittedBrick += Ball_OnBallHittedBrick;
        UpdateGameState(GameState.Playing);
    }

    private void Ball_OnBallHittedBrick()
    {
        BricksCollected++;
        OnBrickUpdated?.Invoke(BricksCollected);
    }

    private void Ball_OnBallReachedBottom()
    {
        CheckLives();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckLives()
    {
        if(Lives <= 0)
        {
            UpdateGameState(GameState.GameOver);
        }
        else
        {
            Lives --;
            UpdateGameState(GameState.Reset);
            OnLivesChanged?.Invoke(Lives);
        }
    }

    public void UpdateGameState(GameState state)
    {
        this.state = state;
        ManageGameState(state);
    }

    private void ManageGameState(GameState state)
    {
        switch (state)
        {
            case GameState.None:
                break;
            case GameState.Playing:
                OnGamePlaying?.Invoke();
                break;
            case GameState.GameOver:
                OnGameOver?.Invoke();
                break;
            case GameState.Reset:
                OnGameStateResetted?.Invoke();
                StartCoroutine(ResetCoroutinue());
                break;

        }
    }

    private IEnumerator ResetCoroutinue()
    {
        yield return new WaitForSeconds(2);
        UpdateGameState(GameState.Playing);
    }
}
