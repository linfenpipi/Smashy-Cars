using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ISSCEditorUserInterface : MonoBehaviour {

	public InputField blockSelector;
	public ISSCECommandLineTools clt;
	public ISSCBEditorCore core;

	public void ChangeToPlacingState(){
		core.state = ISSCBEditorState.Placing;
	}

	public void ChangeToDeletingState(){
		core.state = ISSCBEditorState.Deleting;
	}

	public void Return(string v){
		string command = blockSelector.text;

		clt.Submit (blockSelector.text);

		Debug.Log ("Command Accepted... " + command);
		blockSelector.text = "";
	}

	public void NewScene(){
		core.NewScene (new ISSCBlockVector (21, 21, 21),"NewScene");
	}

	public void Save(){
		core.SaveCurrentScene (Application.dataPath + "/Resources/SavedDatas/slot1");
		Debug.Log (Application.dataPath + "Resources/SavedDatas/slot1");
	}

	public void Load(){
		core.OpenScene (Application.dataPath + "/Resources/SavedDatas/slot1");
	}
}
