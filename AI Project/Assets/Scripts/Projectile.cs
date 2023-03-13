using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] Rigidbody2D projectileRb;
    float timer = 5f;
    Vector3 distance;
    Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        projectileRb = gameObject.GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        transform.rotation = LookAtTarget(playerPos - transform.position);
        distance = playerPos - transform.position;
        projectileRb.velocity = new Vector2(distance.x, distance.y).normalized * projectileSpeed;
        Invoke("DestroyObject", timer);

    }



    static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Invoke("DestroyObject", 0.1f);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

}
