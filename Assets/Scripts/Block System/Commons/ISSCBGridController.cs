using UnityEngine;
using System.Collections;

public class ISSCBGridController : MonoBehaviour
{

	public ISSCBGridDescriber grid;
	public bool updateDataEachFrame;

	ISSCBGrid gridData;
	int currentVersion = 0;
	int[] versionDataCache;

	ISSCDBlocksList blockList;
	GameObject[] blockObjects;

	void Start ()
	{
		blockList = ISSCDBlocksList.LoadList ();
		gridData = new ISSCBGrid (grid);
		int length = gridData.gridSize.Length ();
		blockObjects = new GameObject[length];
		versionDataCache = new int[length];
	}

	void Update(){
		if(updateDataEachFrame) UpdateSceneWithData ();
	}

	public void SwitchData(ISSCBGrid newDataSet){
		gridData = newDataSet;
		currentVersion = -1;
		versionDataCache = new int[newDataSet.GetRawData().Length];
		UpdateSceneWithData ();

		Debug.Log("data switched...");
	}

	void UpdateSceneWithData ()
	{
		int versionCheckResult = gridData.IsLastestVersion (currentVersion);
		if(versionCheckResult == -1) return;

		Debug.Log("New version detected, updating " + (versionCheckResult - currentVersion).ToString() + " changes...");

		int[] data = gridData.GetRawData ();

		for (int i = 0; i < data.Length; i++) {
			if(data[i] == versionDataCache[i]) continue;
			versionDataCache[i] = data[i];

			ISSCBlockVector b = gridData.DecodeIndex(i);

			if (blockObjects [i]) ISObjectPoolManager.Unspawn (blockObjects [i]);
			if(data[i] <= 1) continue;
			if(!gridData.IsBlockVisiable(b)) continue;

			Vector3 position = ISSCBGrid.GridPositionToWorldPosition (b, transform.position);
			blockObjects [i] = ISObjectPoolManager.Spawn (blockList.blocks [data [i]].gameObject, position, Quaternion.identity) as GameObject;
		}

		currentVersion = versionCheckResult;
	}
}
