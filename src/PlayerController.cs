using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	readonly int PLAYER_DEATH_TIME_IF_IDLE = 4;
	public GameUIManager GameUI;

	public float playerSpeed;
	public float playerIdle;

	Vector3 PlayerVelocity;

	public Camera MainCamera;
	public Rigidbody2D PlayerRigid2D;


	void Awake(){
		playerIdle = 0;
		PlayerVelocity = new Vector3(0, playerSpeed, 0);	
	}

	void Update(){
		MovePlayerForward ();
		CheckPlayerIfIdle ();
	}

    public void MovePlayerForward(){
		if (GameData.FINGER_DOWN) {
			PlayerRigid2D.velocity = PlayerVelocity * Time.deltaTime;
			playerIdle = 0;
		}
	}

	void CheckPlayerIfIdle(){
		if (GameData.GAME_STARTED) {
			playerIdle += Time.deltaTime;
			if (playerIdle >= 2) {
				GameUI.NoiseIndicatorImage.gameObject.SetActive (true);
			} else if (playerIdle >= PLAYER_DEATH_TIME_IF_IDLE) {
				StartCoroutine (GameUI.ShowDeathScreen (3));
			} else if (playerIdle < 2) {
				GameUI.NoiseIndicatorImage.gameObject.SetActive (false);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D enter){
		if (enter.gameObject.tag == "LightObstacle") {
			StartCoroutine (GameUI.ShowDeathScreen (1));
		}
		if (enter.gameObject.tag == "PowerAntiNoise") {
			Destroy (enter.gameObject);
			StartCoroutine (ActivatePowerAntiNoise ());
			if (PlayerPrefs.GetInt ("A_MORE_POWER_ACTIVE", 0) != 1) {
				PlayerPrefs.SetInt ("A_MORE_POWER_COUNT", PlayerPrefs.GetInt ("A_MORE_POWER_COUNT", 0) + 1);
				TrackAchievement ();
			} 
			if (PlayerPrefs.GetInt ("A_SILENCE_ACTIVE", 0) != 1) {
				PlayerPrefs.SetInt ("A_SILENCE_COUNT", PlayerPrefs.GetInt ("A_SILENCE_COUNT", 0) + 1); 
				TrackAchievement ();
			}
			Debug.Log (PlayerPrefs.GetInt ("A_MORE_POWER_COUNT", 0));
			Debug.Log (PlayerPrefs.GetInt ("A_SILENCE_COUNT", 0));
		}
		if (enter.gameObject.tag == "Orb") {
			GetComponent<AudioSource> ().Play ();
			GameData.GAME_TOTAL_ORBS++;
			PlayerPrefs.SetInt ("TOTAL_GAME_ORB", PlayerPrefs.GetInt ("TOTAL_GAME_ORB") + 1);
			Destroy (enter.gameObject);
		}
		if (enter.gameObject.tag == "PowerX2Multiplier") {
			Destroy (enter.gameObject);
			PlayerPrefs.SetInt ("STORE_X2BOOSTER_ACTIVE", 0);
			StartCoroutine (ActivatePowerX2Multiplier ());
			if (PlayerPrefs.GetInt ("A_MORE_POWER_ACTIVE", 0) != 1) {
				PlayerPrefs.SetInt ("A_MORE_POWER_COUNT", PlayerPrefs.GetInt ("A_MORE_POWER_COUNT", 0) + 1);
				TrackAchievement ();
			} 
			if (PlayerPrefs.GetInt ("A_DOUBLE_ACTIVE", 0) != 1) {
				PlayerPrefs.SetInt ("A_DOUBLE_COUNT", PlayerPrefs.GetInt ("A_DOUBLE_COUNT", 0) + 1);
				TrackAchievement ();
			}
			Debug.Log (PlayerPrefs.GetInt ("A_MORE_POWER_COUNT", 0));
			Debug.Log (PlayerPrefs.GetInt ("A_DOUBLE_COUNT", 0));
		}
	}

	IEnumerator ActivatePowerX2Multiplier(){
		GameData.POWER_X2_MULTIPLIER_ACTIVE = true;
		yield return new WaitForSeconds (GameData.TIME_POWER_X2_MULTIPLIER);
		GameData.POWER_X2_MULTIPLIER_ACTIVE = false;
	}

	IEnumerator ActivatePowerAntiNoise(){
		GameData.POWER_ANTI_NOISE_ACTIVE = true;
		yield return new WaitForSeconds (GameData.TIME_POWER_ANTI_NOISE);
		GameData.POWER_ANTI_NOISE_ACTIVE = false;
	}

	public void TrackAchievement(){
		//I NEED MORE POWER ACHIEVEMENT
		if (PlayerPrefs.GetInt ("A_MORE_POWER_ACTIVE", 0) != 1) {
			if (PlayerPrefs.GetInt ("A_MORE_POWER_COUNT", 0) >= 10) {
				PlayerPrefs.SetInt ("A_MORE_POWER_ACTIVE", 1);
				GameData.ACHIEVEMENT_MORE_POWER = true;
				GameUI.UpdateAndShowAchievementsNotificationBox ();
			}
		}

		//WALKATHON ACHIEVEMENT
		if (PlayerPrefs.GetInt ("A_WALKATHON_ACTIVE", 0) != 1) {
			if (PlayerPrefs.GetInt ("A_WALKATHON_TOTAL", 0) >= 250000) {
				PlayerPrefs.SetInt ("A_WALKATHON_ACTIVE", 1);
				GameData.ACHIEVEMENT_WALKATHON = true;
				GameUI.UpdateAndShowAchievementsNotificationBox ();
			}
		}

		//SILENCE PLEASE ACHIEVEMENT
		if (PlayerPrefs.GetInt ("A_SILENCE_ACTIVE", 0) != 1) {
			if (PlayerPrefs.GetInt ("A_SILENCE_COUNT", 0) >= 25) {
				PlayerPrefs.SetInt ("A_SILENCE_ACTIVE", 1);
				GameData.ACHIEVEMENT_SILENCE = true;
				GameUI.UpdateAndShowAchievementsNotificationBox ();
			}
		}

		//DOUBLE DOUBLE ACHIEVEMENT
		if (PlayerPrefs.GetInt ("A_DOUBLE_ACTIVE", 0) != 1) {
			if (PlayerPrefs.GetInt ("A_DOUBLE_COUNT", 0) >= 10) {
				PlayerPrefs.SetInt ("A_DOUBLE_ACTIVE", 1);
				GameData.ACHIEVEMENT_DOUBLE = true;
				GameUI.UpdateAndShowAchievementsNotificationBox ();
			}
		}

		//MILLIONAIRE ACHIEVEMENT
		if (PlayerPrefs.GetInt ("A_MILLION_ACTIVE", 0) != 1) {
			if (PlayerPrefs.GetInt ("A_MILLION_TOTAL", 0) >= 1000000) {
				PlayerPrefs.SetInt ("A_MILLION_ACTIVE", 1);
				GameData.ACHIEVEMENT_MILLION = true;
				GameUI.UpdateAndShowAchievementsNotificationBox ();
			}
		}


	}
}
