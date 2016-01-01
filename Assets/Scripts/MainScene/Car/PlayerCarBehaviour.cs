using UnityEngine;
using System.Collections;

public class PlayerCarBehaviour : MonoBehaviour {

	Rigidbody rb;
	public float speed;
	Vector3 clamp;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		rb.MovePosition(transform.position-transform.right*speed*Time.deltaTime);
	}

	void FixedUpdate(){
//		clamp = transform.position;
//		clamp = new Vector3(clamp.x,Mathf.Clamp(clamp.y,0.5f,100f),Mathf.Clamp(clamp.z,-100f,100f));
//		transform.position = clamp;
	}
}
