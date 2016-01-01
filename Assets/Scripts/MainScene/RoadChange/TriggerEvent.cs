using UnityEngine;
using System.Collections;

public class TriggerEvent : MonoBehaviour {
	public RoadChange father;
	void OnTriggerEnter(Collider other){
		if(other.CompareTag("PlayersCar"))
		father.changeLast();
	}
}
