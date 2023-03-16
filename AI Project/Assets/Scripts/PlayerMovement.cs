using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	[SerializeField] GameObject bullet;
	[SerializeField] GameObject bulletPosHorizontal;
	[SerializeField] GameObject bulletPosUp;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	public bool isFacingLeft;
	public bool isFacingUp;
	bool jump = false;
	bool crouch = false;
	Vector2 limits;
	float objectWidth;
	float objectHeight;
	Animator animator;
	Rigidbody2D rb;

	//Retirar Depois
	BossManager bossScript;

	private void Start()
    {
		 limits = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
		objectWidth = transform.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2 - 0.55f;
		objectHeight = transform.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2 - 0.55f;
		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		bossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossManager>();
	}

    // Update is called once per frame
    void Update () 
	{
		if(Input.GetKeyDown(KeyCode.W))
			isFacingUp = true;
		else if(horizontalMove < 0)
		{
			isFacingLeft = true;
			isFacingUp = false;
		}
		else if(horizontalMove > 0)
		{
			isFacingLeft = false;
			isFacingUp = false;
		}


		if (controller.health > 0)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				animator.SetBool("isGrounded", false);
				jump = true;
				animator.SetBool("isJumping", true);
			}


			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

			if (Input.GetButtonDown("Crouch"))
			{
				animator.SetBool("isCrouching", true);
				crouch = true;
			}
			else if (Input.GetButtonUp("Crouch"))
			{
				crouch = false;
				animator.SetBool("isCrouching", false);
			}

			if (Input.GetKeyDown(KeyCode.P))
			{
				if(isFacingUp)
					Instantiate(bullet, bulletPosUp.transform.position, Quaternion.Euler(0,0,-90f));
				else if(isFacingLeft)
					Instantiate(bullet, bulletPosHorizontal.transform.position, Quaternion.Euler(0,0,-90f));
				else if(!isFacingLeft)
					Instantiate(bullet, bulletPosHorizontal.transform.position, Quaternion.Euler(0,0,-90f));

			}

		}
		else
			horizontalMove = 0;

		if (horizontalMove != 0)
			animator.SetBool("isRunning", true);
		else
			animator.SetBool("isRunning", false);

		if (rb.velocity.y < -Mathf.Epsilon && !controller.m_Grounded)
		{
			animator.SetBool("isJumping", false);
			animator.SetBool("isFalling", true);
		}

		if (controller.m_Grounded)
		{
			animator.SetBool("isFalling", false);
			animator.SetBool("isGrounded", true);
		}
		else
		{
			animator.SetBool("isGrounded", false);
		}

	}

	void FixedUpdate ()
	{
		// Move our character
		if (controller.health > 0)
		{
			controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
			jump = false;
		}
	}

	private void LateUpdate()
	{
		// Mant�m o personagem nos limites da telas 
		Vector3 objPos = transform.position;
		objPos.x = Mathf.Clamp(objPos.x, -limits.x + objectWidth, limits.x - objectWidth );
		objPos.y = Mathf.Clamp(objPos.y, -limits.y + objectHeight , limits.y - objectHeight);
		transform.position = objPos;
	}
}
