using UnityEngine;
using System.Collections;

public class ISSCBEditorCore : MonoBehaviour{

	//public ISSCBEditorSelectingState selectState;
	public ISSCECamera editorCamera;
	public ISSCBGrid data;
	public ISSCBGridController moniter;
	public ISSCBEditorDesktopUserInterface view;
	public int rootBlock;
	public int currentFillingBlock;
	public GameObject tipsBlock;
	public CamRenders cameraScript;

	int[] selections;


	float depth;
	Camera mCamera;
	ISSCBlockVector tmpVector1;
	GameObject tmpGO;

	void Start (){
		mCamera = editorCamera.GetComponentInChildren<Camera> ();
	}

	public void SaveCurrentScene (string path){
		ISSCDGridFileUtilities.CreateFile (data, path);
	}

	public void OpenScene (string path){
		OpenScene (ISSCDGridFileUtilities.LoadFromFile (path));
	}

	public void OpenScene (ISSCBGrid newDataSet){
		data = newDataSet;
		editorCamera.SetViewPoint (ISSCBGrid.GridPositionToWorldPosition (data.GetCenterBlock (), moniter.transform.position));
		moniter.SwitchDataSet (data);
	}

	public void NewScene (ISSCBlockVector size, string name){
		data = new ISSCBGrid (size);
		data.name = name;
		data.SetBlock (data.GetCenterBlock (), rootBlock);
		Debug.Log ("New Scene Created");
		editorCamera.SetViewPoint (ISSCBGrid.GridPositionToWorldPosition (data.GetCenterBlock (), moniter.transform.position));
		moniter.SwitchDataSet (data);
	}

	public void SelectBlock(Vector3 position){
		SelectBlock (ISSCBGrid.WorldPositionToGridPosition (position, moniter.transform.position));
	}

	public void SelectBlock(ISSCBlockVector block){
		SelectBlock(data.EncodeIndex (block));
	}

	public void SelectBlock(int blockID){
		int[] array = new int[1];
		array [0] = blockID;
		selections = array;
	}

	public void SelectBlocks(int[] newSelections){
		selections = newSelections;
	}

	public ISSCBlockVector CurrentSelection(){
		return data.DecodeIndex (selections [0]);
	}

	public void PlaceBlockWithCurrentSetting(Vector3 hitPoint, Vector3 hittedBlockPosition){
		PlaceBlockWithCurrentSetting (CalPlacingPosition (hitPoint, hittedBlockPosition));
	}

	public void PlaceBlockWithCurrentSetting(ISSCBlockVector block){
		data.SetBlock (block, currentFillingBlock);
	}

	public void DeleteBlock(Vector3 hittedBlockPosition){
		DeleteBlock (BlockForWorldPosition(hittedBlockPosition));
	}

	public void DeleteBlock(ISSCBlockVector block){
		data.SetBlock (block, 0);
	}

	public ISSCBlockVector BlockForWorldPosition(Vector3 position){
		return ISSCBGrid.WorldPositionToGridPosition (position, moniter.transform.position);
	}

	public ISSCBlockVector CalPlacingPosition (Vector3 hitPoint, Vector3 hittedBlockPosition)
	{
		Vector3 dir = hitPoint - hittedBlockPosition;
		dir = ISMath.Clip2NormalDirectionV2 (dir);
		return ISSCBGrid.WorldPositionToGridPosition (dir + hittedBlockPosition, moniter.transform.position);
	}

