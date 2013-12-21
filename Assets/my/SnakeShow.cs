using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum SnakeState
{
	Sleep, Moving, Death
}

enum Direction
{
	Up = 1, Down = 2, Left = 0, Right = 3
}

struct UnitCoords
{
	public int x;
	public int y;
	public UnitCoords(int xi, int yi) 
	{
      this.x = xi;
      this.y = yi;
	}
}

class Snake
{
	const int InitialLength = 5;
	public List<UnitCoords> UnitList;
	public Direction CurrentDirection;
	public Direction PreviousDirection;
	public int CurrentLength = InitialLength;
	public SnakeState CurrentState;

	public Snake()
	{
		CurrentDirection = PreviousDirection = Direction.Right;
	
		//CurrentState = SnakeState.Sleep;
		CurrentState = SnakeState.Moving;
		UnitList = new List<UnitCoords>();
		UnitCoords uc;
		uc.y = 0;
		for (uc.x = InitialLength-1; uc.x >= 0; uc.x--)
			UnitList.Add(uc);
	}
	
	public void Move()
	{
		if ((int)PreviousDirection + (int)CurrentDirection == 3) // check for opposite direction
			CurrentDirection = PreviousDirection;
		
		UnitList.RemoveAt(UnitList.Count-1); //remove tail
		switch (CurrentDirection)		
		{
			case Direction.Right:
				UnitList.Insert(0, new UnitCoords(UnitList[0].x+1, UnitList[0].y));
				break;
			case Direction.Left:
				UnitList.Insert(0, new UnitCoords(UnitList[0].x-1, UnitList[0].y));
				break;
			case Direction.Up:
				UnitList.Insert(0, new UnitCoords(UnitList[0].x, UnitList[0].y+1));
				break;
			case Direction.Down:
				UnitList.Insert(0, new UnitCoords(UnitList[0].x, UnitList[0].y-1));
				break;
		}
		PreviousDirection = CurrentDirection;
	}
}

public class SnakeShow : MonoBehaviour {
	
	Snake mySnake;
	public tk2dTileMap tilemap;
	public tk2dSpriteCollection SnakeSprites;
	int tmIndexBody = 1;
	int tmIndexHead = 2;
	int tmIndexTail = 3;
	int tmIndexTurnLeftUp_RightDown = 5;
	int tmIndexTurnLeftDown_RightUp = 4;
	Direction NextDirection;
	
	public float lastMovingTime = 0.0f;
	//public float SnakeSpeed = 10.0f;

	// Use this for initialization
	void Start () {
		mySnake = new Snake();
		NextDirection = mySnake.CurrentDirection;
	}
	
	// Update is called once per frame
	void Update () {
		if (mySnake.CurrentState != SnakeState.Death)
		{
			tilemap.SetTile(mySnake.UnitList[0].x, mySnake.UnitList[0].y, 0, tmIndexHead);
			for (int i = 1; i < mySnake.CurrentLength-1; i++)
			{
				if ((mySnake.UnitList[i-1].x-mySnake.UnitList[i+1].x!=0)&&(mySnake.UnitList[i-1].y-mySnake.UnitList[i+1].y!=0)) //if turn
				{
					if ((mySnake.UnitList[i-1].x-mySnake.UnitList[i+1].x>0)^(mySnake.UnitList[i-1].y-mySnake.UnitList[i+1].y>0))
						tilemap.SetTile(mySnake.UnitList[i].x, mySnake.UnitList[i].y, 0, tmIndexTurnLeftDown_RightUp);
					else
						tilemap.SetTile(mySnake.UnitList[i].x, mySnake.UnitList[i].y, 0, tmIndexTurnLeftUp_RightDown);
				}
				else
					tilemap.SetTile(mySnake.UnitList[i].x, mySnake.UnitList[i].y, 0, tmIndexBody);
			}
			tilemap.SetTile(mySnake.UnitList[mySnake.CurrentLength-1].x, mySnake.UnitList[mySnake.CurrentLength-1].y, 0, tmIndexTail);
			tilemap.Build();
		}
		if ((Input.anyKeyDown)&&(mySnake.CurrentState == SnakeState.Sleep))
			mySnake.CurrentState = SnakeState.Moving;
		if (Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W))
			mySnake.CurrentDirection = Direction.Up;
		if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S))
			mySnake.CurrentDirection = Direction.Down;
		if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A))
			mySnake.CurrentDirection = Direction.Left;
		if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D))
			mySnake.CurrentDirection = Direction.Right;
		
		if (Time.time > lastMovingTime + 0.5f)
		{
			tilemap.ClearTile(mySnake.UnitList[mySnake.UnitList.Count-1].x, mySnake.UnitList[mySnake.UnitList.Count-1].y, 0);
			mySnake.Move();
			lastMovingTime = Time.time;	
		}
	}
	
	void OnCollisionEnter(Collision c)
	{
		Debug.Log("collision!");
	}
}
