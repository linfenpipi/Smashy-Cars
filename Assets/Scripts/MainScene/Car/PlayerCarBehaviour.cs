using UnityEngine;
using System.Collections;

public class PlayerCarBehaviour : MonoBehaviour
{

	Rigidbody rb;
	public float speed;
	float force;
	public float horizonSpeed;
	float horizonAxis;
	Vector3 clamp;
	public GameController gc;
	public int hp = 1000;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(hp <= 0){
		GameEnd();
		}
		if (gc.gaming) {
			playerControl ();
		}else{
			transform.position = transform.position;
		}
	}

	void playerControl ()
	{	
		force = speed / rb.velocity.magnitude;
		horizonAxis = Input.GetAxis ("Horizontal");
		rb.AddForce (-transform.right * force, ForceMode.Force);
		rb.AddForce (transform.forward * horizonAxis * horizonSpeed, ForceMode.Force);
	}

	void OnCollisionEnter (Collision other)
	{
		if(!gc.gaming){
		return;
		}
		if (other.collider.CompareTag ("ExpColliders")) {
			GameEnd ();
		}
	}

	void GameEnd ()
	{
		gc.gaming = false;
		CubeBehaviours[] cbs = GetComponentsInChildren<CubeBehaviours> ();
		for (int i = 0; i < cbs.Length; i++) {
			cbs[i].GetComponent<Collider>().isTrigger = false;
			Rigidbody rb;
			if(cbs[i].GetComponent<Rigidbody>()!=null){
				rb = cbs[i].GetComponent<Rigidbody>();
			}else{
				rb = cbs [i].gameObject.AddComponent<Rigidbody> ();
			}
			rb.drag = 0.12f;
			rb.AddExplosionForce (5, transform.position, 5);
		}
	}

}
