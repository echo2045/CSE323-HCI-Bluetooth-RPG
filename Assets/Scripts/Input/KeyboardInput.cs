using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float MovementSpeed;
    private Vector2 MovementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
   
    void FixedUpdate()
    {
        Move();
        Animate();
    }

    private void Move()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        if (Horizontal == 0 &&  Vertical == 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        MovementInput = new Vector2(Horizontal, Vertical);
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void Animate()
    {
        anim.SetFloat("MovementX", MovementInput.x);
        anim.SetFloat("MovementY", MovementInput.y);
    }
}
