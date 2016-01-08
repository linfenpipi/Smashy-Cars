using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadChange : MonoBehaviour
{

	public GameObject lastArea;
	public GameObject spawnNet;
	public Vector3[,] netPoints;
	public int spawnedSizeMin;
	public int spawnedSizeMax;
	GameObject[] spawned;
	Vector3[] spawnedPoints;
	public GameObject[] allKindOfCars;
	bool[,] safety;
	public bool[,] settled;
	public int safetyDistance;
	RoadChange last;


	// Use this for initialization
	void Start ()
	{
		last = lastArea.GetComponent<RoadChange> ();
		spawned = new GameObject[0];
		setPoints ();
		yuanfenhao();
//		for (int i = 0; i < 20; i++) {
//			for (int ii = 0; ii < 3; ii++) {
//				Debug.Log (netPoints [i, ii]);
//			}
//		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void changeLast ()
	{
		Vector3 v3 = new Vector3 (-200, 0, 0);
		Transform ts = lastArea.transform;
		ts.position = transform.position + v3;
		lastArea.GetComponent<RoadChange> ().changeSpawns ();

	}

	public void changeSpawns ()
	{
		unspawnAllCars ();
		newSpawnedSize ();
		newSpawnedPoints ();
		newSpawnedGOs ();
	}

	void unspawnAllCars ()
	{	
		for (int i = 0; i < spawned.Length; i++) {
			Destroy (spawned [i]);
//			Debug.Log(gameObject.name + " destroy complete");
		}
	}

	void newSpawnedSize ()
	{
		spawned = new GameObject[Random.Range (spawnedSizeMin, spawnedSizeMax)];
//		Debug.Log(gameObject.name + " newspawnedsize complete");
	}

	void newSpawnedPoints ()
	{
		spawnedPoints = new Vector3[spawned.Length];

		for(int i = 0 ; i < spawnedPoints.Length ; i++){
			spawnedPoints[i] = Vector3.one;
		}

		yuanfenhao ();

		int x, y;

		for (int i = 0; i < spawnedPoints.Length; i++) {
			x = Random.Range (0, 20);
			y = Random.Range (0, 3);
//			Debug.Log (x + " " + y);
			if (yuanfenhaobuhao (x, y)) {
				spawnedPoints [i] = netPoints [x, y];
				yuanfen (x, y);
			}
		}
//		Debug.Log(gameObject.name + " newspawnedpoints complete");
	}

	void yuanfenhao ()
	{
		safety = new bool[20, 3];
		for (int i = 0; i < 20; i++) {
			for (int ii = 0; ii < 3; ii++) {
				safety [i, ii] = true;
			}
		}
		settled = new bool[20, 3];
		for (int i = 0; i < 20; i++) {
			for (int ii = 0; ii < 3; ii++) {
				settled [i, ii] = false;
			}
		}
	}

	void yuanfen (int x, int y)
	{
		

		//self
		settled [x, y] = true;
		int max = Mathf.Clamp (x + safetyDistance, 0, 19);
		int min = Mathf.Clamp (x - safetyDistance, 0, 19);
		while (min <= max) {
			safety [min, y] = false;
			min++;
		}
		//group
		List<Vector2> l = new List<Vector2> ();
		for (int i = 0; i < 20; i++) {
			for (int ii = 0; ii < 3; ii++) {
				if (settled [i, ii]) {
					l.Add (new Vector2 (i, ii));
				}
				if (last.settled [i, ii]) {
					l.Add (new Vector2 (i - 20, ii));
				}
			}
		}

//		for (int i = 0; i < 20; i++) {
//			for (int ii = 0; ii < 3; ii++) {
//				
//			}
//		}

		for (int i = 0; i < 20; i++) {
			for (int ii = 0; ii < 3; ii++) {
				if (!safety [i, ii])
					continue;
				int j = 0;
				for (int i1 = 0; i1 < l.Count; i1++) {
					if (Vector2.Distance (new Vector2 (i, ii), l [i1]) < safetyDistance)
//						Debug.Log(Vector2.Distance (new Vector2 (i, ii), l [i1]) + " out");
						j++;
				}
				if (j >= 2) {
					safety [i, ii] = false;
//					Debug.Log(i + " " + ii + " out");
				}
			}
		}
	}

	bool yuanfenhaobuhao (int x, int y)
	{
		return safety [x, y];
	}

	void newSpawnedGOs ()
	{
		for (int i = 0; i < spawned.Length; i++) {
			if (spawnedPoints [i].Equals(Vector3.one)) {
//				Debug.Log ("spawnedPoints is null");
				Debug.Log("123");
				continue;

			}
			Debug.Log(spawnedPoints[i]);
			spawned [i] = Instantiate (allKindOfCars [Random.Range (0, allKindOfCars.Length)]) as GameObject;
			spawned [i].transform.parent = spawnNet.transform;
			spawned [i].transform.localPosition = spawnedPoints [i];
		}
//		Debug.Log(gameObject.name + " newspawnedgos complete");
	}

	void setPoints ()
	{
		netPoints = new Vector3[20, 3];
		for (int i = 0; i < 20; i++) {
			for (int ii = 0; ii < 3; ii++) {
				netPoints [i, ii] = new Vector3 (-0.05f * (float)i, 0.2f * (float)ii, 0);
//				Debug.Log(netPoints[i,ii].y);
			}
		}
	}
}
