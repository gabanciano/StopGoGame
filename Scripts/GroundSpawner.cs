using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour {

	public GameObject Ground;

	private float secondsBeforeSpawn = 0;
	readonly double MAX_SPAWN_SECONDS_COUNT = 1.25;

	void Update(){
		SpawnNewGround ();
	}

	void SpawnNewGround(){
		if (GameData.FINGER_DOWN) {
			secondsBeforeSpawn += Time.deltaTime;
			if (secondsBeforeSpawn >= MAX_SPAWN_SECONDS_COUNT) {
				Instantiate (Ground, new Vector3 (transform.position.x, transform.position.y, transform.position.z + -0.18f), transform.rotation);
				secondsBeforeSpawn = 0;
			}
		}
	} 
}
