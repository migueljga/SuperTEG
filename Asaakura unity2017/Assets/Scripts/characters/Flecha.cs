using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour {

	public float speed=100;
	Rigidbody2D flecha;
	//public GameObject audio;

	// Use this for initialization
	void Start () {
		//Instantiate(audio, this.gameObject.transform.position, Quaternion.identity); //Sonido de lanzamiento
		flecha = GetComponent <Rigidbody2D> ();
		flecha.AddForce(transform.right * speed);
	}


	void OnCollisionEnter2D(Collision2D collision) 
	{
		//Verificar si ya hay una flecha lanzada
		//GameManager.BallEnabled = false; //Pelota colisiona con otro objeto
		GameManager.flechasActivas--;
		Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Outscreen") //Pelota fuera de pantalla
		{
			//GameManager.BallEnabled = false;
			GameManager.flechasActivas--;
			Destroy(this.gameObject);
		}

	}
}
