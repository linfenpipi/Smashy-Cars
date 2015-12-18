using System;
using UnityEngine;
using System.Collections;

/*************************************************************************************
 * ISDelayTimer
 * Created by TRULY_SPINACH
 * ISDelayTimer is a free software, so you can use, modify, release for any purposes.
 * 
 * -----------------------------------------------------------------------------------
 * 
 * Usages :
 * 
 * Call DoAfter to select a timer callback.
 * 
 * Single Line :
 *   Example : 
 * 		ISDelayTimer.DoAfter(2f, () => Debug.Log("2 seconds delay..."));
 * 
 * Want to do more ?
 *   Example : 
 *  	ISDelayTimer.DoAfter(2f, () => {
 * 			Debug.Log("2 seconds delay...");
 * 			Debug.Log("I am just another log after 2 seconds...");
 * 		});
**************************************************************************************/

public class ISDelayTimer{

	static public void DoAfter(float timeUp ,Action todo){
		GameObject obj = new GameObject ("ISDelayTimer");
		obj.AddComponent<ISDelayTimerCaller> ().StartTimer (todo, timeUp);
	}
}

class ISDelayTimerCaller : MonoBehaviour{

	public void StartTimer(Action todo,float timeUp){
		StartCoroutine(TimerCallBack(todo, timeUp));
	}

	IEnumerator	TimerCallBack(Action todo,float timeUp){
		yield return new WaitForSeconds (timeUp);
		todo ();
	}
}
