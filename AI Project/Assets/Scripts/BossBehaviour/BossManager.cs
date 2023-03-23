using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Serializables")]
    public bool isFacingLeft;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject rageProjectile;
    public GameObject laser;
    public GameObject laserPos;
    [SerializeField] int numberOfProjectiles; // 1-4
    [SerializeField] GameObject projectileSpawnPos;
    [SerializeField] GameObject player;
    [SerializeField] GameObject dashCollider;
    [SerializeField] Animator laserAnimator;
    [SerializeField] GameObject winUI;

    HealthBar bossHealthUI;
    Animator animator;
    bool alive = true;
    public bool immune;
    float health = 500f;
    float rageHealth;
    Transform bossTransform;
    Rigidbody2D bossRb;
    SpriteRenderer bossSprite;
    bool isPlayerTakingDamage;

    [Header("Skills Config")]
    public GameObject[] laserList;
    [SerializeField] float throwForce;
    [SerializeField] float dashVelocity;
    Vector3 initialPos;
    Vector3 dashPos;
    float playerPos = 50;
    [SerializeField] float meleeDashDamage = 10f;
    [SerializeField] bool onRage;
  

    float deltaDash;
    float deltaImmune;
    float deltaLaser;


    [Header("Probability")]
    public float meleeChance = 50f;
    public float[] chanceList;
    float rand;

    // Start is called before the first frame update
    void Start()
    {
        isFacingLeft = true;
        bossTransform = gameObject.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        bossRb = GetComponent<Rigidbody2D>();
        bossHealthUI = GetComponent<HealthBar>();
        bossHealthUI.currentHealth = health; // arrumar depois
        bossHealthUI.UpdateHealth();
        rageHealth = health * 0.3f;
        bossSprite = GetComponent<SpriteRenderer>();


        chanceList[0] = 48f; // Ranged Chance in %
        chanceList[1] = 20f; // Laser Chance 
        chanceList[2] = 20f; // Immune Chance
        chanceList[3] = 20f; // Dash Chance
        initialPos = new Vector3(transform.position.x, transform.position.y, 0f);
        dashPos = new Vector3(-transform.position.x, transform.position.y, 0f);
    }

    public float GetCurrentHealth()
    {
        return bossHealthUI.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isDashing")) // Talvez Invoke
        {
            Dash();
        }
        if(bossHealthUI.currentHealth == 0f && alive)
        {
            alive = false;
            Invoke("Die", 0.2f);
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

        if (bossHealthUI.currentHealth == 0f)
        {
            StopDashing();
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
        if(!onRage)
            Instantiate(projectile, projectileSpawnPos.transform.position, Quaternion.Euler(0,0,0));
        else
            Instantiate(rageProjectile, projectileSpawnPos.transform.position, Quaternion.Euler(0, 0, 0));
    }

    public void TryNewAttack()
    {
        int test = Random.Range(0, 100);
        if (test < chanceList[0] + 10)
            animator.SetBool("isAttacking", true);
        else
            animator.SetBool("isAttacking", false);
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
        animator.SetBool("isDashing", false);
        animator.SetBool("isMeleeing", false);
        animator.SetBool("isLasering", false);
        animator.SetBool("isImmune", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
    }

    public void DecideNextMove()
    {
        rand = Random.Range(0, 100);
        deltaLaser = chanceList[0] + chanceList[1];
        deltaImmune = deltaLaser + chanceList[2];
        deltaDash = deltaImmune + chanceList[3];

        playerPos = Vector3.Distance(transform.position, player.transform.position);

        if (MeleeChance())
            animator.SetBool("isMeleeing", true);
        else if (RangedChance())
        {
            animator.SetBool("isAttacking", true);
            //TryNewAttack();
        }
        else if (DashChance())
            animator.SetBool("isDashing", true);
        else if (LaserChance())
            animator.SetBool("isLasering", true);
        else if (ImmuneChance())
            animator.SetBool("isImmune",true);
    }
    
    bool MeleeChance()
    {
        return playerPos < 5f && Random.Range(0, 100) <= meleeChance;
    }
    bool RangedChance()
    {
        return rand <= chanceList[0];
    }

    bool ImmuneChance()
    {
        return rand > deltaLaser && rand <= deltaImmune;
    }
    private bool LaserChance()
    {
        return rand > chanceList[0] && rand <= deltaLaser;
    }
    private bool DashChance()
    {
        if (player.transform.position.y > 1.5f)
            return (rand > deltaImmune -20 && rand <= deltaDash); // AUmenta em 20% a chance de Dash
        else
            return (rand > deltaImmune  && rand <= deltaDash );
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
        if (!immune && health > damage)
        {
            health -= damage; 
        }
        else if (!immune && health <= damage)
        {
            health = 0;
        }

        if (!immune)
        {
            if(!onRage)
                bossHealthUI.TakeDamage(damage);
            else
                bossHealthUI.TakeDamage(damage * 0.7f);
        }

        if (health <= rageHealth) //Enter rage mode(); -> Just Make IdleState Different and Faster animations?
        {
            onRage = true;
            animator.speed = 1.75f;
            laserAnimator = laser.GetComponent<Animator>();
            laserAnimator.speed = 1.75f;
            Color color;
            if (ColorUtility.TryParseHtmlString("#F6B6B6", out color))
                bossSprite.color = color;
        }
		    
    }
    public void CheckMeleeMiss() // Se o melee n�o acerte ningu�m, diminui a chance de usar a skill
    {
        meleeChance--;
    }

    public void ShowWinningUI()
    {
        winUI.SetActive(true);
    }

    IEnumerator PlayerDamageCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isPlayerTakingDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(animator.GetBool("isMeleeing") || animator.GetBool("isDashing") && !isPlayerTakingDamage)
            {
                isPlayerTakingDamage = true;
                StartCoroutine(PlayerDamageCoolDown());
                ThrowPlayer();
                player.GetComponent<CharacterController2D>().TakeDamage(meleeDashDamage);
            }
        }
    }
}
