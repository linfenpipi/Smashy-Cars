using UnityEngine;
using System.Collections;

public class RoadChange : MonoBehaviour {

	public GameObject lastArea;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeLast(){
		Vector3 v3 = new Vector3(-200,0,0);
		Transform ts = lastArea.transform;
		ts.position = transform.position + v3;
	}
}
