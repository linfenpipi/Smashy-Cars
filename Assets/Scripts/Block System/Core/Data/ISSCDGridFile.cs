using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;

public class ISSCDGridFileUtilities
{

	static public void CreateFile(ISSCBGrid dataSet, string path){
		string d = EncodeJson (dataSet);

		if(!path.EndsWith(".scb")) path += ".scb";
		string directory = path.Substring (0,path.LastIndexOf ("/"));
		if (!Directory.Exists (directory))Directory.CreateDirectory (directory);
		File.WriteAllText (path, d);
	}

	static public ISSCBGrid LoadFromFile(string path){
		if(!path.EndsWith(".scb")) path += ".scb";
		string d = File.ReadAllText (path);
		return DecodeJson (d);
	}

	static string EncodeJson (ISSCBGrid grid){
	
		int[] data = grid.GetRawData ();
	
		JsonData json = new JsonData ();
		JsonData size = new JsonData();
		JsonData blocks = new JsonData();

		ISSCBlockVector bv = grid.gridSize;
		size["x"] = bv.x.ToString();
		size["y"] = bv.y.ToString();
		size["z"] = bv.z.ToString();

		json["name"] = grid.name;
		json["size"] = size;
	
		for (int i = 0; i < data.Length; i++) {
			blocks [i.ToString()] = data [i];
		}
		json["blocks"] = blocks;

		string str = json.ToJson ();
		return str;
	}

	static ISSCBGrid DecodeJson(string data){
		JsonData json = JsonMapper.ToObject (data);

		int vectorX,vectorY,vectorZ;
		vectorX = int.Parse(json["size"]["x"].ToString());
		vectorY = int.Parse(json["size"]["y"].ToString());
		vectorZ = int.Parse(json["size"]["z"].ToString());
		ISSCBlockVector size = new ISSCBlockVector(vectorX,vectorY,vectorZ);

		ISSCBGrid grid = new ISSCBGrid(size);
		grid.name = json ["name"].ToString ();

		for(int i =0 ;i< grid.GetRawData().Length;i++){
			grid.SetBlock(grid.DecodeIndex(i),int.Parse(json["blocks"][i.ToString()].ToString()));//?
		}

		return grid;
	}
	
	static void SaveDataIntoFile (string DataPath, string FileName,string text,string suffixName){
		File.WriteAllText(DataPath+"/"+FileName+"."+suffixName,text);
	}
	
	static JsonData LoadFileAsJson(string DataPath,string FileName){
	
		JsonData json = JsonMapper.ToObject(File.ReadAllText(DataPath+"/"+FileName));
		return json;
	}
	
	static string GetDataPath (string[] dirs){
		string tmpStr ;
	
		string appDir = Application.dataPath;
		
		for(int i = 0 ; i < dirs.Length ; i++){
		 	tmpStr = System.IO.Path.Combine(appDir,dirs[i]);
		 	System.IO.Directory.CreateDirectory(tmpStr);
		 	appDir += "/" + dirs[i];
		}
		
		return appDir;
		
	}
}
