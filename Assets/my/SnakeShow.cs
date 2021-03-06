﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SnakeState
{
	Sleep, Moving, Death
}

public enum Direction
{
	Up = 1, Down = 2, Left = 0, Right = 3
}

public struct SnakeLink
{
	public int x;
	public int y;
	public SnakeLink(int xi, int yi) 
	{
      this.x = xi;
      this.y = yi;
	}
}

public class SnakeShow : MonoBehaviour {
	public int InitialLength = 5;
	public float InitialDelay = 0.5f;
	public List<SnakeLink> Links;
	public static int FinalLength;
	SnakeState CurrentState;

	Direction CurrentDirection;
	Direction PreviousDirection;
	
	public tk2dTileMap tilemap;

	public int tileIndexHead = 3;
	public int tileIndexBody = 0;
	public int tileIndexTail = 1;
	public int tileIndexTurnLeftUp = 4;
	public int tileIndexTurnLeftDown = 2;
	public int tileIndexTurnRightUp = 6;
	public int tileIndexTurnRightDown = 5;
	public int tileIndexFood = 8;
	
	float lastMovingTime = 0.0f;
	float delayMoving;
	public static int MovesCounter;
	
	public AudioClip soundMove;
	public AudioClip soundSwallow;
	public AudioClip soundPause;
	
	public tk2dTextMesh labelPause;
	
	public string RecordEnterSceneName;
	public string RecordsViewSceneName;
	
	void AddTile(int LinkIndex, int TileIndex)
	{
		tilemap.SetTile(Links[LinkIndex].x, Links[LinkIndex].y, 1, TileIndex);
		
		Direction d = Direction.Right;
		if (LinkIndex == 0) //head
		{
			if (Links[0].x - Links[1].x != 0) //if horizontal
				d = (Links[0].x - Links[1].x > 0) ? Direction.Right : Direction.Left;
			else
				d = (Links[0].y - Links[1].y > 0) ? Direction.Up : Direction.Down;
		}
		else
		{
			if (LinkIndex == Links.Count-1) //tail
			{
				if (Links[LinkIndex].x - Links[LinkIndex-1].x != 0) //if horizontal
					d = (Links[LinkIndex].x - Links[LinkIndex-1].x > 0) ? Direction.Left : Direction.Right;
				else
					d = (Links[LinkIndex].y - Links[LinkIndex-1].y > 0) ? Direction.Down : Direction.Up;
			}
			else
			{
				if (!((Links[LinkIndex-1].x - Links[LinkIndex+1].x != 0)&&(Links[LinkIndex-1].y - Links[LinkIndex+1].y != 0))) //if not turn
				{
					if(Links[LinkIndex-1].x - Links[LinkIndex+1].x == 0) //if vertical
					{
						d = Direction.Up;
					}
				}
			}
		}
		switch (d)
		{
			case Direction.Up:
				tilemap.SetTileFlags(Links[LinkIndex].x, Links[LinkIndex].y, 1, tk2dTileFlags.FlipX | tk2dTileFlags.FlipY | tk2dTileFlags.Rot90);
				break;
			case Direction.Left:
				tilemap.SetTileFlags(Links[LinkIndex].x, Links[LinkIndex].y, 1, tk2dTileFlags.FlipX);
				break;
			case Direction.Down:
				tilemap.SetTileFlags(Links[LinkIndex].x, Links[LinkIndex].y, 1, tk2dTileFlags.FlipX | tk2dTileFlags.Rot90);
				break;
			case Direction.Right:
				tilemap.SetTileFlags(Links[LinkIndex].x, Links[LinkIndex].y, 1, tk2dTileFlags.None);
				break;
		}
	}

	// Use this for initialization
	void Start () {
		GameStart();
		GenerateFood();
	}
	
	void GameStart()
	{
		Links = new List<SnakeLink>();
		SnakeLink uc;
		uc.y = 2;
		for (uc.x = InitialLength; uc.x >= 1; uc.x--)
			Links.Add(uc);
		GenerateSnakeTiles();
		CurrentState = SnakeState.Sleep;
		CurrentDirection = PreviousDirection = Direction.Right;
		delayMoving = InitialDelay;
		MovesCounter = 0;
	}
	
	void GenerateSnakeTiles()
	{
		AddTile(0, tileIndexHead);
		for (int i = 1; i <= Links.Count-2; i++)
		{
			if ((Links[i-1].x - Links[i+1].x != 0)&&(Links[i-1].y - Links[i+1].y != 0)) //if turn
			{
				if ((Links[i-1].x - Links[i+1].x > 0)^(Links[i-1].y - Links[i+1].y > 0))
				{
					if ((Links[i].x - Links[i-1].x > 0)||(Links[i].x - Links[i+1].x > 0))//if one neighbor on the left
						AddTile(i, tileIndexTurnLeftDown);
					else
						AddTile(i, tileIndexTurnRightUp);
				}	
				else
				{
					if ((Links[i].x - Links[i-1].x > 0)||(Links[i].x - Links[i+1].x > 0))//if one neighbor on the left
						AddTile(i, tileIndexTurnLeftUp);
					else
						AddTile(i, tileIndexTurnRightDown);
				}
			}
			else
				AddTile(i, tileIndexBody);
		}
		AddTile(Links.Count-1, tileIndexTail);
		tilemap.Build();
	}
	
