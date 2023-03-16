using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] Rigidbody2D projectileRb;
    [SerializeField] float projectileDamage;
    Vector3 distance;
    Vector3 playerPos;
    bool hitPlayer;
    BossManager bossScript;
    
    // Start is called before the first frame update
    void Start()
    {
        projectileRb = gameObject.GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        transform.rotation = LookAtTarget(playerPos - transform.position);
        distance = playerPos - transform.position;
        projectileRb.velocity = new Vector2(distance.x, distance.y).normalized * projectileSpeed;
        Destroy(gameObject, 5f);
        bossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossManager>();
    }



    static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.isTrigger)
        {
            Destroy(gameObject, 0.1f);
            hitPlayer = true;
            CharacterController2D playerScript = collision.GetComponent<CharacterController2D>();
            playerScript.TakeDamage(projectileDamage);
        }
    }

    private void OnDestroy()
    {
        if (!hitPlayer)
            bossScript.ChangeChance(0, false);
        else
            bossScript.ChangeChance(0, true);
    }

}
