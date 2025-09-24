using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public static Ball Instance;
    public event Action OnBallReachedBottom;
    public event Action OnBallHittedBrick;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float rayLength = 100f;
    [SerializeField]
    private float BallSpeed = 10f;
    [SerializeField]
    private Rigidbody2D _rigidBody;

    private Camera cam;

    private Vector3 launchingDirection;

    public bool isBallLaunched;

    private float _curSpeed;
    private Vector2 initialPos;

   
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        GameManager.Instance.OnGameStateResetted += Game_OnGameStateResetted;
        GameManager.Instance.OnGamePlaying += Game_OnGamePlaying;
        initialPos = transform.position;
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        if(_rigidBody == null)
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        lineRenderer.enabled = false;
        cam = Camera.main;
        _curSpeed = BallSpeed;
    }

    private void Game_OnGameStateResetted()
    {
        ResettingBall();
    }

    private void Game_OnGamePlaying()
    {
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

   

    void Update()
    {
        if (isBallLaunched)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;
            
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3 ballPos = transform.position;
            Vector3 dir = (mousePos - ballPos).normalized;

            launchingDirection = dir;
            // reset to just start and end
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, ballPos);

            Vector2 rayOrigin = (Vector2)ballPos + (Vector2)dir * 2f;
            // Raycast for reflection check
            RaycastHit2D hit = Physics2D.CircleCast(rayOrigin,0.5f,dir);

            
            if (hit.collider != null && hit.collider.CompareTag("Reflect"))
            {
               
                // add hit point
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(1, hit.point);

                // OPTIONAL: handle reflection preview
                Vector2 reflectedDir = Vector2.Reflect(dir, hit.normal);
                lineRenderer.positionCount = 3;
                lineRenderer.SetPosition(2, hit.point + reflectedDir * 2f);
                float distance = hit.distance;
                
            }
            else
            {

                
                // no reflection, just extend 100 units
                Vector3 endPos = ballPos + dir * 100f;
                lineRenderer.SetPosition(1, endPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Release Ball with speed (add your force logic here later)
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            isBallLaunched = true;
            
            _rigidBody.velocity = launchingDirection * BallSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBallLaunched)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Reflect"))
        {
            Vector2 normal = collision.contacts[0].normal;

            // Reflect the current velocity around the normal
            Vector2 reflectedVelocity = Vector2.Reflect(launchingDirection, normal);
            
            launchingDirection = reflectedVelocity.normalized;
            // Apply the new velocity with the same speed
            _rigidBody.velocity = reflectedVelocity.normalized * BallSpeed;

        }

        else if (collision.gameObject.CompareTag("Paddle"))
        {
            
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector2 normal = contactPoint.normal;
            normal = normal.normalized;
            Vector2 reflected = Vector2.Reflect(_rigidBody.velocity, normal);
            _curSpeed = BallSpeed;


            _rigidBody.velocity = reflected * _curSpeed;

        }

        else if (collision.gameObject.CompareTag("Bottom"))
        {
            Debug.Log("Collided with Bottom Boundary");
            isBallLaunched = false;
            OnBallReachedBottom?.Invoke();
        }

        else if (collision.gameObject.CompareTag("Brick"))
        {
            Debug.Log("Collided with Brick");
            Destroy(collision.gameObject);
            OnBallHittedBrick?.Invoke();

        }

    }

    private void ResettingBall()
    {
        //_rigidBody.isKinematic = true;
        //_rigidBody.gravityScale = 0
        _rigidBody.bodyType = RigidbodyType2D.Static;
        transform.position = initialPos;
        _curSpeed = BallSpeed;
    }
}
