using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    PlayerMovement playerMov;
    Rigidbody2D bulletRb;

    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        playerMov = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        bulletRb = GetComponent<Rigidbody2D>();
        //Maybe change to always go right? Check where to rotate object
        if(playerMov.isFacingUp)
            bulletRb.velocity = new Vector2(0,1) * bulletSpeed;
        //*
        else if(playerMov.isFacingLeft)
        {
            bulletRb.velocity = new Vector2(-1,0) * bulletSpeed;
            //arrumar rotação?
        }
        else if(!playerMov.isFacingLeft)
            bulletRb.velocity = new Vector2(1,0) * bulletSpeed;
        //*/
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "Boss")
        {
            BossManager bossScript = other.collider.GetComponent<BossManager>();
            bossScript.TakeDamage(bulletDamage);
            Destroy(gameObject, 0f);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }

}
