using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text_Score;
    [SerializeField]
    private TMP_Text text_Lives;
    // Start is called before the first frame update
    void Start()
    {
        text_Lives.text = 3.ToString();
        text_Score.text = 0.ToString();
        GameManager.Instance.OnLivesChanged += Game_OnLivesChanged;
        GameManager.Instance.OnBrickUpdated += GameManager_OnBrickUpdated;
    }

    private void GameManager_OnBrickUpdated(int bricks)
    {
        text_Score.text = bricks.ToString();
    }

    private void Game_OnLivesChanged(int lives)
    {
        text_Lives.text = lives.ToString() ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
