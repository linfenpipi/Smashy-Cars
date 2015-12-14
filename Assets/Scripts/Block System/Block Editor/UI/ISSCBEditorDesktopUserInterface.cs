using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum EditingState
{
	Placing,
	Deleting,
	Selecting
}

public enum SelectionOperations
{
	Select,
	Move,
	Cube,
	Sphere
}

public class ISSCBEditorDesktopUserInterface : MonoBehaviour
{
	public Camera viewingCamera;
	public GameObject mainScreenObject;
	public ISSCEUIFilePanel filePanel;
	public Text title;
	public InputField blockSelector;
	public ISSCECommandLineTools clt;
	public ISSCBEditorCore core;

	public Image tipsImage;
	ISSCDBlocksList blockList;

	public Image stateTips;
	public Sprite[] stateSprite;

	public GameObject selectTipBlock;

	EditingState userEditingState = EditingState.Placing;
	public SelectionOperations userSelectionState = SelectionOperations.Select;

	ISSCEMouseCaster mouseCaster;
	ISSCEMouseCaster mouseCasterForLayerSelectingBlock;
	ISSCEMouseCaster mouseCasterForLayerTmpBlocks;

	ISSCBlockVector tmpVector1;
	GameObject tmpGO;
	CamRenders cameraScript;
	float depth;
	int tmpID;
	GameObject fallBackGO;
	bool moving = false;
	float length;
	float radius;

	public Text cubeWidth;
	public Text sphereRadius;

	void Start ()
	{
		NewScene ();
		core.SelectBlock (core.data.EncodeIndex (core.data.GetCenterBlock ()));

		blockList = ISSCDBlocksList.LoadList ();
		mouseCaster = new ISSCEMouseCaster ();
		mouseCaster.viewingCamera = viewingCamera;
		mouseCaster.SetLayerMask (1 << ISSCLayerManager.blockLayer);

		mouseCasterForLayerSelectingBlock = new ISSCEMouseCaster ();
		mouseCasterForLayerSelectingBlock.viewingCamera = viewingCamera;
		mouseCasterForLayerSelectingBlock.SetLayerMask (1 << ISSCLayerManager.selectBlockLayer);

		mouseCasterForLayerTmpBlocks = new ISSCEMouseCaster ();
		mouseCasterForLayerTmpBlocks.viewingCamera = viewingCamera;
		mouseCasterForLayerTmpBlocks.SetLayerMask (1 << ISSCLayerManager.tmpBlockLayer);

		selectTipBlock.transform.rotation = Quaternion.identity;
		cameraScript = viewingCamera.GetComponent<CamRenders> ();
	}

	void Update ()
	{

		bool mouseLeftButtonPress = Input.GetMouseButtonDown (0);
		bool mouseLeftButtonPressed = Input.GetMouseButton (0);
		bool mouseLeftButtonReleased = Input.GetMouseButtonUp (0);

		switch (userEditingState) {
		case EditingState.Placing:
			selectTipBlock.SetActive (false);
			if (mouseLeftButtonPress && mouseCaster.CheckIfHitted ())
				core.PlaceBlockWithCurrentSetting (mouseCaster.CurrentHittingWorldPosition (), mouseCaster.CurrentHittingTransfrom ().position);
			break;

		case EditingState.Deleting:
			selectTipBlock.SetActive (false);
			if (mouseLeftButtonPress && mouseCaster.CheckIfHitted ())
				core.DeleteBlock (mouseCaster.CurrentHittingTransfrom ().position);
			break;

		case EditingState.Selecting:
			selectTipBlock.SetActive (true);
//			if (mouseLeftButtonPress && mouseCaster.CheckIfHitted ()) {
//				core.SelectBlock (mouseCaster.CurrentHittingTransfrom ().position);
//			}
			switch (userSelectionState) {
			case SelectionOperations.Select: 
				if (mouseLeftButtonPress) {
					Down_Selecting_Select ();
				}
				if (mouseLeftButtonPressed) {
					Drag_Selecting_Select ();
				}
				if (mouseLeftButtonReleased) {
					Up_Selecting_Select ();
				}
				break;
			case SelectionOperations.Move: 
				if (mouseLeftButtonPress) {
					Down_Selecting_Move ();
				}
				if (mouseLeftButtonPressed) {
					Drag_Selecting_Move ();
				}
				if (mouseLeftButtonReleased) {
					Up_Selecting_Move ();
				}
				break;
			case SelectionOperations.Cube: 
				if (mouseLeftButtonPress) {
					Down_Selecting_Cube ();
				}
				if (mouseLeftButtonPressed) {
					Drag_Selecting_Cube ();
				}
				if (mouseLeftButtonReleased) {
					Up_Selecting_Cube ();
				}
				break;
			case SelectionOperations.Sphere: 
				if (mouseLeftButtonPress) {
					Down_Selecting_Sphere ();
				}
				if (mouseLeftButtonPressed) {
					Drag_Selecting_Sphere ();
				}
				if (mouseLeftButtonReleased) {
					Up_Selecting_Sphere ();
				}
				break;
			}

			break;
		}

		UpdateUI ();
	}

