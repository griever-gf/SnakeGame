using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Mono.Data.SqliteClient;
//using SQLite;

public class Record
{
    //[PrimaryKey, AutoIncrement]
    //public int Id { get; set; }
    public string Player { get; set; }
	public int Links { get; set; }
	public int Moves { get; set; }
}

public class ShowRecords : MonoBehaviour {
	
	public string MainSceneName;
	public tk2dTextMesh textmeshResultsNames;
	public tk2dTextMesh textmeshResultsLinks;
	public tk2dTextMesh textmeshResultsMoves;
	
	private string dbPath;

	// Use this for initialization
	void Start () {
		//Windows
		dbPath = "URI=file:"+ Application.dataPath + "/StreamingAssets/SnakeGameDatabase2.bytes";
		
		//Debug.Log(dbPath);
		IDbConnection dbConnection=new SqliteConnection(dbPath);
		dbConnection.Open();
		//Debug.Log(dbConnection.State);
		IDbCommand dbCommand=dbConnection.CreateCommand();
		dbCommand.CommandText="SELECT `Player`,`Links`,`Moves` FROM `Records` ORDER BY `Links` DESC,`Moves` ASC LIMIT 5";//"SELECT `id` FROM `npc` WHERE `name`='"+NPCname+"'";
		IDataReader dbReader=dbCommand.ExecuteReader();   
		while( dbReader.Read()){
			//Debug.Log(dbReader.GetString(0));
			textmeshResultsNames.text += dbReader.GetString(0) +  "\n";
			textmeshResultsLinks.text += dbReader.GetString(1) +  "\n";
			textmeshResultsMoves.text += dbReader.GetString(2) +  "\n";
		}
		dbConnection.Close();
		
		//Android
		/*string dbExtractPath = "jar:file://" + Application.dataPath + "!/assets/SnakeGameDatabase2.bytes";
		dbPath = Application.persistentDataPath + "/SnakeGameDatabase2.bytes";
		WWW www = new WWW(dbExtractPath);
		while(!www .isDone) {} // тут очень внимательно, используйте корутины
			File.WriteAllBytes(dbPath, www.bytes);
		
		var db = new SQLiteConnection(dbPath, false);
		
		List<Record> QueryVals = db.Query<Record>("SELECT `Player`,`Links`,`Moves` FROM `Records` ORDER BY `Links` DESC,`Moves` DESC LIMIT 5");
		for (int i=0; i<QueryVals.Count; i++)
		{
			textmeshResultsNames.text += QueryVals[i].Player +  "\n";
			textmeshResultsLinks.text += QueryVals[i].Links +  "\n";
			textmeshResultsMoves.text += QueryVals[i].Moves +  "\n";
		}
		db.Close();*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown){
			Application.LoadLevel(MainSceneName);
		}
	}
}
