using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float laserDamage = 1f;
    CharacterController2D playerScript;

    private void Awake() 
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            BossManager bossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossManager>();
            bossScript.ChangeChance(1, true); // Recompensa para o Boss
            playerScript.onLaser = true;
			StartCoroutine(playerScript.LaserDamage(laserDamage));
        }    
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player")
        {
            playerScript.onLaser = false;
        }
    }

    private void OnDestroy()
    {
        playerScript.onLaser = false;
    }

}
