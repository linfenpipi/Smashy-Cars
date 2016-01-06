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
	public int hp = 500;
	bool endPlayed = false;

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
			rb.drag = 0.12f;
			playerControl ();
		}else{
			rb.drag = 5;
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
		if(endPlayed){
		return;
		}
		endPlayed = true;
		gc.endedGame = true;
		gc.gaming = false;
//		GameObject[] cbs = gc.ccs.allExist;
		for (int i = 0; i < gc.ccs.allExist.Length; i++) {
			gc.ccs.allExist[i].tag = "Untagged";
			gc.ccs.allExist[i].GetComponent<Collider>().isTrigger = false;
			Rigidbody rb;
			if(gc.ccs.allExist[i].GetComponent<Rigidbody>()!=null){
				rb = gc.ccs.allExist[i].GetComponent<Rigidbody>();
			}else{
				rb = gc.ccs.allExist [i].gameObject.AddComponent<Rigidbody> ();
			}
			rb.drag = 0.12f;
			rb.AddExplosionForce (20, transform.position, 5);
		}
	}

}
