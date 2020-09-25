using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fix Game over mechanic to stop the boss
public class GhostKingMovement : PhysicsObj
{
    public FloatReference gameState;
    private BoxCollider2D body;
    private Vector2 startingPosition;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private GhostKingStats stats;
    public Vector2 target = Vector2.zero;

    // Start is called before the first frame update
    void OnEnable()
    {
        base.OnEnable();
        stats = GetComponent<GhostKingStats>();
        body = GetComponent<BoxCollider2D>();
        startingPosition = transform.GetChild(0).position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Idle behaviour: patrol inside aggro zone
    private Vector2 Idle()
    {
        Vector2 move = Vector2.zero;

        if (body.bounds.center.x + body.bounds.extents.x - startingPosition.x < stats.BorderX && !stats.lookingLeft)
            move = Vector2.right;
        else if (body.bounds.center.x + body.bounds.extents.x - startingPosition.x >= stats.BorderX && !stats.lookingLeft)
        {
            move = Vector2.left;
            stats.lookingLeft = !stats.lookingLeft;
            body.offset = new Vector2(-body.offset.x, body.offset.y);
        }
        else if (startingPosition.x - (body.bounds.center.x - body.bounds.extents.x) < stats.BorderX && stats.lookingLeft)
            move = Vector2.left;
        else if (startingPosition.x - (body.bounds.center.x - body.bounds.extents.x) >= stats.BorderX && stats.lookingLeft)
        {
            move = Vector2.right;
            stats.lookingLeft = !stats.lookingLeft;
            body.offset = new Vector2(-body.offset.x, body.offset.y);
        }
        return move;
    }

    protected override void ComputeVelocity()
    {
        // Disable movement on gameover
        if (stats.EnemyCurrentHp == 0)
        {
            gravityModifier.Value = 1;
            animator.SetBool("isDead", true);
        }
            

        Vector2 move = Vector2.zero;
        float speed = 1;

        if (stats.Phase1)
        {
            if (target == Vector2.zero)
                move = Idle();

            // Player spotted and move towards him
            if (target != Vector2.zero)
            {
                Vector2 direction = target - (Vector2)transform.position;
                direction.y = 0f;
                direction = direction.normalized;
                stats.lookingLeft = direction == Vector2.left;
                move = direction;
                speed = 1.5f;
            }
        }

        animator.SetFloat("moveX", speed);
        spriteRenderer.flipX = stats.lookingLeft;
        targetVelocity = move * speed;
    }

    // Teleport ghost king to opposide edge of the Aggro Zone with respect to its current position
    // If it's standing on the left edge then teleport to right edge and viceversa
    public void Teleport(bool firstTime)
    {
        // Phase 2 just started
        if(firstTime)
        {
            if(stats.lookingLeft)
                rb2d.position = startingPosition + new Vector2(stats.BorderX - body.size.x / 2 - 0.1f, 0);
            else
                rb2d.position = startingPosition - new Vector2(stats.BorderX - body.size.x / 2, 0);
        }
        else // Between attacks in phase 2
        {
            if (stats.lookingLeft)
                rb2d.position = startingPosition - new Vector2(stats.BorderX - 1, 0);
            else
                rb2d.position = startingPosition + new Vector2(stats.BorderX - 1, 0);
            stats.lookingLeft = !stats.lookingLeft;
        }

    }

    // Called after the death animation is finished
    private void Die()
    {
        spriteRenderer.enabled = false;
        // Game has been won
        gameState.Value = 2;
        this.enabled = false;
    }

}
