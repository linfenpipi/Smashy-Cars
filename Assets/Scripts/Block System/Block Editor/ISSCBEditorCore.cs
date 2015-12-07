using UnityEngine;
using System.Collections;

public enum ISSCBEditorState{
	Placing, Deleting
}

public class ISSCBEditorCore : MonoBehaviour {

	public ISSCBEditorState state;
	public ISSCECamera editorCamera;
	public ISSCBGrid data;
	public ISSCBGridController moniter;
	public ISSCEditorUserInterface view;
	public int rootBlock;
	public int currentFillingBlock;


	ISSCEMouseCaster caster;

	void Start(){
		caster = GetComponent<ISSCEMouseCaster> ();
		caster.core = this;
	}

	public void NewScene(ISSCBlockVector size){
		data = new ISSCBGrid (size);
		data.SetBlock (data.GetCenterBlock (), rootBlock);
		Debug.Log("New Scene Created");
		editorCamera.SetViewPoint (ISSCBGrid.GridPositionToWorldPosition(data.GetCenterBlock (), moniter.transform.position));
		moniter.SwitchData (data);
	}

	public void OpenDataSet(ISSCBGrid newDataSet){
		data = newDataSet;
		editorCamera.SetViewPoint (ISSCBGrid.GridPositionToWorldPosition(data.GetCenterBlock (), moniter.transform.position));
		moniter.SwitchData (data);
	}

	public void UpdateBlockForWorldPosition(Vector3 hitPoint, Vector3 hittedBlockPosition){
		ISSCBlockVector bv = ISSCBlockVector.zero;
		int fb = currentFillingBlock;

		switch (state) {
		case ISSCBEditorState.Placing:
			Vector3 dir = hitPoint - hittedBlockPosition;
			dir = ISMath.Clip2NormalDirection (dir);
			bv = ISSCBGrid.WorldPositionToGridPosition (hittedBlockPosition + dir.normalized,moniter.transform.position);
			break;

		case ISSCBEditorState.Deleting:

			bv = ISSCBGrid.WorldPositionToGridPosition (hittedBlockPosition,moniter.transform.position);
			Debug.Log(bv);
			fb = 0;
			break;
		
		}

		data.SetBlock(bv, fb);
	}

	public void DeleteBlockForWorldPosition(Vector3 hittedBlockPosition){
		ISSCBlockVector bv = ISSCBGrid.WorldPositionToGridPosition (hittedBlockPosition,moniter.transform.position);
		
		data.SetBlock(bv, currentFillingBlock);
	}

}
