using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private GameObject go_HudUI;
    [SerializeField]
    private GameObject go_GameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGamePlaying += Game_OnGamePlaying;
        GameManager.Instance.OnGameOver += Game_OnGameOver;
    }

    private void Game_OnGameOver()
    {
        ShowGameOverUI();
    }

    private void Game_OnGamePlaying()
    {
        ShowHudUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TurnOffAllPanels()
    {
        go_HudUI.SetActive(false);
        go_GameOverUI.SetActive(false);
    }

    public void ShowHudUI()
    {
        TurnOffAllPanels();
        go_HudUI.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        TurnOffAllPanels();
        go_GameOverUI.SetActive(true);
    }
}
