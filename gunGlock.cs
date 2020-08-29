using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class gunGlock : MonoBehaviour {

	public float damage = 10f;
	public float range 	= 100f;
	public float fireRate = 15f;
	public float Force 	= 30f;
	public float ReloadTime = 3;
	public int MaxAmmo = 14;
	public int CurrentAmmo;
	private bool isReloading = false;

	public Text countText;

	public Camera fpsCam;
	public ParticleSystem muzzleFlash;
	public AudioClip ShootSound;
	public GameObject impactEffect;

	public Animator animator;

	private AudioSource source;
	public float vol = 1f; 

	public AudioClip ReloadSound;

	public SettingMenu VolumeSetting;
	private float nextTimeToFire = 0f;

	public Image test;



	void Awake(){

		source = GetComponent<AudioSource> ();

	}
	void start(){

		CurrentAmmo = MaxAmmo;
		SetCountText ();

	}

	void OnEnable(){
	
		isReloading = false;
		animator.SetBool ("Reloading", false);
	}

	// Update is called once per frame
	void Update () {
		SetCountText ();
		if (isReloading) {
			return;}

		if (CurrentAmmo <= 0f || Input.GetKeyDown(KeyCode.R)) {
		
			StartCoroutine (reload());
			return;
				
		}


		if (Input.GetButton ("Fire1") && Time.time >= nextTimeToFire) {

			nextTimeToFire = Time.time + 1f / fireRate;
			shoot ();
			SetCountText ();
		}

	}

	void SetCountText ()
	{
		countText.text = CurrentAmmo.ToString() + "/" + MaxAmmo.ToString();
	}



	IEnumerator reload(){
		SetCountText ();
		isReloading = true;

		GetComponent<AudioSource>().clip = ReloadSound;
		GetComponent<AudioSource>().Play ();

		Debug.Log ("Reload");

		animator.SetBool("Reloading" , true);

		yield return new WaitForSeconds (ReloadTime - 0.25f);

		CurrentAmmo = MaxAmmo;

		animator.SetBool ("Reloading", false);
		yield return new WaitForSeconds (0.25f);

		isReloading = false;
		Debug.Log ("done");

	}

	void shoot(){
		SetCountText ();
		source.PlayOneShot (ShootSound, vol);

		//pause game sound 
		if (PauseGame.GameIsPaused) {
			
			source.pitch = 0f;
		} else {
			source.pitch = 1f;

			//play partical effect
			muzzleFlash.Play();

			CurrentAmmo--;

			RaycastHit hit;

			if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){

				Debug.Log ("objext hit: " + hit.transform.name);

				Target target = hit.transform.GetComponent<Target> ();

				if (target != null) {

					target.TakeDamage(damage);

				}

				if(hit.rigidbody != null){

					hit.rigidbody.AddForce (-hit.normal * Force);
				}

				GameObject impactGo =Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
				Destroy (impactGo, 2f);
			}
		}

	}

}
