using UnityEngine;
using System.Collections;

public class ISSCEMouseCaster
{

	public Camera viewingCamera;

	LayerMask maskedLayer = System.Int32.MaxValue;

	public Vector3 inputPosition;
	bool lastestHitSucceed = false;
	public RaycastHit lastestHit;
	Vector3 lastestHittingPosition = Vector3.zero;
	Transform lastestHitingTransfrom;

	public void SetLayerMask(LayerMask mask){
		maskedLayer = mask;
	}

	public Vector3 CurrentMousePosition(){
		UpdateValues ();
		return Input.mousePosition;
	}

	public Transform CurrentHittingTransfrom(){
		UpdateValues ();
		return lastestHitingTransfrom;
	}

	public Vector3 CurrentHittingWorldPosition(){
		UpdateValues ();
		return lastestHittingPosition;
	}

	public bool CheckIfHitted(){
		UpdateValues ();
		return lastestHitSucceed;
	}

	public RaycastHit LastestHit(){
		UpdateValues();
		return lastestHit;
	}

	public void UpdateValues(){
		inputPosition = Input.mousePosition;

		RaycastHit hit;
		Ray r = viewingCamera.ScreenPointToRay (inputPosition);

		lastestHitSucceed = Physics.Raycast (r, out hit, 10000f, maskedLayer);
		if (lastestHitSucceed) {
			lastestHit = hit;
			lastestHittingPosition = lastestHit.point;
			lastestHitingTransfrom = lastestHit.transform;
		}
	}
}