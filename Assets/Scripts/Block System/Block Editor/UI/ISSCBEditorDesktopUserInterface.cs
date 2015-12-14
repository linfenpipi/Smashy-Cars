using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum EditingState{
	Placing,
	Deleting,
	Selecting
}

enum SelectionOperations{
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

	ISSCEMouseCaster mouseCaster;

	void Start ()
	{
		blockList = ISSCDBlocksList.LoadList ();
		mouseCaster = new ISSCEMouseCaster ();
		mouseCaster.viewingCamera = viewingCamera;
		mouseCaster.SetLayerMask (1 << ISSCLayerManager.blockLayer);
	}

	void Update(){

		bool mouseLeftButtonPress = Input.GetMouseButtonDown (0);
		bool mouseLeftButtonPressed = Input.GetMouseButton (0);
		bool mouseLeftButtonReleased = Input.GetMouseButtonUp (0);

		switch (userEditingState) {
		case EditingState.Placing:
			if (mouseLeftButtonPress && mouseCaster.CheckIfHitted()) core.PlaceBlockWithCurrentSetting (mouseCaster.CurrentHittingWorldPosition (), mouseCaster.CurrentHittingTransfrom ().position);
			break;

		case EditingState.Deleting:
			if (mouseLeftButtonPress && mouseCaster.CheckIfHitted()) core.DeleteBlock (mouseCaster.CurrentHittingTransfrom ().position);
			break;

		case EditingState.Selecting:
			if (mouseLeftButtonPress && mouseCaster.CheckIfHitted()) core.SelectBlock (mouseCaster.CurrentHittingTransfrom ().position);
			break;
		}

		UpdateUI ();
	}

	void UpdateUI ()
	{
		tipsImage.color = blockList.blocks [core.currentFillingBlock].GetComponent<MeshRenderer> ().sharedMaterial.color;
		stateTips.sprite = GetStateSprite ();

		selectTipBlock.SetActive(true);
		selectTipBlock.transform.rotation = Quaternion.identity;
		selectTipBlock.SetActive(false);
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

	public void ChangeToSelectingState(){
		userEditingState = EditingState.Selecting;
	}

	public void ChangeSelectingStateToSelect(){
		//userEditingState = ISSCBEditorSelectingState.Select;
	}

	public void ChangeSelectingStateToMove(){
		//userEditingState = ISSCBEditorSelectingState.Move;
	}

	public void ChangeSelectingStateToCube(){
		//userEditingState = ISSCBEditorSelectingState.Cube;
	}

	public void ChangeSelectingStateToSphere(){
		//userEditingState = ISSCBEditorSelectingState.Sphere;
	}

	public void Return (string v)
	{
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

	public void ShowMainScreen(){
		mainScreenObject.SetActive (true);
		filePanel.HidePanel ();
	}

	public void ShowSavePanel(){
		mainScreenObject.SetActive (false);
		filePanel.ShowPanel (true);
	}

	public void ShowOpenPanel(){
		mainScreenObject.SetActive (false);
		filePanel.ShowPanel (false);
	}
}
