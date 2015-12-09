using UnityEngine;
using System.Collections;

public class SelectTipBlockEffect : MonoBehaviour {

	public AnimationCurve ac;
	float DefaultSize;
	float currentSize;

	// Use this for initialization
	void Start () {
		DefaultSize = 1.2f * ISSCBGrid.ISSC_BLOCK_UNIT_SIZE;
	}
	
	// Update is called once per frame
	void Update () {
		currentSize = DefaultSize*ac.Evaluate(Time.time);
		transform.localScale = Vector3.one*currentSize;

	}
}