	void UpdateUI ()
	{

		if(core.currentFillingBlock!=0)
		tipsImage.color = blockList.blocks [core.currentFillingBlock].GetComponent<MeshRenderer> ().sharedMaterial.color;
		else{
		tipsImage.color = new Color(0,0,0,0);
		}

		stateTips.sprite = GetStateSprite ();
		selectTipBlock.transform.position = ISSCBGrid.GridPositionToWorldPosition (core.CurrentSelection (), core.moniter.transform.position);

		if (userSelectionState == SelectionOperations.Cube) {
			cubeWidth.enabled = true;
			cubeWidth.text = "S :\t" + ((int)length).ToString () + " px";
		} else {
			cubeWidth.enabled = false;
		}

		if (userSelectionState == SelectionOperations.Sphere) {
			sphereRadius.enabled = true;
			sphereRadius.text = "R :\t" + ((int)radius).ToString () + " px";
		} else {
			sphereRadius.enabled = false;
		}
	}

	Sprite GetStateSprite ()
	{
		switch (userEditingState) {
		case EditingState.Placing: 
			return stateSprite [0];
		case EditingState.Deleting: 
			return stateSprite [1];
		case EditingState.Selecting: 
			return stateSprite [2];
		default :
			return stateSprite [0];
		}
	}

	public void ChangeToPlacingState ()
	{
		userEditingState = EditingState.Placing;
	}

	public void ChangeToDeletingState ()
	{
		userEditingState = EditingState.Deleting;
	}

	public void ChangeToSelectingState ()
	{
		userEditingState = EditingState.Selecting;
	}

	public void ChangeSelectingStateToSelect ()
	{
		userEditingState = EditingState.Selecting;
		userSelectionState = SelectionOperations.Select;
	}

	public void ChangeSelectingStateToMove ()
	{
		userEditingState = EditingState.Selecting;
		userSelectionState = SelectionOperations.Move;
	}

	public void ChangeSelectingStateToCube ()
	{
		userEditingState = EditingState.Selecting;
		userSelectionState = SelectionOperations.Cube;
	}

	public void ChangeSelectingStateToSphere ()
	{
		userEditingState = EditingState.Selecting;
		userSelectionState = SelectionOperations.Sphere;
	}

	public void Return (string v)
	{
		Debug.Log (v);
		string command = blockSelector.text;

		if (v != "") {
			clt.Submit (v);
		} else {
			clt.Submit (blockSelector.text);
			blockSelector.text = "";
		}
	}

	public void NewScene ()
	{
		Debug.Log ("new s");
		core.NewScene (new ISSCBlockVector (21, 21, 21), "NewScene");
		title.text = "BE 0.2.3 :" + core.data.name;
	}

