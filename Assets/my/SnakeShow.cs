using UnityEngine;
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
	
	public SnakeState CurrentState;
	const int InitialLength = 5;
	public int CurrentLength = InitialLength;
	public List<SnakeLink> Links;

	public Direction CurrentDirection;
	public Direction PreviousDirection;
	
	public List<GameObject> Sprites;
	public tk2dTileMap tilemap;
	
	public tk2dSprite spriteHead;
	public tk2dSprite spriteBody;
	public tk2dSprite spriteTail;
	public tk2dSprite spriteTurnLeftUp_RightDown;
	public tk2dSprite spriteTurnLeftDown_RightUp;
	
	Direction NextDirection;
	public float lastMovingTime = 0.0f;

	public SnakeShow()
	{
		CurrentDirection = PreviousDirection = Direction.Right;
	
		//CurrentState = SnakeState.Sleep;
		CurrentState = SnakeState.Moving;
		Links = new List<SnakeLink>();
		SnakeLink uc;
		uc.y = 0;
		for (uc.x = InitialLength-1; uc.x >= 0; uc.x--)
			Links.Add(uc);
	}

	// Use this for initialization
	void Start () {
		//filling initial sprites list
		Sprites = new List<GameObject>();
		Sprites.Add(Instantiate(spriteHead, tilemap.GetTilePosition(Links[0].x, Links[0].y)+new Vector3(tilemap.height, tilemap.height, 0), tilemap.transform.rotation) as GameObject);
		NextDirection = CurrentDirection;

		for (int i = 1; i < CurrentLength-1; i++)
		{
			if ((Links[i-1].x - Links[i+1].x != 0)&&(Links[i-1].y - Links[i+1].y != 0)) //if turn
			{
				if ((Links[i-1].x - Links[i+1].x > 0)^(Links[i-1].y - Links[i+1].y > 0))
					Sprites.Add(Instantiate(spriteTurnLeftDown_RightUp, tilemap.GetTilePosition(Links[i].x, Links[i].y)+new Vector3(tilemap.height, tilemap.height, 0), tilemap.transform.rotation) as GameObject);
				else
					Sprites.Add(Instantiate(spriteTurnLeftUp_RightDown, tilemap.GetTilePosition(Links[i].x, Links[i].y)+new Vector3(tilemap.height, tilemap.height, 0), tilemap.transform.rotation) as GameObject);
			}
			else
				Sprites.Add(Instantiate(spriteBody, tilemap.GetTilePosition(Links[i].x, Links[i].y)+new Vector3(tilemap.height, tilemap.height, 0), tilemap.transform.rotation) as GameObject);
		}
		Sprites.Add(Instantiate(spriteTail, tilemap.GetTilePosition(Links[CurrentLength-1].x, Links[CurrentLength-1].y)+new Vector3(tilemap.height, tilemap.height, 0), tilemap.transform.rotation) as GameObject);
	}
	
	// Update is called once per frame
	void Update () {
		//if (mySnake.CurrentState != SnakeState.Death)
		{
			//tilemap.SetTile(mySnake.Links[0].x, mySnake.Links[0].y, 0, tmIndexHead);

			/*for (int i = 1; i < mySnake.CurrentLength-1; i++)
			{
				if ((mySnake.Links[i-1].x-mySnake.Links[i+1].x!=0)&&(mySnake.Links[i-1].y-mySnake.Links[i+1].y!=0)) //if turn
				{
					if ((mySnake.Links[i-1].x-mySnake.Links[i+1].x>0)^(mySnake.Links[i-1].y-mySnake.Links[i+1].y>0))
						tilemap.SetTile(mySnake.Links[i].x, mySnake.Links[i].y, 0, tmIndexTurnLeftDown_RightUp);
					else
						tilemap.SetTile(mySnake.Links[i].x, mySnake.Links[i].y, 0, tmIndexTurnLeftUp_RightDown);
				}
				else
					tilemap.SetTile(mySnake.Links[i].x, mySnake.Links[i].y, 0, tmIndexBody);
			}
			tilemap.SetTile(mySnake.Links[mySnake.CurrentLength-1].x, mySnake.Links[mySnake.CurrentLength-1].y, 0, tmIndexTail);
			tilemap.Build();*/
		}
		//if ((Input.anyKeyDown)&&(mySnake.CurrentState == SnakeState.Sleep))
		//	mySnake.CurrentState = SnakeState.Moving;
		//if (Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W))
		//	mySnake.CurrentDirection = Direction.Up;
		//if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S))
		//	mySnake.CurrentDirection = Direction.Down;
		//if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A))
		//	mySnake.CurrentDirection = Direction.Left;
		//if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D))
		//	mySnake.CurrentDirection = Direction.Right;
		
		//if (Time.time > lastMovingTime + 0.5f)
		//{
			//tilemap.ClearTile(mySnake.Links[mySnake.Links.Count-1].x, mySnake.Links[mySnake.Links.Count-1].y, 0);
			//mySnake.Move();
			//lastMovingTime = Time.time;	
		//}
	}
	
	public void Move()
	{
		if ((int)PreviousDirection + (int)CurrentDirection == 3) // check for opposite direction
			CurrentDirection = PreviousDirection;
		
		Links.RemoveAt(Links.Count-1); //remove tail
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
	}
}
