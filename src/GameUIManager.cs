using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameUIManager : MonoBehaviour {

	[Header("Game Elements")]
	public PlayerController Player;
	public GameObject X2BoosterPowerUp;
	public GameObject AntiNoiseBoosterPowerUp;
	[Space]

	int scoreIncrease;
	readonly float UI_FADE_OUT_DELAY = 7f;

	readonly int NOISE_FILL_250 = 250;
	readonly int NOISE_FILL_300 = 300;
	readonly int NOISE_FILL_375 = 375;
	readonly int NOISE_FULL = 500;

	readonly int GAME_BASE_SCORE = 25;
	readonly int GAME_NEXT1_SCORE = 50;
	readonly int GAME_NEXT2_SCORE = 75;
	readonly int GAME_NEXT3_SCORE = 100;

	readonly int NOISE_DECREASING_VALUE = 180;
	readonly int NOISE_BOOTS_DECREASING_VALUE = 300;
	readonly int NOISE_MAX_VALUE = 500;
	readonly float NOISE_PLAYER_INCREASE_VALUE = 10;

	readonly int STORE_PRICE_BOOSTER_POWER = 25;
	readonly int STORE_PRICE_BOOSTER_BOOTS = 50;

	[Header("Store")]
	public Text StoreOrbsText;

	public Image StoreButton;
	public Image StoreMenu;
	[Space]
	public Image Item1BuyButton;
	public Text Item1BuyButtonText;

	public Image Item2BuyButton;
	public Text Item2BuyButtonText;

	public Image BootsBooster;
	[Space]

	[Header("Achievements")]
	public Image AchievementNotifBox;
	public Image AchievementNBIcon;
	public Text AchievementNBTitle;
	public Image AchievementButton;

	public Image AchievementsMenu;
	public Image AchievementBox1;
	public Image AchievementBox2;
	public Image AchievementBox3;
	public Image AchievementBox4;
	public Image AchievementBox5;

	public Image A1Image;
	public Image A2Image;
	public Image A3Image;
	public Image A4Image;
	public Image A5Image;

	public Slider A1ProgressBar;
	public Slider A2ProgressBar;
	public Slider A3ProgressBar;
	public Slider A4ProgressBar;
	public Slider A5ProgressBar;

	public Sprite AIconLocked;
	public Sprite A1Icon;
	public Sprite A2Icon;
	public Sprite A3Icon;
	public Sprite A4Icon;
	public Sprite A5Icon;
	[Space]

	public float noiseCurrentValue;
	public Image NoiseFill;

	public Image DeathSkullAnim;
	public Image DeathScreen;
	public Image NoiseIndicatorImage;

	public Image Power2XTopImage;
	public Image PowerNoiseTopImage;
	public Slider Power2XSlider;
	public Slider PowerNoiseSlider;

	public TextMesh HighScoreText;
	public Text ScoreText;
	public Text OrbText;
	public Text FinalScore;
	public Text DeathDescriptionText;

	public Slider NoiseMeter;

	void Awake(){
		InitGameData ();
		InitStoreTransactions ();
		InitAchievements ();
	}

	void Start(){
		IncreaseAchievementProgress ();
		GameData.GAME_TOTAL_ORBS = PlayerPrefs.GetInt ("TOTAL_GAME_ORB", 0);
		HighScoreText.text = PlayerPrefs.GetInt ("Game.HighScore").ToString ();
	}

	void Update(){
		UpdateScoreView ();
		UpdateOrbView ();
		UpdateNoiseMeter ();
		UpdatePower2XTimerMeter ();
		UpdatePowerNoiseTimerMeter ();
		UpdateAchievementButton ();
		IncreaseAchievementProgress ();
		IncreaseScore ();
		IncreaseNoise ();

        DetectKeyboardInput();
	}

	void InitGameData(){
		GameData.GAME_STARTED = false;
		GameData.GAME_SCORE = 0;	
		GameData.POWER_ANTI_NOISE_ACTIVE = false;
		GameData.POWER_X2_MULTIPLIER_ACTIVE = false;
	}

    void DetectKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (StoreMenu.IsActive())
            {
                StoreMenu.gameObject.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FingerDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            FingerUp();
        }
    }


    void InitStoreTransactions(){
		if (PlayerPrefs.GetInt ("STORE_X2BOOSTER_ACTIVE", 0) == 1) {
			X2BoosterPowerUp.gameObject.SetActive (true);
			AntiNoiseBoosterPowerUp.gameObject.SetActive (true);
		} else {
			X2BoosterPowerUp.gameObject.SetActive (false);
			AntiNoiseBoosterPowerUp.gameObject.SetActive (false);
		}

		if (PlayerPrefs.GetInt ("STORE_BOOTSBOOSTER_ACTIVE", 0) == 1) {
			GameData.STORE_ITEM_BOOTSBOOSTER = true;
			PlayerPrefs.SetInt ("STORE_BOOTSBOOSTER_ACTIVE", 1);
			BootsBooster.gameObject.SetActive (true);

		} else {
			PlayerPrefs.SetInt ("STORE_BOOTSBOOSTER_ACTIVE", 0);
			BootsBooster.gameObject.SetActive (true);

			GameData.STORE_ITEM_BOOTSBOOSTER = false;
			BootsBooster.gameObject.SetActive (false);
		}
	}

	void UpdatePower2XTimerMeter(){
		if (GameData.POWER_X2_MULTIPLIER_ACTIVE) {
			ScoreText.color = new Color32 (203, 58, 255, 255);
			if (Power2XSlider.value > 0) {
				Power2XTopImage.gameObject.SetActive (true);
				Power2XSlider.value -= Time.deltaTime;
			} else if (Power2XSlider.value <= 0) {
				Power2XTopImage.gameObject.SetActive (false);
				Power2XSlider.value = GameData.TIME_POWER_X2_MULTIPLIER;
			}
		} else {
			ScoreText.color = Color.white;
		}
	}

	void UpdatePowerNoiseTimerMeter(){
		if (GameData.POWER_ANTI_NOISE_ACTIVE) {
			if (PowerNoiseSlider.value > 0) {
				PowerNoiseTopImage.gameObject.SetActive (true);
				PowerNoiseSlider.value -= Time.deltaTime;
			} else if(PowerNoiseSlider.value <= 0){
				PowerNoiseTopImage.gameObject.SetActive (false);
				PowerNoiseSlider.value = GameData.TIME_POWER_ANTI_NOISE;
			}
		}
	}
		
	void UpdateScoreView(){
		ScoreText.text = GameData.GAME_SCORE.ToString ("0000000");
	}
	void UpdateOrbView(){
		OrbText.text = GameData.GAME_TOTAL_ORBS.ToString ();
	}


	void UpdateNoiseMeter(){
		if (noiseCurrentValue > NOISE_MAX_VALUE) {
			NoiseMeter.value = noiseCurrentValue;
			GameData.FINGER_DOWN = false;
			StartCoroutine (ShowDeathScreen (2));
		} else {
			if (noiseCurrentValue > 0) {
				if (GameData.STORE_ITEM_BOOTSBOOSTER) {
					noiseCurrentValue -= NOISE_BOOTS_DECREASING_VALUE * Time.deltaTime;
					NoiseFill.color = new Color32 (76, 236, 222, 255);
				} else {
					noiseCurrentValue -= NOISE_DECREASING_VALUE * Time.deltaTime;
					NoiseFill.color = new Color32 (255, 0, 0, 255);
				}
			} else {
				noiseCurrentValue = 0;
			}
			NoiseMeter.value = noiseCurrentValue;
		}
	}

	public void IncreaseScore(){
		if (GameData.GAME_STARTED) {
			if (GameData.FINGER_DOWN) {
				if (noiseCurrentValue <= NOISE_FILL_250) {
					scoreIncrease = GAME_BASE_SCORE;
				} else if (noiseCurrentValue <= NOISE_FILL_300) {
					scoreIncrease = GAME_NEXT1_SCORE;
				} else if (noiseCurrentValue <= NOISE_FILL_375) {
					scoreIncrease = GAME_NEXT2_SCORE;
				} else if (noiseCurrentValue < NOISE_FULL) {
					scoreIncrease = GAME_NEXT3_SCORE;
				} else {
					scoreIncrease = GAME_BASE_SCORE;
				}
				if (GameData.POWER_X2_MULTIPLIER_ACTIVE) {
					GameData.GAME_SCORE += (scoreIncrease * 2);
					if (PlayerPrefs.GetInt ("A_WALKATHON_ACTIVE", 0) != 1) {
						PlayerPrefs.SetInt ("A_WALKATHON_TOTAL", PlayerPrefs.GetInt ("A_WALKATHON_TOTAL", 0) + (scoreIncrease * 2));
						Player.TrackAchievement ();
					} 
					if (PlayerPrefs.GetInt ("A_MILLION_ACTIVE", 0) != 1) {
						PlayerPrefs.SetInt ("A_MILLION_TOTAL", PlayerPrefs.GetInt ("A_MILLION_TOTAL", 0) + (scoreIncrease * 2));
						Player.TrackAchievement ();
					} 
				} else {
					GameData.GAME_SCORE += scoreIncrease;
					if (PlayerPrefs.GetInt ("A_WALKATHON_ACTIVE", 0) != 1) {
						PlayerPrefs.SetInt ("A_WALKATHON_TOTAL", PlayerPrefs.GetInt ("A_WALKATHON_TOTAL", 0) + scoreIncrease);
						Player.TrackAchievement ();
					}
					if (PlayerPrefs.GetInt ("A_MILLION_ACTIVE", 0) != 1) {
						PlayerPrefs.SetInt ("A_MILLION_TOTAL", PlayerPrefs.GetInt ("A_MILLION_TOTAL", 0) + scoreIncrease);
						Player.TrackAchievement ();
					} 
				}
				Debug.Log (PlayerPrefs.GetInt ("A_MILLION_TOTAL", 0));
				Debug.Log (PlayerPrefs.GetInt ("A_WALKATHON_TOTAL", 0));
				Debug.Log (scoreIncrease);
			}
		}
	}

	public void FingerUp(){
		GameData.FINGER_DOWN = false;
	}
	public void FingerDown(){
		GameData.GAME_STARTED = true;
		GameData.FINGER_DOWN = true;
	}

	void IncreaseNoise(){
		if (GameData.GAME_STARTED) {
			if (!GameData.POWER_ANTI_NOISE_ACTIVE) {
				if (GameData.FINGER_DOWN) {
					noiseCurrentValue += NOISE_PLAYER_INCREASE_VALUE;
					if (noiseCurrentValue >= NOISE_FILL_375) {
						NoiseIndicatorImage.gameObject.SetActive (false);
						NoiseIndicatorImage.gameObject.SetActive (true);
					}
				}
			}
		}
	}
		
	public IEnumerator ShowDeathScreen(int deathState){
		DeathSkullAnim.gameObject.SetActive (true);
		GameData.STORE_ITEM_BOOTSBOOSTER = false;
		PlayerPrefs.SetInt ("STORE_BOOTSBOOSTER_ACTIVE", 0); 

		GameData.GAME_STARTED = false;
		GameData.STORE_ITEM_X2BOOSTER = false;
		CheckHighScore ();
		DeathScreen.gameObject.SetActive (true);
		if (deathState == 1) {
			DeathDescriptionText.text = "DODGE THE LIGHT NEXT TIME!";
		} else if (deathState == 2) {
			DeathDescriptionText.text = "YOU ARE WALKING TOO LOUDLY!";
		} else if (deathState == 3) {
			DeathDescriptionText.text = "YOU STOOD FOR TOO LONG!";
		}
		FinalScore.text = GameData.GAME_SCORE.ToString ();
		yield return new WaitForSeconds (UI_FADE_OUT_DELAY);
		SceneManager.LoadScene ("game");
	}

	public void CheckHighScore(){
		if (PlayerPrefs.GetInt ("Game.HighScore", 0) > GameData.GAME_SCORE) {
			//STICK WITH HIGHSCORE
		} else if (PlayerPrefs.GetInt ("Game.HighScore", 0) < GameData.GAME_SCORE) {
			PlayerPrefs.SetInt ("Game.HighScore", GameData.GAME_SCORE);
		}
	}
	//CODES FOR STORE
	public void BuyPowerBooster(){
		if (GameData.GAME_TOTAL_ORBS >= STORE_PRICE_BOOSTER_POWER) {
			GameData.GAME_TOTAL_ORBS -= STORE_PRICE_BOOSTER_POWER;
			PlayerPrefs.SetInt ("TOTAL_GAME_ORB", GameData.GAME_TOTAL_ORBS);

			X2BoosterPowerUp.gameObject.SetActive (true);
			AntiNoiseBoosterPowerUp.gameObject.SetActive (true);
			Item1BuyButton.GetComponent<Button> ().interactable = false;
			Item1BuyButtonText.text = "ACTIVE";
			PlayerPrefs.SetInt ("STORE_X2BOOSTER_ACTIVE", 1);
			StoreOrbsText.text = GameData.GAME_TOTAL_ORBS.ToString ();
			GameData.STORE_ITEM_X2BOOSTER = true;
		} 
	}

	public void BuySilentBootsBooster(){
		if (GameData.GAME_TOTAL_ORBS >= STORE_PRICE_BOOSTER_BOOTS) {
			GameData.GAME_TOTAL_ORBS -= STORE_PRICE_BOOSTER_BOOTS;
			PlayerPrefs.SetInt ("TOTAL_GAME_ORB", GameData.GAME_TOTAL_ORBS);
			BootsBooster.gameObject.SetActive (true);
		
			Item2BuyButton.GetComponent<Button> ().interactable = false;
			Item2BuyButtonText.text = "ACTIVE";
			PlayerPrefs.SetInt ("STORE_BOOTSBOOSTER_ACTIVE", 1);
			StoreOrbsText.text = GameData.GAME_TOTAL_ORBS.ToString ();
			GameData.STORE_ITEM_BOOTSBOOSTER = true;
		}
	}

	public void ShowStoreMenu(){
		StoreOrbsText.text = GameData.GAME_TOTAL_ORBS.ToString();
		if (GameData.GAME_TOTAL_ORBS < STORE_PRICE_BOOSTER_POWER) {
			Item1BuyButton.GetComponent<Button> ().interactable = false;
			Item1BuyButtonText.text = "NOT ENOUGH ORBS";
		}
		if (GameData.GAME_TOTAL_ORBS < STORE_PRICE_BOOSTER_BOOTS) {
			Item2BuyButton.GetComponent<Button> ().interactable = false;
			Item2BuyButtonText.text = "NOT ENOUGH ORBS";
		}

		if (PlayerPrefs.GetInt ("STORE_X2BOOSTER_ACTIVE", 0) == 1) {
			GameData.STORE_ITEM_X2BOOSTER = true;
			Item1BuyButton.GetComponent<Button> ().interactable = false;
			Item1BuyButtonText.text = "ACTIVE";
		}
		if (PlayerPrefs.GetInt ("STORE_BOOTSBOOSTER_ACTIVE", 0) == 1) {
			GameData.STORE_ITEM_BOOTSBOOSTER = true;
			Item2BuyButton.GetComponent<Button> ().interactable = false;
			Item2BuyButtonText.text = "ACTIVE";
		}
		StoreMenu.gameObject.SetActive (true);
	}
	public void HideStoreMenu(){
		StoreMenu.gameObject.SetActive (false);
	}
	//CODES FOR ACHIEVEMENT
	#region Achievements
	void ShowAchievementNotification(string achievement_name, Sprite achievement_icon){
		AchievementNBIcon.sprite = achievement_icon;
		AchievementNBTitle.text = achievement_name;
		AchievementNotifBox.gameObject.SetActive (false);
		AchievementNotifBox.gameObject.SetActive (true);
	}
		
	public void UpdateAndShowAchievementsNotificationBox(){
		if (PlayerPrefs.GetInt ("A_MORE_POWER_ACTIVE", 0) == 1) {
			ShowAchievementNotification ("I NEED MORE POWER", A1Icon);
			AchievementBox1.color = new Color32 (20, 212, 45, 168);
			A1Image.sprite = A1Icon;
		}
		if (PlayerPrefs.GetInt ("A_WALKATHON_ACTIVE", 0) == 1) {
			ShowAchievementNotification ("WALKATHON", A2Icon);
			AchievementBox2.color = new Color32 (20, 212, 45, 168);
			A2Image.sprite = A2Icon;
		}
		if (PlayerPrefs.GetInt ("A_SILENCE_ACTIVE", 0) == 1) {
			ShowAchievementNotification ("SILENCE PLEASE", A3Icon);
			AchievementBox3.color = new Color32 (20, 212, 45, 168);
			A3Image.sprite = A3Icon;
		}
		if (PlayerPrefs.GetInt ("A_DOUBLE_ACTIVE", 0) == 1) {
			ShowAchievementNotification ("DOUBLE DOUBLE", A4Icon);
			AchievementBox4.color = new Color32 (20, 212, 45, 168);
			A4Image.sprite = A4Icon;
		}
		if (PlayerPrefs.GetInt ("A_MILLION_ACTIVE", 0) == 1) {
			ShowAchievementNotification ("MILLIONAIRE", A5Icon);
			AchievementBox5.color = new Color32 (20, 212, 45, 168);
			A5Image.sprite = A5Icon;
		}
	}

	public void InitAchievements(){
		if (PlayerPrefs.GetInt ("A_MORE_POWER_ACTIVE", 0) == 1) {
			AchievementBox1.color = new Color32 (20, 212, 45, 168);
			A1Image.sprite = A1Icon;
		}
		if (PlayerPrefs.GetInt ("A_WALKATHON_ACTIVE", 0) == 1) {
			AchievementBox2.color = new Color32 (20, 212, 45, 168);
			A2Image.sprite = A2Icon;
		}
		if (PlayerPrefs.GetInt ("A_SILENCE_ACTIVE", 0) == 1) {
			AchievementBox3.color = new Color32 (20, 212, 45, 168);
			A3Image.sprite = A3Icon;
		}
		if (PlayerPrefs.GetInt ("A_DOUBLE_ACTIVE", 0) == 1) {
			AchievementBox4.color = new Color32 (20, 212, 45, 168);
			A4Image.sprite = A4Icon;
		}
		if (PlayerPrefs.GetInt ("A_MILLION_ACTIVE", 0) == 1) {
			AchievementBox5.color = new Color32 (20, 212, 45, 168);
			A5Image.sprite = A5Icon;
		}
	}

	void IncreaseAchievementProgress (){
		A1ProgressBar.value = Mathf.RoundToInt ((float)PlayerPrefs.GetInt ("A_MORE_POWER_COUNT", 0));
		A2ProgressBar.value = Mathf.RoundToInt ((float)PlayerPrefs.GetInt ("A_WALKATHON_TOTAL", 0));
		A3ProgressBar.value = Mathf.RoundToInt ((float)PlayerPrefs.GetInt ("A_SILENCE_COUNT", 0));
		A4ProgressBar.value = Mathf.RoundToInt ((float)PlayerPrefs.GetInt ("A_DOUBLE_COUNT", 0));
		A5ProgressBar.value = Mathf.RoundToInt ((float)PlayerPrefs.GetInt ("A_MILLION_TOTAL", 0));
	}

	void UpdateAchievementButton(){
		if (GameData.GAME_STARTED) {
			AchievementButton.gameObject.SetActive (false);
			StoreButton.gameObject.SetActive (false);
		}
	}

	public void ShowAchievementMenu(){
		AchievementsMenu.gameObject.SetActive (true);
	}
	public void HideAchievementMenu(){
		AchievementsMenu.gameObject.SetActive (false);
	}
	#endregion

}
