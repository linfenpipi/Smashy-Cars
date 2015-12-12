using UnityEngine;
using System.Collections;

public enum ISSCBEditorState
{
	Placing,
	Deleting,
	Selecting
}

public class ISSCBEditorCore : MonoBehaviour
{

	public ISSCBEditorState state;
	public ISSCECamera editorCamera;
	public ISSCBGrid data;
	public ISSCBGridController moniter;
	public ISSCEditorUserInterface view;
	public int rootBlock;
	public int currentFillingBlock;
	public GameObject tipsBlock;

//	public Vector3 selectTipPosition;


	ISSCEMouseCaster caster;

	void Start ()
	{
		caster = GetComponent<ISSCEMouseCaster> ();
		caster.core = this;
	}

	public void SaveCurrentScene (string path)
	{
		ISSCDGridFileUtilities.CreateFile (data, path);
	}

	public void OpenScene (string path)
	{
		OpenScene (ISSCDGridFileUtilities.LoadFromFile (path));
	}

	public void OpenScene (ISSCBGrid newDataSet)
	{
		data = newDataSet;
		editorCamera.SetViewPoint (ISSCBGrid.GridPositionToWorldPosition (data.GetCenterBlock (), moniter.transform.position));
		moniter.SwitchDataSet (data);
	}

	public void NewScene (ISSCBlockVector size, string name)
	{
		data = new ISSCBGrid (size);
		data.name = name;
		data.SetBlock (data.GetCenterBlock (), rootBlock);
		Debug.Log ("New Scene Created");
		editorCamera.SetViewPoint (ISSCBGrid.GridPositionToWorldPosition (data.GetCenterBlock (), moniter.transform.position));
		moniter.SwitchDataSet (data);
	}

	public void UpdateBlockForWorldPosition (Vector3 hitPoint, Vector3 hittedBlockPosition, Transform ts)
	{
		ISSCBlockVector bv = ISSCBlockVector.zero;
		int fb = currentFillingBlock;

		switch (state) {
		case ISSCBEditorState.Placing:
			Vector3 dir = hitPoint - hittedBlockPosition;
			dir = ISMath.Clip2NormalDirectionV2 (dir, ts);
			bv = ISSCBGrid.WorldPositionToGridPosition (dir + ts.position, moniter.transform.position);
			break;

		case ISSCBEditorState.Deleting:

			bv = ISSCBGrid.WorldPositionToGridPosition (hittedBlockPosition, moniter.transform.position);
			Debug.Log (bv);
			fb = 0;
			break;

		case ISSCBEditorState.Selecting:

			tipsBlock.transform.position = ts.position;

			return;
		}

		data.SetBlock (bv, fb);
	}

	public void DeleteBlockForWorldPosition (Vector3 hittedBlockPosition)
	{
		ISSCBlockVector bv = ISSCBGrid.WorldPositionToGridPosition (hittedBlockPosition, moniter.transform.position);
		
		data.SetBlock (bv, currentFillingBlock);
	}

}
