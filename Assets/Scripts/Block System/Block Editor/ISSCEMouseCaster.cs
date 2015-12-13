using UnityEngine;
using System.Collections;

public class ISSCEMouseCaster : MonoBehaviour
{

	public Camera viewingCamera;
	public ISSCBEditorCore core;

	LayerMask maskedLayer;
	LayerMask selectMaskedLayer;
	Vector3 inputPosition;

	ISSCBlockVector tmpVector1;
	ISSCBlockVector tmpVector2;
	GameObject tmpGO;

	float depth;

	public float rayDistance;

	public CamRenders cameraScript;

	void Awake ()
	{
		maskedLayer = 1 << ISSCLayerManager.blockLayer;
		selectMaskedLayer = 1 << ISSCLayerManager.selectBlockLayer;
	}

	void Start ()
	{

	}

	void Update ()
	{
		inputPosition = Input.mousePosition;

		//core.core.MouseCasterAction_MouseDown()
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray r = viewingCamera.ScreenPointToRay (inputPosition);
			Physics.Raycast (r, out hit, rayDistance, maskedLayer);
			RaycastHit hit2;
			Ray r2 = viewingCamera.ScreenPointToRay (inputPosition);
			Physics.Raycast (r2, out hit2, rayDistance, selectMaskedLayer);
			core.MouseCasterAction_MouseDown (hit, hit2, inputPosition);
		}
		//core.core.MouseCasterAction_MouseDrag()
		if (Input.GetMouseButton (0)) {
			Ray r = viewingCamera.ScreenPointToRay (inputPosition);
			RaycastHit hit;
			Physics.Raycast (r, out hit, rayDistance, maskedLayer);
			core.MouseCasterAction_MouseDrag (hit, inputPosition);
		}
		//core.core.MouseCasterAction_MouseUp()
		if (Input.GetMouseButtonUp (0)) {

			RaycastHit hit;
			Ray r = viewingCamera.ScreenPointToRay (Input.mousePosition);
			Physics.Raycast (r, out hit, rayDistance, maskedLayer);
			core.MouseCasterAction_MouseUp (hit, inputPosition);
		}
	}



	
}