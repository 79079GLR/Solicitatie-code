using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject gameOverObject;
	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public int Highscore = 0;

	public Text HighscoreText;
	public Text scoreText;
	public Text restartText;
	public Text gameOverText;
	private int score;
	private bool gameOver;
	private bool restart;

	void Start() {
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine(SpawnWaves ());
		HighscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();//haalt de Highsccore en zet het in de UI
		Highscore = PlayerPrefs.GetInt("Highscore", 0) ;//pas de Highsccore aan in de code 
	}

	void Update() {
		if (restart) {
			if (Input.GetKeyDown(KeyCode.R)) {
				gameOverObject.gameObject.SetActive(false);
				Application.LoadLevel(Application.loadedLevel);
			}
		}

		
	}

	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds(startWait);
		while(!restart) {
			for (int i = 0; i < hazardCount; ++i) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);
			if (gameOver) {
				gameOverObject.gameObject.SetActive(true);
				restartText.text = "Press 'R' for restart";
				restart = true;
			}
		}
	}

	public void AddScore(int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore() {
		if(Highscore < score)
		UpdateHighScore();//update de Highscore

		scoreText.text = "Score: " + score;

	}
//toegevoeg voor de Highscore
	void UpdateHighScore(){
			Highscore = score;
			HighscoreText.text = "Highscore: " + Highscore;
			PlayerPrefs.SetInt("Highscore", Highscore);

	}

	public void GameOver() {
		gameOver = true;
		gameOverText.text = "Game Over";
	}
}
