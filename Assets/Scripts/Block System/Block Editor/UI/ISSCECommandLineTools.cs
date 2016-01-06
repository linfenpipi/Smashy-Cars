using UnityEngine;
using System.Collections;

public class ISSCECommandLineTools : MonoBehaviour
{

	public ISSCBEditorCore core;

	public void Submit (string command)
	{
		if (command.StartsWith ("select block")) {
			string argu = command.Substring (12);
			core.currentFillingBlock = int.Parse (argu);
		} else if (command.StartsWith ("sb")) {
			string argu = command.Substring (2);
			core.currentFillingBlock = int.Parse (argu);
		} else if (command.StartsWith ("save")) {
			string argu = command.Substring (command.IndexOf ("\"") + 1, command.LastIndexOf ("\"") - command.IndexOf ("\"") - 1);
			core.SaveCurrentScene (Application.dataPath + "/Resources/SavedDatas/" + argu);
		} else if (command.StartsWith ("load")) {
			string argu = command.Substring (command.IndexOf ("\"") + 1, command.LastIndexOf ("\"") - command.IndexOf ("\"") - 1);
			core.OpenScene (Application.dataPath + "/Resources/SavedDatas/" + argu);
		} else if (command.StartsWith ("new")) {
			string[] argus = ParseArguments (command);
			int size = int.Parse (argus [0]);
			core.NewScene (new ISSCBlockVector (size, size, size), "New Scene");
		} else if (command.StartsWith ("sphere")) {
			string[] argus = ParseArguments (command);
			ISSCBlockVector center = ParseBlockVector (argus [0]);
			int r = int.Parse (argus [1]);
			int fillingBlock = int.Parse (argus [2]);

			ISSCGridPrimitiveShapeUtilities.CreateSphere (core.data, center, fillingBlock, r);
		} else if (command.StartsWith ("cube")) {
			string[] argus = ParseArguments (command);
			ISSCBlockVector a = ParseBlockVector (argus [0]);
			ISSCBlockVector b = ParseBlockVector (argus [1]);
			int fillingBlock = int.Parse (argus [2]);

			ISSCGridPrimitiveShapeUtilities.CreateCube (core.data, fillingBlock, a, b);
		} else if (command.StartsWith ("move")) {
			string[] argus = ParseArguments (command);
			ISSCBlockVector a = ParseBlockVector (argus [0]);
			ISSCBlockVector b = ParseBlockVector (argus [1]);
			bool forceMove;
			switch (argus [2]) {
			case "true": 
				forceMove = true;
				break;
			case "false": 
				forceMove = false;
				break;
			default :
				forceMove = true;
				break;
			}

			core.data.MoveBlock(a,b,forceMove);
		}else if (command.StartsWith ("currentmoveto")) {
			string[] argus = ParseArguments (command);
			ISSCBlockVector a = ParseBlockVector (argus [0]);
			bool forceMove;
			switch (argus [1]) {
			case "true": 
				forceMove = true;
				break;
			case "false": 
				forceMove = false;
				break;
			default :
				forceMove = true;
				break;
			}
			ISSCBlockVector currentBlock = ISSCBGrid.WorldPositionToGridPosition(core.tipsBlock.transform.position,core.moniter.transform.position);
			core.data.MoveBlock(currentBlock,a,forceMove);
			core.tipsBlock.transform.position = ISSCBGrid.GridPositionToWorldPosition(a,core.moniter.transform.position);
			Debug.Log( ISSCBGrid.GridPositionToWorldPosition(a,core.moniter.transform.position));
		}
	}
		
	// -example -example2
	string[] ParseArguments (string command)
	{
		char[] ca = command.ToCharArray ();
		int count = 0;
		for (int i = 0; i < ca.Length; i++)
			if (ca [i] == '-')
				count++;

		string[] argus = new string[count];
		string tempString = string.Empty;
		count = -1;

		for (int i = 0; i < ca.Length; i++) {
			if (ca [i] == '-') {
				if (count >= 0)
					argus [count] = tempString;
				tempString = string.Empty;
				count++;
			} else {
				if (count >= 0)
					tempString += ca [i].ToString ();
			}
		}
		if (count >= 0)
			argus [count] = tempString;

		return argus;
	}

	// x,y,z
	ISSCBlockVector ParseBlockVector (string vectorString)
	{
		string sub = vectorString;

		string x = sub.Substring (0, sub.IndexOf (","));
		sub = sub.Substring (sub.IndexOf (",") + 1);

		string y = sub.Substring (0, sub.IndexOf (","));
		sub = sub.Substring (sub.IndexOf (",") + 1);

		string z = sub;
		sub = sub.Substring (sub.IndexOf (",") + 1);

		return new ISSCBlockVector (int.Parse (x), int.Parse (y), int.Parse (z));
	}
}
