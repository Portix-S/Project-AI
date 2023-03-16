using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float laserDamage = 1f;
    CharacterController2D playerScript;

    private void Awake() 
    {
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            playerScript = other.GetComponent<CharacterController2D>();
            BossManager bossScript = GetComponent<BossManager>();
            bossScript.ChangeChance(1, true);
            playerScript.onLaser = true;
			StartCoroutine(playerScript.LaserDamage(laserDamage)); // Em teoria faz ele tomar dano a cada 0.1seg
        }    
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player")
        {
            playerScript = other.GetComponent<CharacterController2D>();
            playerScript.onLaser = false;
        }
    }

}
