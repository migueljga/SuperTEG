﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public GameObject flecha;
	public int maxFlechas = 3;
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.


	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.


	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		anim.SetBool ("Grounded", grounded);

		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && grounded)
			jump = true;
		if (Input.GetButtonDown ("Fire1")) {
			lanzarFlecha ();
		}
	}


	void FixedUpdate ()
	{


		//Agacharse
		if (Input.GetKey (KeyCode.DownArrow)) {
			anim.SetBool ("crouch", true);
		} else {
			anim.SetBool ("crouch", false);
			// Cache the horizontal input.
			float h = Input.GetAxis ("Horizontal");

			anim.SetFloat ("Speed", Mathf.Abs (h));

			if (h != 0) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			
				// If the input is moving the player right and the player is facing left...
				if (h > 0 && !facingRight)
				// ... flip the player.
				Flip ();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (h < 0 && facingRight)
				// ... flip the player.
				Flip ();
			}
		}
			
		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");



			// Play a random jump audio clip.
			int i = Random.Range(0, jumpClips.Length);
			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
		if (!grounded)
			anim.SetFloat ("yvelocity", GetComponent<Rigidbody2D> ().velocity.y);
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);

			// If there is no clip currently playing.
			if(!GetComponent<AudioSource>().isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();

				// Play the new taunt.
				GetComponent<AudioSource>().clip = taunts[tauntIndex];
				GetComponent<AudioSource>().Play();
			}
		}
	}


	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}

	void lanzarFlecha(){
		float flechaOrientacion;
		float flechaPosX =  0.18f;
		if (GameManager.flechasActivas < maxFlechas){
			
			anim.SetTrigger ("Arrow");
		//Detener el caminar del personaje
		//GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y); //No funciona

		//Instanciar prefab para arrojar flecha
		//if (!GameManager.BallEnabled && !GameManager.NoBall) {
			//GameManager.BallEnabled = true;
			if (facingRight) {
				flechaOrientacion = 0f;
				flechaPosX = 0.18f;
			} else {
				flechaOrientacion = 180f;
				flechaPosX = flechaPosX * -1;
			}			
			Instantiate (flecha, new Vector2 (transform.position.x + flechaPosX, transform.position.y + 0.22f),Quaternion.Euler(0,0,flechaOrientacion));//, Quaternion.Euler(0,0,ballOrientation));
			GameManager.flechasActivas++;
		}
		//}
	}
}
