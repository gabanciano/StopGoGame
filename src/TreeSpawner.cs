using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour {

	public GameObject Tree;

	private float secondsBeforeSpawn = 0;
	private int MAX_SPAWN_SECONDS_COUNT;

	void Update(){
		SpawnTree ();
	}

	void SpawnTree(){
		if (GameData.FINGER_DOWN) {
			MAX_SPAWN_SECONDS_COUNT = Random.Range (1, 3);
			secondsBeforeSpawn += Time.deltaTime;
			if (secondsBeforeSpawn >= MAX_SPAWN_SECONDS_COUNT) {
				Instantiate (Tree, new Vector3 (Random.Range (-3.5f, 3.5f), transform.position.y, transform.position.z + -0.50f), transform.rotation);
				secondsBeforeSpawn = 0;
			}
		}
	}
}
