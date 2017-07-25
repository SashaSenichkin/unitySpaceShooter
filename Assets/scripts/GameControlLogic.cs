using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlLogic : MonoBehaviour {

    public GameObject[] AsteroidHazard1;
    public int ZValue;
    public int XSize;
    public float SpawnSpeed;
    public UnityEngine.UI.Text ScoreText;
    public UnityEngine.UI.Text GameOverText;
    private int fScore = 50;
    private bool GameOver = false;

    /*
    снимать очки за каждый выстрел
    добавлять за каждый сбитый корабль и за каждый астероид, уничтоженный границей
    возможные улучшения: добавление небольшого количества очков за уничтоженные астероиды
    ускорение перезарядки
    двойной-тройной выстрел
    укрепление брони игрока
    увеличение наград, уменьшение цен
    установка мин
    притягивающий луч (кинул астероид в нужное место - больше награда)
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
    IEnumerator SpawnWaves()
    {
        while (!GameOver)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-XSize, XSize), 0, ZValue);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(AsteroidHazard1[Random.Range(0, 3)], spawnPosition, spawnRotation);
            yield return new WaitForSeconds(SpawnSpeed);
        }
        yield break;
    }
    // Use this for initialization
    private void Start () {
       GameOverText.enabled = false;
       StartCoroutine( SpawnWaves());
       Score = 0;
       UpdateScore();
    }
    private void UpdateScore()
    {
        ScoreText.text = "Score: " + Score;
    }
    public void SetTextGameOver()
    {
        GameOverText.enabled = true;
        GameOver = true;
    }
}
