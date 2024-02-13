using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Controller : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    public float speed;
    public Rigidbody2D rb;//∏’ÃÂ
    public SpriteRenderer sr;

    private void Awake()
    {
        inputControl = new PlayerInputControl();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        if(inputDirection.x==-1)
            sr.flipX = true;
        else sr.flipX = false;
    }
}
