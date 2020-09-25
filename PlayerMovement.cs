using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PhysicsObj
{
    public bool LookingLeft { get; private set; } = false;
    public FloatReference jumpTakeOffSpeed;
    public FloatReference maxSpeed;
    public FloatReference currentHp;
    public FloatReference gameState;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        base.OnEnable();
        GetComponent<AudioListener>().enabled = true;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
   
    

    protected override void ComputeVelocity()
    {
        // If dead and grounded trigger death animation
        if (currentHp.Value <= 0 && grounded)
        {
            animator.SetBool("isDead", true);
            maxSpeed.Value = 0;
            jumpTakeOffSpeed.Value = 0;
        }
            

        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && Grounded)
        {
            velocity.y = jumpTakeOffSpeed.Value;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
                velocity.y *= .5f;
        }

        // Update facing direction variable for sprite renderer 
        if (move.x > 0.01f)
            LookingLeft = false;
        else if (move.x < -0.01f)
            LookingLeft = true;


        spriteRenderer.flipX = LookingLeft;
        animator.SetBool("grounded", Grounded);
        animator.SetFloat("moveX", Mathf.Abs(move.x));

        targetVelocity = move * maxSpeed.Value;
    }

    // Triggered at the end of death animation
    private void Die()
    {
        // Game has been lost
        gameState.Value = 1;
    }

}
