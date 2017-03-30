using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetDamage : MonoBehaviour {

	public AudioSource BirdDead;
	public int hitPoints = 1;
	public Sprite damagedSprite;
	public float damageImpactSpeed;
	public ParticleSystem particle;
	public Text PassMessage;


	private SpriteRenderer spriteRenderer;
	private int currentHitPoints;
	private float damageImpactSpeedSqr;


	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		currentHitPoints = hitPoints;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
		PassMessage.gameObject.SetActive (false);


	}
	



	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.tag != "Damager")
			return;


		spriteRenderer.sprite = damagedSprite;
		currentHitPoints--;

		if (currentHitPoints <= 1){
			Kill ();
			Invoke ("NextLevel",5);
		}
	}

	void Kill(){
		spriteRenderer.enabled = false;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Rigidbody2D>().isKinematic = true;
		particle.transform.position = this.transform.position;
		particle.gameObject.SetActive (true);
		BirdDead.Play ();
		PassMessage.gameObject.SetActive (true);
	}

	public void NextLevel(){
		SceneManager.LoadScene ("SecondScene", LoadSceneMode.Single);
	
	}


}
