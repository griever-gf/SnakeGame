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
	
	//Direction NextDirection;
	public float lastMovingTime = 0.0f;
	
	void AddSprite(int LinkIndex, tk2dSprite sprite)
	{
		tk2dSprite tmp = Instantiate(sprite, tilemap.GetTilePosition(Links[LinkIndex].x, Links[LinkIndex].y)+new Vector3(tilemap.height, tilemap.height, 0), tilemap.transform.rotation) as tk2dSprite;
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
		uc.y = 1;
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
					AddSprite(i, spriteTurnLeftDown_RightUp);
				else
					AddSprite(i, spriteTurnLeftUp_RightDown);
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