	public void Save ()
	{
		core.SaveCurrentScene (Application.dataPath + "/Resources/SavedDatas/slot1");
		Debug.Log (Application.dataPath + "Resources/SavedDatas/slot1");
	}

	public void Load ()
	{
		core.OpenScene (Application.dataPath + "/Resources/SavedDatas/slot1");
		title.text = "BE 0.2.3 :" + core.data.name;
	}

	public void ShowMainScreen ()
	{
		mainScreenObject.SetActive (true);
		filePanel.HidePanel ();
	}

	public void ShowSavePanel ()
	{
		mainScreenObject.SetActive (false);
		filePanel.ShowPanel (true);
	}

	public void ShowOpenPanel ()
	{
		mainScreenObject.SetActive (false);
		filePanel.ShowPanel (false);
	}

	void Down_Selecting_Select ()
	{
		if (mouseCaster.CheckIfHitted ()) {
			core.SelectBlock (mouseCaster.CurrentHittingTransfrom ().position);
		}
	}

	void Down_Selecting_Move ()
	{
		if (mouseCaster.CheckIfHitted ())
			CreateTmpGO (mouseCaster.lastestHit);
		else
			return;
	}

	void Down_Selecting_Cube ()
	{
		if (mouseCasterForLayerSelectingBlock.CheckIfHitted ())
			StartCamGLRender ();
		else
			return;
	}

	void Down_Selecting_Sphere ()
	{
		if (mouseCasterForLayerSelectingBlock.CheckIfHitted ())
			StartCamGLRender ();
		else
			return;
	}

	void Drag_Selecting_Select ()
	{
	}

	void Drag_Selecting_Move ()
	{
		TmpGOAndSelectCubeDragingBehaviour (mouseCaster.LastestHit ());
	}

	void Drag_Selecting_Cube ()
	{
		mouseCasterForLayerSelectingBlock.UpdateValues ();
		UpdateCamGLRenderParams ();
	}

	void Drag_Selecting_Sphere ()
	{
		mouseCasterForLayerSelectingBlock.UpdateValues ();
		UpdateCamGLRenderParams ();
	}

	void Up_Selecting_Select ()
	{
	}

	void Up_Selecting_Move ()
	{
		DestroyTmpGOAndPlacing (mouseCaster.lastestHit);
	}

	void Up_Selecting_Cube ()
	{
		StopCamGLRenderAndPlacingCube ();
	}

	void Up_Selecting_Sphere ()
	{
		StopCamGLRenderAndPlacingSphere ();
	}

	void CreateTmpGO (RaycastHit hit)
	{
		if (!hit.collider)
			return;
		moving = true;

		tmpVector1 = ISSCBGrid.WorldPositionToGridPosition (hit.transform.position, core.moniter.transform.position);
		tmpID = core.data.GetRawData () [core.data.EncodeIndex (tmpVector1)];
		fallBackGO = hit.collider.gameObject;
		fallBackGO.SetActive (false);
		tmpGO = Instantiate (fallBackGO) as GameObject;
		tmpGO.SetActive (true);
		tmpGO.transform.localScale *= 0.8f;
		tmpGO.layer = ISSCLayerManager.tmpBlockLayer;
	}

	void StartCamGLRender ()
	{
		cameraScript.selecting = true;
		cameraScript.selectStartPoint = new Vector3 (mouseCasterForLayerSelectingBlock.inputPosition.x / Screen.width, mouseCasterForLayerSelectingBlock.inputPosition.y / Screen.height, 0);
		depth = Vector3.Distance (viewingCamera.ScreenToWorldPoint (mouseCasterForLayerSelectingBlock.inputPosition), ISSCBGrid.GridPositionToWorldPosition (core.CurrentSelection (), core.moniter.transform.position));
	}

