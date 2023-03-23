using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rb;
    [SerializeField] bool isFacingRight;
    bool isTouchingTrigger;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (IsFacingRight())
            rb.velocity = new Vector2(moveSpeed, 0f);
        else
            rb.velocity = new Vector2(-moveSpeed, 0f);
        //*/

        if (isFacingRight)
            rb.velocity = new Vector2(moveSpeed, 0f);
        else
            rb.velocity = new Vector2(-moveSpeed, 0f);
    }

    private bool IsFacingRight()
    {
        return -transform.localScale.x > Mathf.Epsilon;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PatrolCollider" && !isTouchingTrigger)
        {
            isTouchingTrigger = true;
            StartCoroutine(DisableTrigger(collision.gameObject));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        /*
        if (collision.tag == "PatrolCollider")
        {
            //transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            StartCoroutine(DisableTrigger(collision.gameObject));
        }
        //*/
    }

    

    IEnumerator DisableTrigger(GameObject trigger)
    {
        trigger.SetActive(false);
        isFacingRight = !isFacingRight;
        yield return new WaitForSeconds(1.5f);
        isTouchingTrigger = false;
        trigger.SetActive(true);
    }
}
