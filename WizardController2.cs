using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController2 : MonoBehaviour
{

    private PolygonCollider2D body;
    private BoxCollider2D visionArea;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private WizardStats stats;

    void OnEnable()
    {
        body = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
        stats = GetComponent<WizardStats>();
        visionArea = transform.GetChild(1).GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Flip colliders and sprite depending on looking direction of wizard
        if (stats.lookingRight)
        {
            spriteRenderer.flipX = stats.lookingRight;
            visionArea.offset = new Vector2(-visionArea.offset.x, visionArea.offset.y);
            var bodyShape = body.points;
            for (int i = 0; i < bodyShape.Length; i++)
            {
                bodyShape[i].x *= -1;
            }
            body.points = bodyShape;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.EnemyCurrentHp == 0)
        {
            animator.SetBool("isDead", true);
            body.enabled = false;
        }
 
    }

    public void Attack()
    {
        transform.GetChild(1).GetComponent<WizardAttack2>().Fire();
    }

}
