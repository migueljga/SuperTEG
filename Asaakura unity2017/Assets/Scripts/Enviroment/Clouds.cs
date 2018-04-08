using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

	public int velocity = -1; // Velocidad de desplazamiento de las nubes. Negativo para desplazar haci la izquierda
	public float leftBorder= -70; //Posición máxima del desplazamiento
	public float rightBorder = 174; //Posición de partida
	private Rigidbody2D rbnube;


	void Start () {
		rbnube = GetComponent<Rigidbody2D> ();
		rbnube.velocity = new Vector2 (velocity, 0);

	}

	void Update (){

		//Cuando el objeto nube alcance la posición máxmima es transportada al punto de partida
		if (this.gameObject.transform.position.x <= leftBorder) {
			this.gameObject.transform.position = new Vector3(rightBorder,this.gameObject.transform.position.y,this.gameObject.transform.position.z);
		}
	}
}
