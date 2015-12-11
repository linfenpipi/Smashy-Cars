using UnityEngine;
using System.Collections;

public enum SelectBehaviour
{
	Change,
	Move,
	Sphere,
	Cube
}

public class SelectTipBlockEffect : MonoBehaviour
{
	LayerMask maskedLayer;
	LayerMask tipsBlockLayerMask;
	public SelectBehaviour sb;

	public AnimationCurve ac;
	float DefaultSize;
	float currentSize;

	public Camera viewingCamera;




	void Awake ()
	{

		tipsBlockLayerMask = 1 << gameObject.layer;

		maskedLayer = 1 << ISSCLayerManager.blockLayer;
	}

	// Use this for initialization
	void Start ()
	{
		DefaultSize = 1.2f * ISSCBGrid.ISSC_BLOCK_UNIT_SIZE;
		sb = SelectBehaviour.Change;
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentSize = DefaultSize * ac.Evaluate (Time.time);
		transform.localScale = Vector3.one * currentSize;

	}

	void OnMouseDrag ()
	{
		Ray r1 = viewingCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit rh1;
		Physics.Raycast (r1, out rh1, tipsBlockLayerMask);
		Ray r2 = viewingCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit rh2;
		Physics.Raycast (r2, out rh2, maskedLayer);
		switch (sb) {
		case SelectBehaviour.Change:
			break;
		case SelectBehaviour.Move:
			break;
		case SelectBehaviour.Sphere:
			break;
		case SelectBehaviour.Cube:
			break;
		default :
			Debug.Log ("Null Select-Behaviour exist!");
			break;
		}
	}
}