	void TmpGOAndSelectCubeDragingBehaviour (RaycastHit hit)
	{
		if (!moving)
			return;
		Vector3 position;
		if (!hit.collider)
			return;
		else if (hit.collider.gameObject.Equals (fallBackGO)) {
			return;
		} else {
			position = ISSCBGrid.GridPositionToWorldPosition (core.CalPlacingPosition (hit.point, hit.collider.transform.position), core.moniter.transform.position);
			core.SelectBlock (position);
			if (!tmpGO)
				return;
			tmpGO.transform.Rotate (tmpGO.transform.up, 45 * Time.deltaTime);
			tmpGO.transform.position = position;
		}
	}

	void UpdateCamGLRenderParams ()
	{
		if(!cameraScript.selecting)return;
		length = (Vector3.Distance (cameraScript.selectStartPoint, cameraScript.selectEndPoint)) * (Mathf.Tan (viewingCamera.fieldOfView) * (depth + 10) * 2);
		radius = (Vector3.Distance (cameraScript.selectStartPoint, cameraScript.selectEndPoint)) * (Mathf.Tan (viewingCamera.fieldOfView) * (depth + 10) * 2);
		cameraScript.selectEndPoint = new Vector3 (mouseCasterForLayerSelectingBlock.inputPosition.x / Screen.width, mouseCasterForLayerSelectingBlock.inputPosition.y / Screen.height, 0);
	}

	void DestroyTmpGOAndPlacing (RaycastHit hit)
	{
		if (!moving)
			return;
		moving = false;
		Destroy (tmpGO);
		if (fallBackGO == null)
			return;
		if (hit.collider.gameObject.Equals (fallBackGO)) {
			fallBackGO.SetActive (true);
			return;
		}
		if (hit.collider) {
			ISSCBlockVector bv = core.CalPlacingPosition (hit.point, hit.collider.transform.position);
			core.data.MoveBlock (tmpVector1, bv, false);
			fallBackGO.SetActive (true);
		} else {
			fallBackGO.SetActive (true);
		}
	}


	void StopCamGLRenderAndPlacingCube ()
	{
		if (cameraScript.selecting) {
			Vector3 v1 = selectTipBlock.transform.position;
			ISSCBlockVector fromPosition = ISSCBGrid.WorldPositionToGridPosition (v1 + Vector3.one * length, core.moniter.transform.position);
			ISSCBlockVector toPosition = ISSCBGrid.WorldPositionToGridPosition (v1 - Vector3.one * length, core.moniter.transform.position);
			ISSCGridPrimitiveShapeUtilities.CreateCube (core.data, core.currentFillingBlock, fromPosition, toPosition);
			cameraScript.selecting = false;
			length = 0;
		}
	}

	void StopCamGLRenderAndPlacingSphere ()
	{
		if (cameraScript.selecting) {
			Vector3 v1 = selectTipBlock.transform.position;
			ISSCGridPrimitiveShapeUtilities.CreateSphere (core.data, ISSCBGrid.WorldPositionToGridPosition (v1, core.moniter.transform.position), core.currentFillingBlock, radius);
			cameraScript.selecting = false;
			radius = 0;
		}
	}

