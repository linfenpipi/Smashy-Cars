using UnityEngine;
using System.Collections;

public class ISSCBlock : MonoBehaviour
{

	public ISSCBlockVector size = new ISSCBlockVector (1, 1, 1);

	void OnCollisionEnter (Collision collision)
	{
		if (collision.collider.CompareTag ("ExpColliders")) {
//			Debug.Log(1);
			Vector3 hitPoint = collision.contacts [0].point;
			float rv = collision.relativeVelocity.magnitude;
			float hitRange = rv ;
			float expForce = rv * rv * collision.gameObject.GetComponent<Rigidbody> ().mass;
			Collider[] c = Physics.OverlapSphere (hitPoint, hitRange);
			for (int i = 0; i < c.Length; i++) {
//				Debug.Log(1);
//				Debug.Log(c.Length);
				if (!c [i].CompareTag ("BasicCubes"))
					continue;
//					Debug.Log(1);
				c [i].transform.parent = null;
				c [i].GetComponent<Rigidbody> ().isKinematic = false;
				c [i].GetComponent<Rigidbody> ().AddExplosionForce (expForce, hitPoint, hitRange);
			}
		}
	}

}
