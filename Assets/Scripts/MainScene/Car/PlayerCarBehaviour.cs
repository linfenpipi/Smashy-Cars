using UnityEngine;
using System.Collections;

public class PlayerCarBehaviour : MonoBehaviour {

	Rigidbody rb;
	public float speed;
	public float horizonSpeed;
	float horizonAxis;
	Vector3 clamp;
	public GameController gc;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		rb.AddForce(-transform.right*speed,ForceMode.Force);
		if(gc.gaming){
			playerControl();
		}
	}

	void playerControl(){
//		if(Input.GetButton("Horizontal")){
			horizonAxis = Input.GetAxis("Horizontal");
//		}
//		Debug.Log(transform.right*horizonAxis*horizonSpeed);
			rb.AddForce(transform.forward*horizonAxis*horizonSpeed,ForceMode.Force);
	}

	void FixedUpdate(){
//		clamp = transform.position;
//		clamp = new Vector3(clamp.x,Mathf.Clamp(clamp.y,0.5f,100f),Mathf.Clamp(clamp.z,-100f,100f));
//		transform.position = clamp;
	}
}
