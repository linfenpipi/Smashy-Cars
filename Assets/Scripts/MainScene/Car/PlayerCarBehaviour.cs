using UnityEngine;
using System.Collections;

public class PlayerCarBehaviour : MonoBehaviour
{

	Rigidbody rb;
	public float speed;
	float force;
	public float horizonSpeed;
	float horizonForce;
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
	void FixedUpdate ()
	{
		if (hp <= 0) {
			GameEnd ();
		}
		if (gc.gaming) {
			rb.drag = 0.12f;
			playerControl ();
		} else {
			rb.drag = 10;
		}
	}

	void playerControl ()
	{	
		horizonAxis = Input.GetAxis ("Horizontal");
		force = speed / Mathf.Abs (Vector3.Dot (rb.velocity, -transform.right) + 1);
		rb.AddForce (-transform.right * force, ForceMode.Force);

		float f = 0;
		if (horizonAxis == 0) {
		} else {
			f = Vector3.Dot (rb.velocity, (transform.forward * horizonAxis).normalized);
		}
		if (f <= 0) {
			horizonForce = horizonSpeed;
		} else {
			horizonForce = horizonSpeed / Mathf.Abs (f+1);
		}
		if (Vector3.Dot (rb.velocity, transform.forward) < 0) {
			rb.AddForce (transform.forward * Mathf.Abs (f * 2000), ForceMode.Force);
		} else if(Vector3.Dot (rb.velocity, transform.forward) > 0){
			rb.AddForce (-transform.forward * Mathf.Abs (f * 2000), ForceMode.Force);
		}
		rb.AddForce (transform.forward * horizonAxis * horizonForce, ForceMode.Force);

	}

	void OnCollisionEnter (Collision other)
	{
		if (!gc.gaming) {
			return;
		}
		if (other.collider.CompareTag ("ExpColliders")) {
			GameEnd ();
		}
	}

	void GameEnd ()
	{	
		if (endPlayed) {
			return;
		}
		endPlayed = true;
		gc.endedGame = true;
		gc.gaming = false;
//		GameObject[] cbs = gc.ccs.allExist;
		for (int i = 0; i < gc.ccs.allExist.Length; i++) {
			gc.ccs.allExist [i].tag = "Untagged";
			gc.ccs.allExist [i].GetComponent<Collider> ().isTrigger = false;
			Rigidbody rb;
			if (gc.ccs.allExist [i].GetComponent<Rigidbody> () != null) {
				rb = gc.ccs.allExist [i].GetComponent<Rigidbody> ();
			} else {
				rb = gc.ccs.allExist [i].gameObject.AddComponent<Rigidbody> ();
			}
			rb.isKinematic = false;
			rb.drag = 0.12f;
//			rb.AddExplosionForce (20, transform.position, 5);
		}
	}

}
