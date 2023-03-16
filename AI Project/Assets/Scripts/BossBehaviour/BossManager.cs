using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Serializables")]
    public bool isFacingLeft;
    [SerializeField] GameObject projectile;
    public GameObject laser;
    public GameObject laserPos;
    [SerializeField] int numberOfProjectiles; // 1-4
    [SerializeField] GameObject projectileSpawnPos;
    [SerializeField] GameObject player;
    [SerializeField] GameObject dashCollider;

    HealthBar bossHealthUI;
    Animator animator;
    bool temVida;
    public bool immune;
    float health = 100;
    float rageHealth;
    Transform bossTransform;
    Rigidbody2D bossRb;

    [Header("Skills Config")]
    public GameObject[] laserList;
    [SerializeField] float throwForce;
    [SerializeField] float dashVelocity;
    Vector3 initialPos;
    Vector3 dashPos;
    float playerPos = 50;
    float meleeDashDamage = 10f;

    float deltaDash;
    float deltaImmune;
    float deltaLaser;


    [Header("Probability")]
    public float meleeChance = 50f;
    public float[] chanceList;

    // Start is called before the first frame update
    void Start()
    {
        isFacingLeft = true;
        bossTransform = gameObject.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        bossRb = GetComponent<Rigidbody2D>();
        bossHealthUI = GetComponent<HealthBar>();
        bossHealthUI.currentHealth = health;

        chanceList[0] = 40f; // Ranged Chance in %
        chanceList[1] = 20f; // Laser Chance 
        chanceList[2] = 20f; // Immune Chance
        chanceList[3] = 20f; // Dash Chance
        initialPos = new Vector3(transform.position.x, transform.position.y, 0f);
        dashPos = new Vector3(-transform.position.x, transform.position.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            LaunchProjectileAtPlayer();
        //CheckIfNeedToFlip();

        if (animator.GetBool("isDashing")) // Talvez Invoke
        {
            Dash();
        }
    }

    public void ChangeChance(int alvo, bool positive) // Altera as chances de usar cada habilidade
    {
        for (int i = 0; i < chanceList.Length; i++) // Diminui as chances de todas as habilidades
        {                                           //para aumentar apenas a chance de uma
            if (positive)
            {
                chanceList[i] -= 0.2f;
                if (i == alvo)
                    chanceList[i] += 0.8f;
            }
            else
            {
                chanceList[i] += 0.2f;
                if (i == alvo)
                    chanceList[i] -= 0.8f;
            }
        }
    }
    void Dash()
    {
        if (isFacingLeft)
        {
            if (transform.position.x > dashPos.x)
                bossRb.velocity = new Vector2(-transform.localScale.x * dashVelocity, 0f);
            else if (transform.position.x <= dashPos.x && isFacingLeft)
                StopDashing();
        }
        else
        {
            if (transform.position.x < initialPos.x)
                bossRb.velocity = new Vector2(transform.localScale.x * dashVelocity, 0f);
            else if(transform.position.x >= initialPos.x && !isFacingLeft)
                StopDashing();
        }
    }

    void CheckIfNeedToFlip() // Outdated
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
        int test = Random.Range(0, 100);
        if (test < chanceList[0] + 10)
            animator.SetBool("isAttacking", true);

    }

    public void DashAttack() // Faz o dash de um lado para o outro da tela
    {
        animator.SetBool("isDashing", true);
    }

    public void StopDashing() // Faz a m�quina de estados saber que o dash acabou
    {
        animator.SetBool("isDashing", false);
        //dashCollider.SetActive(false);
        bossRb.velocity = Vector2.zero;
        Flip();
    }

    public void StopImmune()
    {
        animator.SetBool("isImmune", false);
    }

    public void StopLaser() // Faz a m�quina de estados saber que o laser acabou
    {
        animator.SetBool("isLasering", false);
    }

    public void StopMelee()
    {
        animator.SetBool("isMeleeing", false);
    }

    private void Die()  // Boss morre, mostra tela de vit�ria
    {
        animator.SetBool("isDead", true);
        ShowWinningUI();
    }

    public void DecideNextMove()  // Decision "Tree"
    {
        float rand = Random.Range(0, 100);
        deltaLaser = chanceList[0] + chanceList[1];
        deltaImmune = deltaLaser + chanceList[2];
        deltaDash = deltaImmune + chanceList[3];

        playerPos = Vector3.Distance(transform.position, player.transform.position);

        if (health == 0)
            Die();
        else if (playerPos < 5f && Random.Range(0, 100) <= meleeChance) //playerPos n esta implementado
        {
            Debug.Log("Melee");
            animator.SetBool("isMeleeing", true);
        }
        else if (rand <= chanceList[0])
        {
            Debug.Log("Ranged");
            animator.SetBool("isAttacking", true);
            TryNewAttack();
        }
        else if (rand > chanceList[0] && rand <= deltaLaser)
        {
            Debug.Log("Laser");
            animator.SetBool("isLasering", true);
        }
        else if (rand > deltaLaser && rand <= deltaImmune)
        {
            Debug.Log("Immune");
            animator.SetBool("isImmune",true);
        }
        else if (rand > deltaImmune && rand <= deltaDash)
        {
            Debug.Log("Dash");
            animator.SetBool("isDashing", true);
            //DashAttack();
        }

    }

    public void ThrowPlayer() // Caso o boss acerte o player, joga o player pra longe
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (isFacingLeft)
        {
            //throw player left
            playerRb.AddForce(-player.transform.right * throwForce, ForceMode2D.Impulse);
        }
        else
        {
            //throw player right
            playerRb.AddForce(player.transform.right * throwForce, ForceMode2D.Impulse);
        }

    }

    public void TakeDamage(float damage) // Boss toma dano, caso n�o esteja imune
    {
        bossHealthUI.TakeDamage(damage);
        if (!immune && health > damage)
            health -= damage; // take in consideration armor? On Rage could have more?
        else if (!immune && health <= damage)
        {
            health = 0;
        }

        if (health <= rageHealth) //Enter rage mode(); -> Just Make IdleState Different and Faster animations?
        {

        }
		    
    }
    public void CheckMeleeMiss() // Se o melee n�o acerte ningu�m, diminui a chance de usar a skill
    {
        meleeChance--;
    }

    public void ShowWinningUI()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(animator.GetBool("isMeleeing") || animator.GetBool("isDashing"))
            {
                ThrowPlayer();
                player.GetComponent<CharacterController2D>().TakeDamage(meleeDashDamage);
            }
        }
    }
}
