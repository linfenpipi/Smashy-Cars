using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ISSCEUIFilePanel : MonoBehaviour {

	public ISSCEditorUserInterface uiCore;
	public string defaultPath;
	public InputField textField;
	public Text title;
	public Text buttonLabel;


	bool savingFlag;

	void Awake(){
		defaultPath = Application.dataPath + "/Resources/SavedDatas/";
	}

	public void ShowPanel(bool saving){
		savingFlag = saving;

		gameObject.SetActive (true);
		if(saving) buttonLabel.text = "Save";
		else buttonLabel.text = "Open";

		textField.text = defaultPath + uiCore.core.data.name;
	}

	public void HidePanel(){
		gameObject.SetActive (false);
	}

	public void ShowMainScreen(){
		uiCore.ShowMainScreen ();
	}

	public void MainButtonAction(){
		if (savingFlag) {
			uiCore.core.SaveCurrentScene (textField.text);
		} else {
			uiCore.core.OpenScene (textField.text);
		}

		uiCore.ShowMainScreen ();
	}
}
