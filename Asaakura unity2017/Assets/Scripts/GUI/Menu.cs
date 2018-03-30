using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	public GameObject panelMenu;
	public GameObject txtInicio;
	public GameObject btnContinuar;
	public GameObject panelOpciones;
	public GameObject panelCreditos;
	public GameObject panelConfirmacion;

	// Use this for initialization
	void Start () {
		
		//Ocultar menú y paneles
		panelMenu.SetActive(false);
		panelOpciones.SetActive(false);
		panelCreditos.SetActive(false);
		//Si Existen partidas guardadas se muestra el botón de cargar partida
		if (PlayerPrefs.GetInt ("JuegoGuardado",1) == 1) {
			btnContinuar.SetActive (true);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().buildIndex == 0 && !panelMenu.activeSelf) {
			if (Input.anyKey) {
				txtInicio.SetActive (false);
				panelMenu.SetActive (true);
			}
		}
	}
	public void BotonNuevaPartida(){
		//Comprobar si existen datos guardados
		if (PlayerPrefs.GetInt ("JuegoGuardado",1) == 1) {
			//Mensaje de confirmacion
			panelConfirmacion.SetActive(true);

		}

	}
	public void BontonConfirmarNuevaPartida(){
		//Borrar Datos Guardados
		
		//Cargar escena inicial (Mapa)
		SceneManager.LoadScene("Main");

	}
	
	public void BotonOpciones(){
			panelOpciones.SetActive(true);
	}
	
	public void BotonCreditos(){
			panelCreditos.SetActive(true);
	}
	
	public void BotonVolverCancelar(){
			panelOpciones.SetActive(false);
			panelCreditos.SetActive(false);
			panelConfirmacion.SetActive(false);
	}
	
	public void BotonSalir(){
			Application.Quit();;
	}
}
