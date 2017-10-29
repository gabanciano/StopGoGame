using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject[] Enemies;

	private float secondsBeforeSpawn;
	readonly int MAX_SPAWN_SECONDS_COUNT = 1;

	void Update(){
		SpawnEnemy ();
	}

	void SpawnEnemy(){
		if (GameData.FINGER_DOWN) {
			secondsBeforeSpawn += Time.deltaTime;
			if (secondsBeforeSpawn >= MAX_SPAWN_SECONDS_COUNT) {
				Instantiate (Enemies [Random.Range (0, 2)], transform.position, transform.rotation);
				secondsBeforeSpawn = 0;
			}
		}
	}
}
