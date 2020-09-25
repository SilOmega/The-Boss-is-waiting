using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulMovement : PhysicsObj
{
    public FloatReference gameState;
    private bool lookingLeft = true;
    [System.NonSerialized]
    private Vector2 startingPosition;
    [System.NonSerialized]
    private float borderX;

    private BoxCollider2D body;
    private SpriteRenderer spriteRenderer;


    // Initialize components and movement's limits
    void OnEnable()
    {
        base.OnEnable();
        body = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingPosition = transform.GetChild(0).position;
        borderX = transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.extents.x;

    }


    // Idle behaviour: patrol inside aggro zone
    private Vector2 Idle()
    {
        Vector2 move = Vector2.zero;

        if (body.bounds.center.x + body.bounds.extents.x - startingPosition.x < borderX && !lookingLeft)
            move = Vector2.right;
        else if (body.bounds.center.x + body.bounds.extents.x - startingPosition.x >= borderX && !lookingLeft)
        {
            move = Vector2.left;
            lookingLeft = !lookingLeft;
            body.offset = new Vector2(-body.offset.x, body.offset.y);
        }
        else if (startingPosition.x - (body.bounds.center.x - body.bounds.extents.x) < borderX && lookingLeft)
            move = Vector2.left;
        else if (startingPosition.x - (body.bounds.center.x - body.bounds.extents.x) >= borderX && lookingLeft)
        {
            move = Vector2.right;
            lookingLeft = !lookingLeft;
            body.offset = new Vector2(-body.offset.x, body.offset.y);
        }
        return move;
    }

    protected override void ComputeVelocity()
    {
        if (gameState.Value != 0)
            this.enabled = false;

        Vector2 move = Idle();
        spriteRenderer.flipX = !lookingLeft;
        targetVelocity = move;
    }

    private void OnDisable()
    {
        this.enabled = false;
    }
}
