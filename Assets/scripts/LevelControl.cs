using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour {
    private const string UI_Score = "Score: ";
    private const string UI_Life = "Lives: ";

    public GameObject[] AsteroidHazards;
    public int AsteroidStartPoint = 16;
    public int AsteroidStartWidth = 8;
    public float SpawnSpeed;
    public UnityEngine.UI.Text ScoreText;
    public UnityEngine.UI.Text LifeText;

    public UnityEngine.UI.Text GameOverText;
    private int fScore = 50;
    private bool GameOver = false;

    /*
    снимать очки за каждый выстрел
    добавлять за каждый астероид, уничтоженный границей
    возможные улучшения: добавление небольшого количества очков за уничтоженные астероиды
    ускорение перезарядки
    двойной-тройной выстрел
    укрепление брони игрока
    увеличение наград, уменьшение цен
    установка мин
    */


    public int Score
    {
        get { return fScore; }
        set {
            if (fScore == value)
                return;
            fScore = value;
            UpdateScore();
        }
    }
    private static LevelControl instance;

    public static LevelControl Instance
    {
        get 
        {
            return instance; 
        }
    }

    IEnumerator SpawnWaves()
    {
        while (!GameOver)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-AsteroidStartWidth, AsteroidStartWidth), 0, AsteroidStartPoint);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(AsteroidHazards[Random.Range(0, 3)], spawnPosition, spawnRotation);
            yield return new WaitForSeconds(SpawnSpeed);
        }
        yield break;
    }
    // Use this for initialization
    private void Start()
    {
        instance = this;
        GameOverText.enabled = false;
        StartCoroutine(SpawnWaves());
        Score = 0;
        UpdateScore();
    }
    private void UpdateScore()
    {
        ScoreText.text = UI_Score + Score;
    }

    public void UpdateLives(int lifeNum)
    {
        LifeText.text = UI_Life + lifeNum;
    }
    public void SetTextGameOver()
    {
        GameOverText.enabled = true;
        GameOver = true;
    }
}
