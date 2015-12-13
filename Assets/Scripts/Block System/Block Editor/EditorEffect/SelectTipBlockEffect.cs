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

	public ISSCBEditorCore core;




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



}
