using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// Para determinar en que sentido está viendo el jugador
	[HideInInspector]
	public bool jump = false;				// Conidción en la que el jugador puede saltar.
	[HideInInspector]
	public bool pickUpObject = false;		//Si el jugador está recogiendo un objeto

	public GameObject stone;
	public GameObject arrow;
	public GameObject spear;
	public float moveForce = 365f;			// Fuerza para desplazar al jugador horizontalmente.
	public float maxSpeed = 5f;				// Velocidad en que se desplaza el jugador.
	public AudioClip[] jumpClips;			// Sonidos para el salto del jugador.
	public float jumpForce = 1000f;			// Fuerza de salto.
	public AudioClip[] taunts;				// Sonidos aleatorios que puede emitir el jugador.
	public float tauntProbability = 50f;	// Probabilidad de reproducción de los sonidos.
	public float tauntDelay = 1f;			// Retardo cuando el sonido se deba reproducir.


	private int tauntIndex;					// Índice que indica qué sonido se reprodujo recientemente.
	private Transform groundCheck;			// Posición que indica dónde validar si el jugador está en el suelo.
	private bool grounded = false;			// Cuando el jugador esté o no en el suelo.
	private Animator anim;					// Referencia al componente animator del jugador.


	void Awake()
	{
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();

		//Reestablecer variables de juego (Solo usar en pruebas)
		GameManager.stonesCarried = 3;
		GameManager.arrowsCarried = 6;

	}


	void Update()
	{
		//El jugador esta sobre el suelo si un linecast hasta la posición de groundcheck toca cualquier elemento de la capa ground
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		anim.SetBool ("Grounded", grounded);

		//Si el botón de salto es presionado y el jugador está en el suelo, entonces el jugador salta
		if(Input.GetButtonDown("Jump") && grounded)
			jump = true;
		//Si el jugador presiona disparar entonces arroja una flecha
		if (Input.GetButtonDown ("Fire1")) {
			shoot ();
		}
		/*if (Input.GetKeyDown(KeyCode.DownArrow)) 
			pickUpObject = true;
		else
			pickUpObject = false;*/
	}

	void FixedUpdate ()
	{


		//Agacharse
		if (Input.GetKey (KeyCode.DownArrow)) {
			anim.SetBool ("crouch", true);
		} else {
			anim.SetBool ("crouch", false);

			//valor del input horizontal
			float h = Input.GetAxis ("Horizontal");

			//enviar valor de movimiento al animator
			anim.SetFloat ("Speed", Mathf.Abs (h));

			if (h != 0) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			
				//Si el jugador se mueve a la derecha y está de cara hacia la izquierda...
				if (h > 0 && !facingRight)
				// ... Voltear jugador.
				Flip ();
			//Si el jugador se mueve a la izquierda y está de cara a la derecha...
			else if (h < 0 && facingRight)
				// ... Voltear jugador.
				Flip ();
			}
		}
			
		// Si el jugador debe saltar...
		if(jump)
		{
			playerJump ();
		}
		if (!grounded)
			anim.SetFloat ("yvelocity", GetComponent<Rigidbody2D> ().velocity.y);
	}
	
	
	void Flip ()
	{
		//Cambiar el sentida hacia donde se dirige el jugador
		facingRight = !facingRight;

		//Multiplicar la escala local en x del jugador po -1
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

	void shoot(){
		float shotOrientation = 0f;
		float shotPosX =  0.18f;
		bool outAmmo = false;
		GameObject bullet = arrow;

		Debug.Log (GameManager.stonesCarried + " restantes ");

		if (GameManager.activeShots < GameManager.maxActiveShots){
			
			switch (GameManager.currentAmmo) {
			case "Stone":
				bullet = stone;
				if (GameManager.stonesCarried <= 0)
					outAmmo = true;
				else {
					GameManager.stonesCarried--;
					anim.SetTrigger ("Shoot");
					if (facingRight) {
						shotOrientation = 45f;
						shotPosX = 0.18f;
					} else {
						shotOrientation = 135f;
						shotPosX = shotPosX * -1;
					}	
				}
				break;
			case "Arrow":
				bullet = arrow;
				if (GameManager.arrowsCarried <= 0)
					outAmmo = true;
				else {
					GameManager.arrowsCarried--;
					anim.SetTrigger ("Arrow");
					if (facingRight) {
						shotOrientation = 0f;
						shotPosX = 0.18f;
					} else {
						shotOrientation = 180f;
						shotPosX = shotPosX * -1;
					}	
				}
				break;
			case "Spear":
				bullet = spear;
				if (GameManager.spearsCarried <= 0)
					outAmmo = true;
				else {
					GameManager.spearsCarried--;
					anim.SetTrigger ("Shoot");
					if (facingRight) {
						shotOrientation = 0f;
						shotPosX = 0.18f;
					} else {
						shotOrientation = 180f;
						shotPosX = shotPosX * -1;
					}	
				}
				break;
			}

			if(!outAmmo){
		
				Instantiate (bullet, new Vector2 (transform.position.x + shotPosX, transform.position.y + 0.22f),Quaternion.Euler(0,0,shotOrientation));//, Quaternion.Euler(0,0,ballOrientation));
			}
			//else indicar que no hay munición
		}
	}

	void playerJump() {
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


}
