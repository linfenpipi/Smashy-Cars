using UnityEngine;
using System.Collections;

public class ISSCBlock : MonoBehaviour
{

	public ISSCBlockVector size = new ISSCBlockVector (1, 1, 1);

	public bool droped = false;
	bool destroyed = false;
	bool b = true;

	void OnCollisionEnter (Collision collision)
	{
		if (collision.collider.CompareTag ("ExpColliders") && collision != null && !droped) {
//			Debug.Log(1);
			Vector3 hitPoint = collision.contacts [0].point;
			float rv = collision.relativeVelocity.magnitude;
			float hitRange = rv /16;
			Collider[] c = Physics.OverlapSphere (hitPoint, hitRange);
			float expForce = rv * rv * collision.collider.GetComponent<Rigidbody> ().mass / c.Length;
			for (int i = 0; i < c.Length; i++) {
//				Debug.Log(1);
//				Debug.Log(c.Length);
				if (!c [i].CompareTag ("BasicCubes"))
					continue;
//					Debug.Log(1);
				c [i].transform.parent = null;
				c [i].gameObject.tag = "Untagged";
				c [i].GetComponent<Rigidbody> ().isKinematic = false;
				c [i].GetComponent<Rigidbody> ().AddExplosionForce (expForce, hitPoint, hitRange);
				c [i].GetComponent<ISSCBlock>().droped = true;
			}
		}
	}

	void Update(){
	if(droped&&!destroyed){
		transform.parent = null;
		gameObject.tag = "Untagged";
		GetComponent<Rigidbody>().isKinematic = false;
		destroyed = true;
	}

	if(destroyed&&b){
		StartCoroutine(DestroyThis());
		b = false;
	}

	}

	IEnumerator DestroyThis(){
	yield return new WaitForSeconds(5);
	Destroy(gameObject);
	yield return null;
	}

}
