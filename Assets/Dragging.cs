using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour {


	public AudioSource crash;
	public AudioSource shootSound;
	public float maxStretch = 3.0f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;
	public int counter=0;

	private SpringJoint2D spring;
	private bool clickedOn;
	private Rigidbody2D rigidBody; 
	private Ray rayToMouse;
	private Transform catapult;
	private float maxStrechSqr;
	private Vector2 preVelocity;
	private Ray leftCatapultToProjectile;
	private float circleRadius;
	private CircleCollider2D circle;


	void Awake(){
		spring = GetComponent<SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
		
	}
	// Use this for initialization
	void Start () {
		

		rigidBody =this.GetComponent<Rigidbody2D> ();
		LineRendererSetup ();
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray (catapultLineFront.transform.position, Vector3.zero);
		maxStrechSqr = maxStretch * maxStretch;
		CircleCollider2D circle = GetComponent<CircleCollider2D>();
		circleRadius = circle.radius;

	}

	// Update is called once per frame
	void Update () {
		

		if (clickedOn) {
			
			Drag ();

	

		}

		if (catapultLineFront.enabled == true) {
			LineRendererUpdate ();
		
		}


		if (spring.enabled==true) {
			if (!rigidBody.isKinematic && preVelocity.sqrMagnitude > rigidBody.velocity.sqrMagnitude) {
				counter++;
				spring.enabled = false;
				catapultLineFront.enabled = false;
				catapultLineBack.enabled = false;
				rigidBody.velocity = preVelocity;

			}

			if (!clickedOn)
				preVelocity = rigidBody.velocity;


		}
			
		
	}
	 
	public void LineRendererSetup (){
		catapultLineFront.SetPosition (0,catapultLineFront.transform.position);
		catapultLineBack.SetPosition (0,catapultLineBack.transform.position);

		catapultLineFront.sortingLayerName="Forground";
		catapultLineBack.sortingLayerName="Forground";

		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;
	}

	void OnMouseDown(){
		spring.enabled = false;
		clickedOn = true;
	

	}

	void OnMouseUp(){
		spring.enabled = true;
		rigidBody.isKinematic = false;
		clickedOn = false;
		shootSound.Play ();
	}

	void Drag(){
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		if (catapultToMouse.sqrMagnitude > maxStrechSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		
		}
		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;


	}

	void LineRendererUpdate(){
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint (catapultToProjectile.magnitude + circleRadius);
		catapultLineFront.SetPosition (1, holdPoint);
		catapultLineBack.SetPosition (1, holdPoint);
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.tag == "Damager") {
			crash.Play ();
		}
	}
}
