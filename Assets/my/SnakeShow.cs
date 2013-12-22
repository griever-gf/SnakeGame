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
	public tk2dSprite spriteTurnLeftUp;
	public tk2dSprite spriteTurnLeftDown;
	public tk2dSprite spriteTurnRightUp;
	public tk2dSprite spriteTurnRightDown;
	
	//Direction NextDirection;
	public float lastMovingTime = 0.0f;
	
	void AddSprite(int LinkIndex, tk2dSprite sprite)
	{
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
			if (LinkIndex == CurrentLength-1) //tail
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

		//tk2dSprite tmp = Instantiate(sprite, tilemap.GetTilePosition(Links[LinkIndex].x, Links[LinkIndex].y)+new Vector3(tilemap.height, tilemap.height, 0), tilemap.transform.rotation) as tk2dSprite;
		tk2dSprite tmp = Instantiate(sprite, tilemap.GetTilePosition(Links[LinkIndex].x, Links[LinkIndex].y)+new Vector3(16, 16, 0), tilemap.transform.rotation) as tk2dSprite;
		switch (d)
		{
			case Direction.Up:
				tmp.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(1, 0) * Mathf.Rad2Deg);
				break;
			case Direction.Left:
				tmp.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(0, -1) * Mathf.Rad2Deg);
				break;
			case Direction.Down:
				tmp.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-1, 0) * Mathf.Rad2Deg);
				break;
		}
		Sprites.Add(tmp.gameObject);
	}
	
	void InsertSprite(int position, int LinkIndex, tk2dSprite sprite)
	{
		//tk2dSprite tmp = Instantiate(sprite, tilemap.GetTilePosition(Links[LinkIndex].x, Links[LinkIndex].y)+new Vector3(tilemap.height, tilemap.height, 0), tilemap.transform.rotation) as tk2dSprite;
		//Sprites.Add(tmp.gameObject);
	}
	
	void ShowNewDiscretePosition()
	{
		foreach (GameObject go in Sprites)
			Destroy(go);
		GenerateSnakeSprites();
	}

	// Use this for initialization
	void Start () {
		Links = new List<SnakeLink>();
		SnakeLink uc;
		uc.y = 2;
		for (uc.x = InitialLength; uc.x >= 1; uc.x--)
			Links.Add(uc);
		GenerateSnakeSprites();
		CurrentState = SnakeState.Sleep;
		CurrentDirection = PreviousDirection = Direction.Right;
	}
	
	void GenerateSnakeSprites()
	{
		Sprites = new List<GameObject>();
		AddSprite(0, spriteHead);
		for (int i = 1; i < CurrentLength-1; i++)
		{
			if ((Links[i-1].x - Links[i+1].x != 0)&&(Links[i-1].y - Links[i+1].y != 0)) //if turn
			{
				if ((Links[i-1].x - Links[i+1].x > 0)^(Links[i-1].y - Links[i+1].y > 0))
				{
					if ((Links[i].x - Links[i-1].x > 0)||(Links[i].x - Links[i+1].x > 0))//if one neighbor on the left
						AddSprite(i, spriteTurnLeftDown);
					else
						AddSprite(i, spriteTurnRightUp);
				}	
				else
				{
					if ((Links[i].x - Links[i-1].x > 0)||(Links[i].x - Links[i+1].x > 0))//if one neighbor on the left
						AddSprite(i, spriteTurnLeftUp);
					else
						AddSprite(i, spriteTurnRightDown);
				}
			}
			else
				AddSprite(i, spriteBody);
		}
		AddSprite(CurrentLength-1, spriteTail);
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentState != SnakeState.Death)
		{
			if ((Input.anyKeyDown)&&(CurrentState == SnakeState.Sleep))
				CurrentState = SnakeState.Moving;
				
			if (Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W))
				CurrentDirection = Direction.Up;
			if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S))
				CurrentDirection = Direction.Down;
			if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A))
				CurrentDirection = Direction.Left;
			if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D))
				CurrentDirection = Direction.Right;
			
			if (CurrentState == SnakeState.Moving)
				if (Time.time > lastMovingTime + 0.5f)
				{
					Move();
					lastMovingTime = Time.time;
				}
		}
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
		
		ShowNewDiscretePosition();
	}
}
