using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostKingAttack : MonoBehaviour
{

    public GameObject projectile;
    [SerializeField]
    private int attackCD = 100;
    private Transform projectileSpawn;
    private bool attacked = false;
    private int launchedProj = 0;
    private Animator animator;
    private int attackRemainingTime;
    private bool finished = false;
    private GhostKingMovement gkMovement;
    private bool phase2JustTriggered = true;
    private GhostKingStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<GhostKingStats>();
        projectileSpawn = transform.GetChild(1).transform;
        gkMovement = GetComponent<GhostKingMovement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.EnemyCurrentHp == 0)
            this.enabled = false;

        // Phase 2: move to an edge of aggro zone and shoot 10 projectiles
        if (!stats.Phase1 && !phase2JustTriggered)
        {

            if (!attacked && launchedProj < 10)
            {
                animator.SetTrigger("attackRanged");
                attacked = true;
            }

            if (launchedProj == 10 && !finished)
            {
                attackRemainingTime = attackCD * 4;
                finished = true;
            }

            if (finished && attackRemainingTime == 0)
            {
                attackRemainingTime = attackCD;
                launchedProj = 0;
                finished = false;
                gkMovement.Teleport(false);
            }


        }
        else if (!stats.Phase1 && phase2JustTriggered) 
        // Phase 2 trigger (prepare to shoot projectile and teleport to opposite side of the room
        {
            attacked = true;
            attackCD = 45;
            attackRemainingTime = attackCD;
            phase2JustTriggered = false;
            gkMovement.Teleport(true);
        }

        if (attacked && attackRemainingTime > 0)
        {
            attackRemainingTime--;
        }
        else if (attackRemainingTime == 0)
        {
            attacked = false;
            attackRemainingTime = attackCD;
        }

    }


    void AttackRanged()
    {
        Vector3 spawn = projectileSpawn.position;

        // Adjusting spawn position of fireball depending on looking side
        if (stats.lookingLeft)
            spawn += Vector3.right * -projectileSpawn.localPosition.x * 2;
        GameObject ball = Instantiate(projectile, spawn, Quaternion.identity);
        Fireball2 fire = ball.GetComponent<Fireball2>();
        fire.destroyDistance = stats.BorderX * 1.5f;
        if (!stats.lookingLeft)
            fire.Launch(Vector2.right);
        else
            fire.Launch(Vector2.left);

        attacked = true;
        launchedProj++;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !attacked && stats.Phase1)
        {
            animator.SetTrigger("attack");
            attacked = true;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !attacked && stats.Phase1)
        {
            animator.SetTrigger("attack");
            attacked = true;
        }
    }

}
