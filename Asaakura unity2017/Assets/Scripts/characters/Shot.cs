using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

	public float speed=100;
	Rigidbody2D shot;
	//public GameObject audio;
	private string munition;
	private bool picked; //validar que el objeto fue recogido una sola vez

	// Use this for initialization
	void Start () {
		//Instantiate(audio, this.gameObject.transform.position, Quaternion.identity); //Sonido de lanzamiento
		shot = GetComponent <Rigidbody2D> ();
		shot.AddForce(transform.right * speed); //Impulsar el proyectil
		munition = this.gameObject.tag; //Determinar el tipo de proyectil que fue arrojado
		GameManager.activeShots++; //Indicar el número de proyectiles activos en pantalla
		picked = false; //Cuando el objeto es invocado no está en posesión del jugador

	}


	void OnCollisionEnter2D(Collision2D collision) 
	{
		GameManager.activeShots--;

		//Si el objeto es una piedra no es destruido al caer en la capa suelo
		if(munition != "Stone") 
			Destroy(this.gameObject);
		else
			if(collision.gameObject.layer != 12 )
				Destroy(this.gameObject);
		
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Outscreen") // Destruir el objeto si es arrojado fuera de pantalla
		{

			GameManager.activeShots--;
			Destroy(this.gameObject);
		}


	}

	void OnTriggerStay2D(Collider2D collision) {
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			Debug.Log ("Recoger");
			if (collision.gameObject.tag == "Player") {
				switch (this.gameObject.tag) {
				case "Stone":
					if (GameManager.stonesCarried < GameManager.maxStonesCarried && !picked) {
						picked = true;
						GameManager.stonesCarried++;
						Destroy (this.gameObject);
	
					}
					//else indicar que no puede recoger el objeto
					break;

				}
			}

		}


	}
}
