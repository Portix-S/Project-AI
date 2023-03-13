using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] bool isFacingLeft;
    [SerializeField] GameObject projectile;
    [SerializeField] int numberOfProjectiles; // 1-4
    Transform bossTransform;
    [SerializeField] GameObject projectileSpawnPos;
    [SerializeField] GameObject player;
    [SerializeField] GameObject dashCollider;
    Vector3 initialPos;
    Vector3 DashPosition;
    float playerPos;
    Animator animator;
    bool temVida;
    bool immune;
    int health;
    int rageHealth;

    // Start is called before the first frame update
    void Start()
    {
        isFacingLeft = true;
        bossTransform = gameObject.GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            LaunchProjectileAtPlayer();
        CheckIfNeedToFlip();
    }

    void CheckIfNeedToFlip()
    {
        float playerDirection = player.transform.position.x - transform.position.x;

        if(playerDirection > 0 && isFacingLeft || playerDirection < 0 && !isFacingLeft)
        {
            Flip();
        }
    }

    public void Flip()
    {
        isFacingLeft = !isFacingLeft;
        bossTransform.Rotate(0,180,0);
    }

    public void LaunchProjectileAtPlayer()
    {
        Instantiate(projectile, projectileSpawnPos.transform.position, Quaternion.Euler(0,0,0));
        
    }

    public void TryNewAttack()
    {
        int test = Random.Range(0, 3);
        Debug.Log(test);
        if (test != 0)
            animator.SetBool("isAttacking", true);

    }

    public void DashAttack()
    {
        if (transform.position == initialPos)
            transform.position = DashPosition;
        else
            transform.position = initialPos;
        // OU
        animator.SetBool("isDashing", true);
        // Player Dash with animation that gets him to the other side
        dashCollider.SetActive(true);    // Maybe(ProbYes) this can be activated with the animation also

        Flip(); 
	        // Do I have to FLip de animation for it to go to the other side? Or jsut by fliping the boss, the animation will
        //turn with him?

    }

    public void StopDashing()
    {
        animator.SetBool("isDashing", false);
        dashCollider.SetActive(false);
    }

    public void LaserAttack()
    {
        animator.SetBool("isLasering", true); // Talvez possa ser trigger tb
    }

    public void StopLaser()
    {
        animator.SetBool("isLasering", false);
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        ShowWinningUI();
    }

    public void DecideNextMove()
    {
        int rand = Random.Range(0, 3);
        if (health == 0)
            Die();
        else if (playerPos < 50) // <, não ==, pois tem q ser float
        {
            //MeleeAttack();
        }
        else if (rand == 0)
        {
            TryNewAttack();
        }
        else if (rand == 1)
        {
            LaserAttack();
        }
        else
        {
            animator.SetTrigger("isImmune");
            immune = true;
        }

    }

    public void TakeDamage(int damage)
    {
        if (temVida && !immune)
            health -= damage; // take in consideration armor? On Rage could have more?
        else if (!immune)
            health = 0;

        if (health <= rageHealth)
        {

        }
		    //Enter rage mode(); -> Just Make IdleState Different and Faster animations?
    }

    public void ShowWinningUI()
    {

    }
}
