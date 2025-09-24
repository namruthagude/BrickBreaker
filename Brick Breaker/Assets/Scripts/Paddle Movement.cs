using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField]
    private float MinX;
    [SerializeField] 
    private float MaxX;
    [SerializeField]
    private float PaddleSpeed;

    private Vector2 intialPos;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameStateResetted += Game_OnGameStateResetted; ;
        intialPos  = transform.position;
    }

    private void Game_OnGameStateResetted()
    {
        ResettingPaddle();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(Vector2.left * PaddleSpeed * Time.deltaTime);

            
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right *  PaddleSpeed * Time.deltaTime); 
        }

        //Clamping Position
        float Xpos = Mathf.Clamp(transform.position.x, MinX, MaxX);
        transform.position = new Vector3 (Xpos, transform.position.y, transform.position.z);
    }

    private void ResettingPaddle()
    {
        transform.position = intialPos;
    }
}