	/*
	public void MouseCasterAction_MouseDown (RaycastHit hit1, RaycastHit hit2, Vector3 inputPosition){
		if (hit1.collider) {
			UpdateBlockForWorldPosition (hit1.point, hit1.transform.position);
			if (state == ISSCBEditorState.Selecting && selectState == ISSCBEditorSelectingState.Move) {
				tmpVector1 = ISSCBGrid.WorldPositionToGridPosition (hit1.transform.position, moniter.transform.position);
				hit1.collider.gameObject.SetActive (false);
				tmpGO = Instantiate (hit1.collider.gameObject) as GameObject;
				tmpGO.SetActive (true);
				tmpGO.transform.localScale *= 0.8f;
				tmpGO.layer = ISSCLayerManager.tmpBlockLayer;

			}
		}

		if (hit2.collider) {
			if (state == ISSCBEditorState.Selecting && (selectState == ISSCBEditorSelectingState.Cube || selectState == ISSCBEditorSelectingState.Sphere)) {
				cameraScript.selecting = true;
				cameraScript.selectStartPoint = new Vector3 (inputPosition.x / Screen.width, inputPosition.y / Screen.height, 0);
				depth = Vector3.Distance (mCamera.ScreenToWorldPoint (inputPosition), hit2.collider.transform.position);
			}
		}
	}

	public void MouseCasterAction_MouseDrag (RaycastHit hit,Vector3 inputPosition){
		if (state == ISSCBEditorState.Selecting) {
				switch (selectState) {

				case ISSCBEditorSelectingState.Select:
					break;
				case ISSCBEditorSelectingState.Move:
					Vector3 position;
					if (hit.collider) {
						position = ISSCBGrid.GridPositionToWorldPosition (CalPlacingPosition (hit.point, hit.collider.transform.position), moniter.transform.position);
						tipsBlock.transform.position = position;
						tmpGO.transform.Rotate (tmpGO.transform.up, 45 * Time.deltaTime);
						tmpGO.transform.position = tipsBlock.transform.position;
					}
					break;
				case ISSCBEditorSelectingState.Cube:
					cameraScript.selectEndPoint = new Vector3 (inputPosition.x / Screen.width, inputPosition.y / Screen.height, 0);
					break;
				case ISSCBEditorSelectingState.Sphere:
					cameraScript.selectEndPoint = new Vector3 (inputPosition.x / Screen.width, inputPosition.y / Screen.height, 0);
					break;
				}


			}
	}

	public void MouseCasterAction_MouseUp (RaycastHit hit, Vector3 inputPosition){
			if (state == ISSCBEditorState.Selecting) {
				switch (selectState) {

				case ISSCBEditorSelectingState.Select:
					return;
				case ISSCBEditorSelectingState.Move:
					Destroy (tmpGO);

					if (hit.collider) {
						ISSCBlockVector bv = CalPlacingPosition (hit.point, hit.collider.transform.position);
						data.SetBlock (bv, data.GetRawData () [data.EncodeIndex (tmpVector1)]);
						data.SetBlock (tmpVector1, 0);
					}
					break;
				case ISSCBEditorSelectingState.Cube:
					if (cameraScript.selecting) {
						Vector3 v1 = tipsBlock.transform.position;
						float length = (Vector3.Distance (cameraScript.selectStartPoint, cameraScript.selectEndPoint)) * (Mathf.Tan (mCamera.fieldOfView/2f) * (depth + 5) * 2);
						ISSCBlockVector fromPosition = ISSCBGrid.WorldPositionToGridPosition (v1 + Vector3.one * length, moniter.transform.position);
						ISSCBlockVector toPosition = ISSCBGrid.WorldPositionToGridPosition (v1 - Vector3.one * length, moniter.transform.position);
						ISSCGridPrimitiveShapeUtilities.CreateCube (data, currentFillingBlock, fromPosition, toPosition);
						cameraScript.selecting = false;
					}
					break;
				case ISSCBEditorSelectingState.Sphere:
					if (cameraScript.selecting) {
						Vector3 v1 = tipsBlock.transform.position;
						float radius = (Vector3.Distance (cameraScript.selectStartPoint, cameraScript.selectEndPoint)) * (Mathf.Tan (mCamera.fieldOfView/2f) * (depth + 5) * 2);
						ISSCGridPrimitiveShapeUtilities.CreateSphere (data, ISSCBGrid.WorldPositionToGridPosition (v1, moniter.transform.position), currentFillingBlock, radius);
						cameraScript.selecting = false;
					}
					break;
				}


			}
	}
	*/
}
