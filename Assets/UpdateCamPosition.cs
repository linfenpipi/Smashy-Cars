using UnityEngine;
using System.Collections;

public class UpdateCamPosition : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = target.position;
	}
}
