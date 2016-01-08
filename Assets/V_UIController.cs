using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class V_UIController : MonoBehaviour
{

	public GameController gc;
	PlayerCarBehaviour pcb;

	public Text integrityLevel;
	ColorStruct integrityLevelColorStruct;

	public Text scoreBoard;
	ColorStruct scoreBoardColorStruct;
	float score;
	float lastdistance;
	float drivedDelta;



	// Use this for initialization
	void Start ()
	{
		//
		pcb = gc.playersCar.transform.parent.GetComponent<PlayerCarBehaviour> ();
		//
		IntegrityLevelIntialize ();
		ScoreBoardIntialize ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		IntegrityLevelView ();
		ScoreBoardView ();
	}

	void IntegrityLevelIntialize ()
	{
		integrityLevelColorStruct = new ColorStruct (Color.red, Color.green);
	}

	void IntegrityLevelView ()
	{
		integrityLevel.text = Mathf.Clamp (((int)((float)pcb.hp / (float)pcb.maxHp * 100)), 0, 100).ToString ();
		integrityLevel.color = integrityLevelColorStruct.C (integrityLevelColorStruct.Par ((float)pcb.hp / (float)pcb.maxHp));
	}

	void ScoreBoardIntialize ()
	{
		score = 0;
		lastdistance = 0;
		drivedDelta = 0;
		scoreBoardColorStruct = new ColorStruct (Color.green, Color.red);
	}

	void ScoreBoardView ()
	{
		drivedDelta = Mathf.Clamp(pcb.driveDistance - lastdistance,0,100);
		score += drivedDelta*(float)pcb.hp / (float)pcb.maxHp;
		scoreBoard.text = ((int)(score)).ToString ();
		scoreBoard.color = scoreBoardColorStruct.C (scoreBoardColorStruct.Par (score / 10000));
		lastdistance = pcb.driveDistance;
	}
}

public struct ColorStruct
{
	Color c1;
	Color c2;
	Color cDgree;
	float par;

	public ColorStruct (Color c1, Color c2)
	{
		this.c1 = c1;
		this.c2 = c2;
		cDgree = c2 - c1;
		par = 1;
	}

	public float Par (float p)
	{
		par = Mathf.Clamp (p, 0, 1);
		return par;
	}

	public Color C (float p)
	{
		return (c1 + (cDgree * p));
	}
}