	//	public void MouseCasterAction_MouseDown (RaycastHit hit, RaycastHit hit, Vector3 inputPosition)
	//	{
	//		if (hit.collider) {
	//			UpdateBlockForWorldPosition (hit.point, hit.transform.position);
	//			if (state == ISSCBEditorState.Selecting && selectState == ISSCBEditorSelectingState.Move) {
	//				tmpVector1 = ISSCBGrid.WorldPositionToGridPosition (hit.transform.position, moniter.transform.position);
	//				hit.collider.gameObject.SetActive (false);
	//				tmpGO = Instantiate (hit.collider.gameObject) as GameObject;
	//				tmpGO.SetActive (true);
	//				tmpGO.transform.localScale *= 0.8f;
	//				tmpGO.layer = ISSCLayerManager.tmpBlockLayer;
	//
	//			}
	//		}
	//
	//		if (hit.collider) {
	//			if (state == ISSCBEditorState.Selecting && (selectState == ISSCBEditorSelectingState.Cube || selectState == ISSCBEditorSelectingState.Sphere)) {
	//				cameraScript.selecting = true;
	//				cameraScript.selectStartPoint = new Vector3 (inputPosition.x / Screen.width, inputPosition.y / Screen.height, 0);
	//				depth = Vector3.Distance (viewingCamera.ScreenToWorldPoint (inputPosition), hit.collider.transform.position);
	//			}
	//		}
	//	}
	//
	//	public void MouseCasterAction_MouseDrag (RaycastHit hit, Vector3 inputPosition)
	//	{
	//		if (state == ISSCBEditorState.Selecting) {
	//			switch (selectState) {
	//
	//			case ISSCBEditorSelectingState.Select:
	//				break;
	//			case ISSCBEditorSelectingState.Move:
	//				Vector3 position;
	//				if (hit.collider) {
	//					position = ISSCBGrid.GridPositionToWorldPosition (CalPlacingPosition (hit.point, hit.collider.transform.position), moniter.transform.position);
	//					selectTipBlock.transform.position = position;
	//					tmpGO.transform.Rotate (tmpGO.transform.up, 45 * Time.deltaTime);
	//					tmpGO.transform.position = selectTipBlock.transform.position;
	//				}
	//				break;
	//			case ISSCBEditorSelectingState.Cube:
	//				cameraScript.selectEndPoint = new Vector3 (inputPosition.x / Screen.width, inputPosition.y / Screen.height, 0);
	//				break;
	//			case ISSCBEditorSelectingState.Sphere:
	//				cameraScript.selectEndPoint = new Vector3 (inputPosition.x / Screen.width, inputPosition.y / Screen.height, 0);
	//				break;
	//			}
	//
	//
	//		}
	//	}

	//	public void MouseCasterAction_MouseUp (RaycastHit hit, Vector3 inputPosition)
	//	{
	//		if (state == ISSCBEditorState.Selecting) {
	//			switch (selectState) {
	//
	//			case ISSCBEditorSelectingState.Select:
	//				return;
	//			case ISSCBEditorSelectingState.Move:
	//				Destroy (tmpGO);
	//
	//				if (hit.collider) {
	//					ISSCBlockVector bv = CalPlacingPosition (hit.point, hit.collider.transform.position);
	//					data.SetBlock (bv, data.GetRawData () [data.EncodeIndex (tmpVector1)]);
	//					data.SetBlock (tmpVector1, 0);
	//				}
	//				break;
	//			case ISSCBEditorSelectingState.Cube:
	//				if (cameraScript.selecting) {
	//					Vector3 v1 = selectTipBlock.transform.position;
	//					float length = (Vector3.Distance (cameraScript.selectStartPoint, cameraScript.selectEndPoint)) * (Mathf.Tan (viewingCamera.fieldOfView / 2f) * (depth + 5) * 2);
	//					ISSCBlockVector fromPosition = ISSCBGrid.WorldPositionToGridPosition (v1 + Vector3.one * length, moniter.transform.position);
	//					ISSCBlockVector toPosition = ISSCBGrid.WorldPositionToGridPosition (v1 - Vector3.one * length, moniter.transform.position);
	//					ISSCGridPrimitiveShapeUtilities.CreateCube (data, currentFillingBlock, fromPosition, toPosition);
	//					cameraScript.selecting = false;
	//				}
	//				break;
	//			case ISSCBEditorSelectingState.Sphere:
	//				if (cameraScript.selecting) {
	//					Vector3 v1 = selectTipBlock.transform.position;
	//					float radius = (Vector3.Distance (cameraScript.selectStartPoint, cameraScript.selectEndPoint)) * (Mathf.Tan (viewingCamera.fieldOfView / 2f) * (depth + 5) * 2);
	//					ISSCGridPrimitiveShapeUtilities.CreateSphere (data, ISSCBGrid.WorldPositionToGridPosition (v1, moniter.transform.position), currentFillingBlock, radius);
	//					cameraScript.selecting = false;
	//				}
	//				break;
	//			}
	//
	//
	//		}
	//	}

}
