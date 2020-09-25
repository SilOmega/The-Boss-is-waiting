using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack2 : MonoBehaviour
{
    public FloatReference playerAttackDamage;

    private Animator animator;
    private CapsuleCollider2D attackHitBox;
    private Vector2 attackHitBoxOffset;
    private PlayerMovement pMovement;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        attackHitBox = GetComponent<CapsuleCollider2D>();
        attackHitBoxOffset = attackHitBox.offset;
        pMovement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for attack input
        if (Input.GetButtonDown("Fire1") && pMovement.Grounded)
        {
            attackHitBox.enabled = true;
            Attack();
        }
        else
            attackHitBox.enabled = false;

    }

    public void Attack()
    {
        animator.SetTrigger("attack");

        // Hitbox change side depengin on where player is facing
        if (!pMovement.LookingLeft)
            attackHitBox.offset = attackHitBoxOffset;
        else
            attackHitBox.offset = new Vector2(-attackHitBoxOffset.x, attackHitBoxOffset.y);

    }

    // When attack collider hit an enemy then apply the damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyTakeDamage>() != null)
            collision.gameObject.GetComponent<EnemyTakeDamage>().TakeDamage(playerAttackDamage.Value);
    }

}