	void GenerateFood()
	{
		System.Random rnd = new System.Random();
		int x, y;
		do
		{
			x = rnd.Next(0,tilemap.width);
			y = rnd.Next(0,tilemap.height);
		}
		while(tilemap.GetTile(x, y, 1) != -1);
		tilemap.SetTile(x, y, 1, tileIndexFood);
		tilemap.Build();
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentState != SnakeState.Death)
		{
			//if ((Input.anyKeyDown)&&(CurrentState == SnakeState.Sleep))
				//CurrentState = SnakeState.Moving;
				
			if (Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W))
				CurrentDirection = Direction.Up;
			if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S))
				CurrentDirection = Direction.Down;
			if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A))
				CurrentDirection = Direction.Left;
			if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D))
				CurrentDirection = Direction.Right;
			
			if (CurrentState == SnakeState.Moving)
				if (Time.time > lastMovingTime + delayMoving)
				{
					Move();
					lastMovingTime = Time.time;
				}
		}
		else
		{
			if (Input.anyKeyDown)
			{
				GameStart();
			}
		}
	}
	
	public void Move()
	{
		MovesCounter++;
		
		if ((int)PreviousDirection + (int)CurrentDirection == 3) // check for opposite direction
			CurrentDirection = PreviousDirection;

		switch (CurrentDirection)		
		{
			case Direction.Right:
				Links.Insert(0, new SnakeLink(Links[0].x+1, Links[0].y));
				break;
			case Direction.Left:
				Links.Insert(0, new SnakeLink(Links[0].x-1, Links[0].y));
				break;
			case Direction.Up:
				Links.Insert(0, new SnakeLink(Links[0].x, Links[0].y+1));
				break;
			case Direction.Down:
				Links.Insert(0, new SnakeLink(Links[0].x, Links[0].y-1));
				break;
		}
		PreviousDirection = CurrentDirection;
		
		if (tilemap.GetTile(Links[0].x, Links[0].y, 1) == -1) //if next tilemap is clear
		{
			tilemap.ClearTile(Links[Links.Count-1].x, Links[Links.Count-1].y, 1);
			Links.RemoveAt(Links.Count-1); //remove tail
			AudioSource.PlayClipAtPoint(soundMove, transform.position);
		}
		else
		{
			if (tilemap.GetTile(Links[0].x, Links[0].y, 1) == tileIndexFood) // grow snake
			{
				GenerateFood();
				delayMoving *= 0.9f;
				AudioSource.PlayClipAtPoint(soundSwallow, transform.position);
			}
			else //game over
			{
				GameObject go = GameObject.Find("DestroySound");
				ObjectSaver os = go.GetComponent<ObjectSaver>();
				os.PlayGameOver();
				Links.RemoveAt(0);
				Debug.Log("GAME OVER! Links: " + Links.Count + " Moves: " + MovesCounter);
				CurrentState = SnakeState.Death;
				FinalLength = Links.Count;
				foreach (SnakeLink link in Links)
					tilemap.ClearTile(link.x, link.y, 1);
				tilemap.Build();
				SwitchToRecordEnter();
			}
		}

		if (CurrentState != SnakeState.Death)
			GenerateSnakeTiles();
	}
	
	//external messages receivers
	void StartButtonHandler()
	{
		if (CurrentState != SnakeState.Moving)
		{
			CurrentState = SnakeState.Moving;
			labelPause.text = "";
		}
		else
		{
			CurrentState = SnakeState.Sleep;
			labelPause.text = "pause";
			AudioSource.PlayClipAtPoint(soundPause, transform.position);
		}
	}
	
	void SnakeMoveUp()
	{
		if (CurrentState == SnakeState.Moving)
			CurrentDirection = Direction.Up;
	}
	
	void SnakeMoveDown()
	{
		if (CurrentState == SnakeState.Moving)
			CurrentDirection = Direction.Down;
	}
	
	void SnakeMoveLeft()
	{
		if (CurrentState == SnakeState.Moving)
			CurrentDirection = Direction.Left;
	}
	
	void SnakeMoveRight()
	{
		if (CurrentState == SnakeState.Moving)
			CurrentDirection = Direction.Right;
	}
	
	void SwitchToRecordsView()
	{
		Application.LoadLevel(RecordsViewSceneName);
	}
	
	void SwitchToRecordEnter()
	{
		Application.LoadLevel(RecordEnterSceneName);
	}
}
