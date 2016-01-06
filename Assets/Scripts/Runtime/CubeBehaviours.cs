using UnityEngine;
using System.Collections;

public class CubeBehaviours : MonoBehaviour
{

	public GameController runtimeCore;
	public GameObject massCore;
	public bool droped;
	public float catchRadiusParam;
	public float forceParam;

	// Use this for initialization
	void Start ()
	{
		droped = false;
		Destroy (GetComponent<Rigidbody> ());
		GetComponent<Collider> ().isTrigger = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("BasicCubes") || droped) {
			return;
		} else if (runtimeCore.gaming && other.CompareTag ("ExpColliders")) {
			float relateSpeed;
			float catchRadius;
			float force;

			relateSpeed = massCore.GetComponent<Rigidbody> ().velocity.magnitude;
			catchRadius = relateSpeed * catchRadiusParam;
			force = relateSpeed * forceParam;

			Collider[] cols = Physics.OverlapSphere (transform.position, catchRadius);
			for (int i = 0; i < cols.Length; i++) {
				if (cols [i].CompareTag ("BasicCubes")) {
					cols [i].GetComponent<CubeBehaviours> ().droped = true;
					cols [i].transform.parent = null;
					cols [i].isTrigger = false;
					if (cols [i].GetComponent<Rigidbody> () == null) {
						Rigidbody rb = cols [i].gameObject.AddComponent<Rigidbody> ();
					}
//					rb.AddExplosionForce (force, transform.position, catchRadius);
					cols[i].GetComponent<Rigidbody>().AddExplosionForce (force, transform.position, catchRadius);
				}
			}
		}
	}
}
