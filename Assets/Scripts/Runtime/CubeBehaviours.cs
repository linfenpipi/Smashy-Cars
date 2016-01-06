using UnityEngine;
using System.Collections;

public class CubeBehaviours : MonoBehaviour
{

	public GameController runtimeCore;
	public GameObject massCore;
	public bool droped = false;
	bool hpDecreased = false;
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
		if (droped && !hpDecreased) {
			hpDecreased = true;
			massCore.GetComponent<PlayerCarBehaviour> ().hp--;
		}
		
	}

	void OnTriggerEnter (Collider other)
	{

		if (other.CompareTag ("BasicCubes") || droped) {
			return;
		} else if (runtimeCore.gaming && other.CompareTag ("ExpColliders")) {

			if (runtimeCore.supported != null && !runtimeCore.supported.Equals (this)) {
				return;
			} else {
				Debug.Log(2);
				StartCoroutine(test());

			}




		}
	}

	IEnumerator test ()
	{
		Debug.Log(1);
		runtimeCore.supported = this;

//		yield return null;

		float relateSpeed;
		float catchRadius;
		float force;

		relateSpeed = massCore.GetComponent<Rigidbody> ().velocity.magnitude;
		catchRadius = relateSpeed * catchRadiusParam;
		force = relateSpeed * forceParam;

		massCore.GetComponent<Rigidbody> ().AddForce (relateSpeed * 200 * (massCore.transform.position - transform.position).normalized, ForceMode.Impulse);

//		Collider[] cols = Physics.OverlapSphere (transform.position, catchRadius);
		Collider[] cols = runtimeCore.ccs.getSphere(gameObject,catchRadius);
		for (int i = 0; i < cols.Length; i++) {
			if (cols [i].CompareTag ("BasicCubes")) {
				cols [i].tag = "Untagged";
				cols [i].GetComponent<CubeBehaviours> ().droped = true;
//					cols [i].transform.parent = null;
				cols [i].isTrigger = false;
				Rigidbody rb;
				if (cols [i].GetComponent<Rigidbody> () == null) {
					rb = cols [i].gameObject.AddComponent<Rigidbody> ();
				} else {
					rb = cols [i].GetComponent<Rigidbody> ();
				}
				rb.drag = 0.12f;
				rb.AddExplosionForce (force, transform.position, catchRadius);
			}
		}

		yield return null;

		runtimeCore.supported = null;

		yield return null;
	}
}
