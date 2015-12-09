using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ISSCEditorUserInterface : MonoBehaviour
{

	public InputField blockSelector;
	public ISSCECommandLineTools clt;
	public ISSCBEditorCore core;

	public Image tipsImage;
	ISSCDBlocksList blockList;

	public Image stateTips;
	public Sprite[] stateSprite;

	public GameObject selectTipBlock;

	void Start ()
	{
		blockList = ISSCDBlocksList.LoadList ();
	}

	void Update ()
	{
		tipsImage.color = blockList.blocks [core.currentFillingBlock].GetComponent<MeshRenderer> ().sharedMaterial.color;
		stateTips.sprite = GetStateSprite ();
		if(core.state == ISSCBEditorState.Selecting){
			selectTipBlock.SetActive(true);
			selectTipBlock.transform.position = core.selectTipPosition;
			selectTipBlock.transform.rotation = Quaternion.identity;
		}else{
			selectTipBlock.SetActive(false);
		}
	}

	Sprite GetStateSprite ()
	{
		switch (core.state) {
		case ISSCBEditorState.Placing: 
			return stateSprite [0];
		case ISSCBEditorState.Deleting: 
			return stateSprite [1];
		case ISSCBEditorState.Selecting: 
			return stateSprite [2];
		default :
			return stateSprite [0];
		}
	}

	public void ChangeToPlacingState ()
	{
		core.state = ISSCBEditorState.Placing;
	}

	public void ChangeToDeletingState ()
	{
		core.state = ISSCBEditorState.Deleting;
	}

	public void ChangeToSelectingState(){
		core.state = ISSCBEditorState.Selecting;
	}

	public void Return (string v)
	{
		if (v != "") {

			clt.Submit (v);

		} else {

			string command = blockSelector.text;

			clt.Submit (blockSelector.text);

			Debug.Log ("Command Accepted... " + command);
			blockSelector.text = "";
		}
	}

	public void NewScene ()
	{
		core.NewScene (new ISSCBlockVector (21, 21, 21), "NewScene");
	}

	public void Save ()
	{
		core.SaveCurrentScene (Application.dataPath + "/Resources/SavedDatas/slot1");
		Debug.Log (Application.dataPath + "Resources/SavedDatas/slot1");
	}

	public void Load ()
	{
		core.OpenScene (Application.dataPath + "/Resources/SavedDatas/slot1");
	}
}
