using UnityEngine;
using System.Collections;

public class ObjectSaver : MonoBehaviour {
	
	public AudioClip soundGameOver;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}
	
	public void PlayGameOver() {
		AudioSource.PlayClipAtPoint(soundGameOver, transform.position);
	}
}
