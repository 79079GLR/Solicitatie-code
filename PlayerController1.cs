using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController1 : MonoBehaviour {
	
	public float Health = 100f;
	public Text HealthText;

	public bool takingDMG;
	public bool stopDMG;
	public float Damage = 1f; 

	public float Timeleft = 3f;
	public float timer;

	public float Parts;
	public Text partsText;




	public GameObject Hit;
	public AudioClip hitSound;
	public float hitTimer = 2f;


	private AudioSource source;
	public float vol = 1f; 

	void Awake(){

		source = GetComponent<AudioSource> ();

	}

	void start(){

		SetHealthText ();

		Parts = 0f;

	}

	void Update () {

		if(PauseGame.GameIsPaused) {
		
			Hit.SetActive (false);
		}

		SetHealthText ();




		hitTimer -= Time.deltaTime;
		if (hitTimer <= 0) {
			Hit.SetActive (false);
			hitTimer = 2f;
		}

		if (takingDMG) {
			stopDMG = false;
			timer -= Time.deltaTime;

			Timeleft -= Time.deltaTime;

			if (Timeleft <= 0) {
				Timeleft = 1f;
				takingDMG = false;
				stopDMG = true;


			}
			if(timer <= 0){
				Hit.SetActive (true);
				
			Health -= Damage;
				timer = 0.5f;
				source.PlayOneShot (hitSound, vol);
			}

			SetHealthText ();
	
		}

		if(Health <= 0){

			SceneManager.LoadScene("gameover");

		}

		if (Parts >= 3) {
			SceneManager.LoadScene("gameover");
		}
	}

	void SetHealthText ()
	{
		HealthText.text =  "Health: " + Health.ToString();
		partsText.text =  "Parts: " + Parts.ToString();
	}
		

	void OnTriggerEnter(Collider other){

	
		if (other.tag == "enemy") {
		
			takingDMG = true;
			stopDMG = false;
		}

		if (other.tag == "parts") {
			Destroy (other.gameObject);
			Parts += 1;

		
		}

	}
	void OnTriggerExit (Collider other){

		if (other.tag == "enemy") {

			takingDMG = false;
			stopDMG = true;
		}


	}


}
