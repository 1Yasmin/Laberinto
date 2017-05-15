using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{
	

	void Start(){
		transform.Translate (-4.32f, 0.5f, -4.94f);
		
	}

	void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

	}

	void OnCollisionEnter(Collision col)
	{
		// Destruir los bloques
		if (col.gameObject.name.Contains ("Cube")) {
			Destroy (col.gameObject); // destruye los bolques
			SceneManager.LoadScene(1); // carga escena de juego ganado
		
		}

	}
		

}