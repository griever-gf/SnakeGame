using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;

public class ShowRecords : MonoBehaviour {
	
	public string MainSceneName;
	public tk2dTextMesh textmeshResultsNames;
	public tk2dTextMesh textmeshResultsLinks;
	public tk2dTextMesh textmeshResultsMoves;
	
	private string dbPath;
	private IDbConnection dbConnection;
	private IDbCommand dbCommand;
	private IDataReader dbReader;

	// Use this for initialization
	void Start () {
		dbPath = "URI=file:"+ Application.dataPath + "/StreamingAssets/SnakeGameDatabase2.bytes";
		//Debug.Log(dbPath);
		dbConnection=new SqliteConnection(dbPath);
		dbConnection.Open();
		//Debug.Log(dbConnection.State);
		dbCommand=dbConnection.CreateCommand();
		dbCommand.CommandText="SELECT `Player`,`Links`,`Moves` FROM `Records` ORDER BY `Links` DESC,`Moves` DESC LIMIT 5";//"SELECT `id` FROM `npc` WHERE `name`='"+NPCname+"'";
		dbReader=dbCommand.ExecuteReader();   
		while( dbReader.Read()){
			//Debug.Log(dbReader.GetString(0));
			textmeshResultsNames.text += dbReader.GetString(0) +  "\n";
			textmeshResultsLinks.text += dbReader.GetString(1) +  "\n";
			textmeshResultsMoves.text += dbReader.GetString(2) +  "\n";
		}
		dbConnection.Close();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown){
			Application.LoadLevel(MainSceneName);
		}
	}
}
