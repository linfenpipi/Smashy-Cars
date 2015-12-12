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

	public Light[] lights;

	void Start ()
	{
		blockList = ISSCDBlocksList.LoadList ();

		for (int i = 0; i < blockList.blocks.Length; i++)
			ISObjectPoolManager.New (blockList.blocks [i].gameObject, 1);

		gridData = new ISSCBGrid (grid);
		int length = gridData.gridSize.Length ();
		blockObjects = new GameObject[length];
		versionDataCache = new int[length];

		LightSetting ();
	}

	void Update ()
	{
		if (updateDataEachFrame)
			UpdateEntireScene ();
	}

	public void SwitchDataSet (ISSCBGrid newDataSet)
	{
		gridData = newDataSet;
		currentVersion = -1;
		ClearNCreateNewInSceneCaches (gridData.gridSize.Length ());
		UpdateEntireScene ();

		Debug.Log ("data switched...");
	}

	void LightSetting ()
	{
		lights = GetComponentsInChildren<Light> ();
		lights [0].transform.position = ISSCBGrid.GridPositionToWorldPosition (new ISSCBlockVector (0, 0, 0), transform.position);
		lights [1].transform.position = ISSCBGrid.GridPositionToWorldPosition (new ISSCBlockVector (0, gridData.gridSize.y - 1, 0), transform.position);
		lights [2].transform.position = ISSCBGrid.GridPositionToWorldPosition (new ISSCBlockVector (gridData.gridSize.x - 1, 0, 0), transform.position);
		lights [3].transform.position = ISSCBGrid.GridPositionToWorldPosition (new ISSCBlockVector (gridData.gridSize.x - 1, gridData.gridSize.y - 1, 0), transform.position);
		lights [4].transform.position = ISSCBGrid.GridPositionToWorldPosition (new ISSCBlockVector (gridData.gridSize.x - 1, 0, gridData.gridSize.z - 1), transform.position);
		lights [5].transform.position = ISSCBGrid.GridPositionToWorldPosition (new ISSCBlockVector (gridData.gridSize.x - 1, gridData.gridSize.y - 1, gridData.gridSize.z - 1), transform.position);
		lights [6].transform.position = ISSCBGrid.GridPositionToWorldPosition (new ISSCBlockVector (0, 0, gridData.gridSize.z - 1), transform.position);
		lights [7].transform.position = ISSCBGrid.GridPositionToWorldPosition (new ISSCBlockVector (0, gridData.gridSize.y - 1, gridData.gridSize.z - 1), transform.position);

		for(int i = 0 ; i < lights.Length ; i++){
//		lights[i].transform.LookAt(ISSCBGrid.GridPositionToWorldPosition(gridData.GetCenterBlock(),transform.position));
			lights[i].range = Mathf.Sqrt(Mathf.Pow(gridData.gridSize.x-1,2)+Mathf.Pow(gridData.gridSize.y-1,2)+Mathf.Pow(gridData.gridSize.z-1,2));
//			lights[i].spotAngle = 90;
		}
	}

	void UpdateEntireScene ()
	{
		int versionCheckResult = gridData.IsLastestVersion (currentVersion);
		if (versionCheckResult == -1)
			return;

		Debug.Log ("New version detected, updating " + (versionCheckResult - currentVersion).ToString () + " changes...");

		int[] data = gridData.GetRawData ();

		for (int i = 0; i < data.Length; i++) {
			if (data [i] == versionDataCache [i])
				continue;

			//Once a block is changed(otherwise, loop is continued), check the surrounding blocks and see if some of them also need to be updated.
			UpdateBlocksCacheIgroned (gridData.SurroundingBlocks (gridData.DecodeIndex (i)));

			versionDataCache [i] = data [i];

			ISSCBlockVector b = gridData.DecodeIndex (i);

			if (blockObjects [i])
				ISObjectPoolManager.Unspawn (blockObjects [i]);
			if (data [i] <= 1)
				continue;
			if (!gridData.IsBlockVisiable (b))
				continue;

			Vector3 position = ISSCBGrid.GridPositionToWorldPosition (b, transform.position);
			blockObjects [i] = ISObjectPoolManager.Spawn (blockList.blocks [data [i]].gameObject, position, Quaternion.identity) as GameObject;
		}

		currentVersion = versionCheckResult;
	}

	public void UpdateBlocksCacheIgroned (ISSCBlockVector[] blocks)
	{
		for (int i = 0; i < blocks.Length; i++) {
			UpdateBlockCacheIgroned (blocks [i]);
		}
	}

	public void UpdateBlockCacheIgroned (ISSCBlockVector block)
	{
		int id = gridData.EncodeIndex (block);

		if (blockObjects [id])
			ISObjectPoolManager.Unspawn (blockObjects [id]);

		if (!gridData.IsBlockVisiable (block))
			return;
		int[] data = gridData.GetRawData ();

		if (data [id] <= 1)
			return;

		Vector3 position = ISSCBGrid.GridPositionToWorldPosition (block, transform.position);
		blockObjects [id] = ISObjectPoolManager.Spawn (blockList.blocks [data [id]].gameObject, position, Quaternion.identity) as GameObject;
	}

	void ClearNCreateNewInSceneCaches (int newLength)
	{
		versionDataCache = new int[newLength];

		for (int i = 0; i < blockObjects.Length; i++) {
			if (blockObjects [i])
				ISObjectPoolManager.Unspawn (blockObjects [i]);
		}

		blockObjects = new GameObject[newLength];
	}
}
