using UnityEditor;
using UnityEngine;
using System.Collections;

public class ISEditorTools : MonoBehaviour{

	[MenuItem("SPINACH/Tools/Connect Children via Fixed Joint to Selection Root",false,1)]
	static void Connect2SelectionRoot(){
		GameObject rootObj = Selection.activeGameObject;
		GameObject[] childrenTrans = ISEditorTools.GetChildren(rootObj);

		Debug.Log (rootObj);
		Debug.Log (childrenTrans.Length);

		Rigidbody rootRigid = rootObj.GetComponent<Rigidbody> ();
		if (!rootRigid) rootRigid = rootObj.AddComponent<Rigidbody> ();

		for (int i = 0; i < childrenTrans.Length; i++) {
			//if (childrenTrans [i].gameObject == rootObj) continue;

			FixedJoint joint = childrenTrans [i].GetComponent<FixedJoint> ();
			joint = rootObj.gameObject.AddComponent<FixedJoint> ();

			joint.connectedBody = childrenTrans [i].GetComponent<Rigidbody> ();
			/*
			if (i == 0) {
				joint.connectedBody = rootRigid;
			} else {
				joint.connectedBody = childrenTrans[i-1].GetComponent<Rigidbody>();
			}
			*/
		}

		int select = EditorUtility.DisplayDialogComplex ("Successfully Connected", "Do you want to select all joints added to do additional settings ?", "Sure !", "Nope, keep my current selection", "Go away !");
		if (select == 0) {
			Selection.activeGameObject = null;
			GameObject[] objs = new GameObject[childrenTrans.Length];
			for (int i = 0; i < childrenTrans.Length; i++) if(objs [i] != rootObj) objs [i] = childrenTrans [i].gameObject;
			Selection.objects = objs;
		}
		else if (select == 2)
			Debug.LogError ("Sad,,,");

	}

	[MenuItem("SPINACH/Tools/Set Fixed Joint Break Force From Childrens",false,1)]
	static void SetFixedJointBreakForce(){
		GameObject rootObj = Selection.activeGameObject;
		GameObject[] childrenTrans = ISEditorTools.GetChildren(rootObj);

		ISEditorInputDialog.Display ("Break Force", "OK", (string v) => {
			float force = float.Parse(v);
			for (int i = 0; i < childrenTrans.Length; i++) {

				FixedJoint[] joints = childrenTrans[i].gameObject.GetComponents<FixedJoint>();
				foreach(FixedJoint joint in joints){
					joint.breakForce = force;
					joint.breakTorque = force;
				}

			}
		});
	}

	static public GameObject[] GetChildren(GameObject obj){

		int count = obj.transform.childCount;
		GameObject[] list = new GameObject[count];

		for (int i = 0; i < count; i++) list [i] = obj.transform.GetChild (i).gameObject;

		return list;

	}
}
