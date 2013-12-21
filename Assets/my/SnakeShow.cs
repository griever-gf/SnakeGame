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

public struct SnakeSprite
{
	public Vector3 position;
	public tk2dSprite sprite;
	//public GameObject obj;
	public SnakeSprite(Vector3 pos, tk2dSprite sp)
	{
		position = pos;
		sprite = sp;
	}
}

public class Snake
{
	const int InitialLength = 5;
	public List<SnakeLink> Links;
	public Direction CurrentDirection;
	public Direction PreviousDirection;
	public int CurrentLength = InitialLength;
	public SnakeState CurrentState;

	public Snake()
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

public class SnakeShow : MonoBehaviour {
	
	Snake mySnake;
	public tk2dTileMap tilemap;
	//public tk2dSpriteCollection SnakeSprites;
	//int tmIndexBody = 1;
	//int tmIndexHead = 2;
	//int tmIndexTail = 3;
	//int tmIndexTurnLeftUp_RightDown = 5;
	//int tmIndexTurnLeftDown_RightUp = 4;
	public tk2dSprite spriteHead;
	public tk2dSprite spriteBody;
	public tk2dSprite spriteTail;
	public tk2dSprite spriteTurnLeftUp_RightDown;
	public tk2dSprite spriteTurnLeftDown_RightUp;
	public List<SnakeSprite> SnakeSprites;
	
	Direction NextDirection;
	
	public float lastMovingTime = 0.0f;
	//public float SnakeSpeed = 10.0f;

	// Use this for initialization
	void Start () {
		mySnake = new Snake();
		NextDirection = mySnake.CurrentDirection;
		//tk2dSprite sprite = SnakeSprites.spriteCollection.spriteDefinitions[0].
		
		//filling snake sprites list
		SnakeSprites = new List<SnakeSprite>();
		tk2dSprite tmpSprite;
		tmpSprite = spriteHead;
		SnakeSprites.Add( new SnakeSprite(tilemap.GetTilePosition(mySnake.Links[0].x, mySnake.Links[0].y)+new Vector3(tilemap.height, tilemap.height, 0), spriteHead));
		for (int i = 1; i < mySnake.CurrentLength-1; i++)
		{
			if ((mySnake.Links[i-1].x-mySnake.Links[i+1].x!=0)&&(mySnake.Links[i-1].y-mySnake.Links[i+1].y!=0)) //if turn
			{
				if ((mySnake.Links[i-1].x-mySnake.Links[i+1].x>0)^(mySnake.Links[i-1].y-mySnake.Links[i+1].y>0))
					//tilemap.SetTile(mySnake.Links[i].x, mySnake.Links[i].y, 0, tmIndexTurnLeftDown_RightUp);
					SnakeSprites.Add( new SnakeSprite(tilemap.GetTilePosition(mySnake.Links[i].x, mySnake.Links[i].y)+new Vector3(tilemap.height, tilemap.height, 0), spriteTurnLeftDown_RightUp));
				else
					//tilemap.SetTile(mySnake.Links[i].x, mySnake.Links[i].y, 0, tmIndexTurnLeftUp_RightDown);
					SnakeSprites.Add( new SnakeSprite(tilemap.GetTilePosition(mySnake.Links[i].x, mySnake.Links[i].y)+new Vector3(tilemap.height, tilemap.height, 0), spriteTurnLeftUp_RightDown));
			}
			else
				//tilemap.SetTile(mySnake.Links[i].x, mySnake.Links[i].y, 0, tmIndexBody);
				SnakeSprites.Add( new SnakeSprite(tilemap.GetTilePosition(mySnake.Links[i].x, mySnake.Links[i].y)+new Vector3(tilemap.height, tilemap.height, 0), spriteBody));
		}
		//tilemap.SetTile(mySnake.Links[mySnake.CurrentLength-1].x, mySnake.Links[mySnake.CurrentLength-1].y, 0, tmIndexTail);
		SnakeSprites.Add( new SnakeSprite(tilemap.GetTilePosition(mySnake.Links[mySnake.CurrentLength-1].x, mySnake.Links[mySnake.CurrentLength-1].y)+new Vector3(tilemap.height, tilemap.height, 0), spriteTail));
		
		//tmpSprite.
		//GameObject bullet = Instantiate(tmpSprite,, tilemap.transform.rotation) as GameObject;
		GameObject bullet;
		for (int j=0; j < SnakeSprites.Count; j++)
			bullet = Instantiate(SnakeSprites[j].sprite, SnakeSprites[j].position, tilemap.transform.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (mySnake.CurrentState != SnakeState.Death)
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
			//tilemap.ClearTile(mySnake.Links[mySnake.Links.Count-1].x, mySnake.Links[mySnake.Links.Count-1].y, 0);
			//mySnake.Move();
			//lastMovingTime = Time.time;	
		}
	}
	
	void OnCollisionEnter(Collision c)
	{
		Debug.Log("collision!");
	}
}
