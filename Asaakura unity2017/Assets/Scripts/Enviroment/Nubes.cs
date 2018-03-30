using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nubes : MonoBehaviour {

	public int velocidad = -10;
	public float limiteIzq = -70;
	public float limiteDer = 174;
	private Rigidbody2D rbnube;

	//public Vector2 velocity = new Vector2 (-4, 0);
	// Use this for initialization
	void Start () {
		rbnube = GetComponent<Rigidbody2D> ();

		rbnube.velocity = new Vector2 (velocidad, 0);

	}

	void Update (){
		/*if (GameManager.gameOver) {
			rbnube.velocity = new Vector2 (0, 0);

		}*/
		if (this.gameObject.transform.position.x <= limiteIzq) {
			this.gameObject.transform.position = new Vector3(limiteDer,this.gameObject.transform.position.y,this.gameObject.transform.position.z);
		}
	}
}
