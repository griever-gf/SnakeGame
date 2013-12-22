using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {

	void OnCollisionEnter(Collision c)
	{
		//if (c.collider.name.Contains("prefabBullet"))
		{
			Debug.Log("collider " + c.collider.name);
		}
	}
}

