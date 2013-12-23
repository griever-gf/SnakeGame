using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;

public class AddResult : MonoBehaviour {
	
	public tk2dUITextInput textinput;
	public tk2dTextMesh resultinfo;
	
	public string RecordsViewSceneName;

	// Use this for initialization
	void Start () {
		resultinfo.text = SnakeShow.FinalLength + " Links, " + SnakeShow.MovesCounter + " Moves";
	}

	void InputAndSwitch(){
		string dbPath = "URI=file:"+ Application.dataPath + "/StreamingAssets/SnakeGameDatabase2.bytes";
		SqliteConnection dbConnection=new SqliteConnection(dbPath);
		
		string Query = "INSERT INTO Records (Player,Links,Moves) VALUES('"+ textinput.Text +"'," + SnakeShow.FinalLength.ToString() +"," + SnakeShow.MovesCounter.ToString()+")";
		IDbCommand dbCommand=new SqliteCommand(Query, dbConnection);
		dbCommand.Connection.Open();
		dbCommand.ExecuteNonQuery();
		
		dbConnection.Close();

		Application.LoadLevel(RecordsViewSceneName);
	}
}
