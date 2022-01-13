using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public List<GameObject> listPrefabsObstacles;
    public List<Material> listMaterials;
    public GameObject prefabMoney;
    public GameObject prefabFeverMode;

    //The current score
    private int score = 0;
    //The current highscore
    private int highScore = 0;
    //Total money earned by the player
    private int money = 0;
    //Current level of the player
    private int level = 0;
    //Max progression to reach the next level
    private int progressionMaxPerLevel = 200;
    //Current progression
    private int levelProgression = 0;

    private bool endGame = true;

    private float feverTime = 10.0f;
    private bool feverActivated = false;

    //Global speed of GameObjects (obstacles, fever, money)
    public float speed = 2.5f;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //we recover the saved data
        highScore = SaveData.Instance.GetHighScore();
        money = SaveData.Instance.GetMoneyCount();
        level = SaveData.Instance.GetLevel();
        levelProgression = SaveData.Instance.GetLevelProgression();

        Restart();
        UIController.Instance.UpdateMoney(money);
        UIController.Instance.SetLevel(level, levelProgression, progressionMaxPerLevel);
    }

    public void StartSpawnObstacles()
    {
        if (endGame)
        {
            endGame = false;
            UIController.Instance.HideAnimHand();
            UIController.Instance.ShowScore();
            UIController.Instance.VideoPlayerOn();
            StartCoroutine(SpawnObstacles());
        }
    }

    IEnumerator SpawnObstacles()
    {
        GameObject lastObject = null;

        yield return new WaitForSeconds(2.0f);

        //We choose the obstacle
        int randObstacle = 0;
        int cptObstacle = 0;

        //We choose the obstacle's color
        int randMaterial = 0;
        int cptMaterial = 0;

        while (!endGame)
        {
            //We choose the type of obstacle here
            randObstacle = Random.Range(0, listPrefabsObstacles.Count);

            //We choose the colors here
            if (cptMaterial == 0)
                randMaterial = Random.Range(0, listMaterials.Count / 2); // -/- by 2 because we choose 2 materials at the time

            //We spawned 5 obstacles before picking a new one
            while (cptObstacle < 5 && !endGame)
            {
                lastObject = Instantiate(listPrefabsObstacles[randObstacle]);
                lastObject.transform.SetParent(this.transform);
                lastObject.GetComponent<Obstacle>().SetSpeed(speed);
                lastObject.GetComponent<Obstacle>().SetMaterials(listMaterials[randMaterial * 2], listMaterials[randMaterial * 2 + 1]); //We change the materials here

                //We choose a bonus
                int randNumber = Random.Range(0, 100);
                //FEVER 5% luck
                if (randNumber < 5 && !feverActivated)
                {
                    yield return new WaitUntil(() => lastObject.transform.localPosition.z < 1.75f || endGame);
                    if (!endGame)
                    {
                        GameObject feverMode = Instantiate(prefabFeverMode);
                        feverMode.transform.SetParent(this.transform);
                        feverMode.GetComponent<Fever>().SetSpeed(speed);
                    }
                }
                //MONEY 25% luck
                else if (randNumber < 30)
                {
                    yield return new WaitUntil(() => lastObject.transform.localPosition.z < 1.75f || endGame);
                    if (!endGame)
                    {
                        GameObject money = Instantiate(prefabMoney);
                        money.transform.SetParent(this.transform);
                        money.GetComponent<Money>().SetSpeed(speed);
                    }
                }

                //We wait for the first obstacle to disappear
                yield return new WaitUntil(() => lastObject.transform.localPosition.z < 0.75f || endGame);
                cptObstacle++;
            }
            cptObstacle = 0;

            //We change the colors every 2 cycles
            if (cptMaterial == 1)
                cptMaterial = 0;
            else
                cptMaterial++;
        }
    }

    public void UpdateCounter()
    {
        score += 1;
        UIController.Instance.UpdateScore(score);
    }

    //When the player dies
    public void EndGame()
    {
        if (feverActivated)
            return;

        endGame = true;
        //we pause the game 
        Time.timeScale = 0;
        PlayerController.Instance.Death();
        PlayerController.Instance.DestroyTrail();
        PlayerController.Instance.SpawnParticle();

        //If it's a new highscore
        if (score > highScore)
        {
            highScore = score;
            SaveData.Instance.SaveHighScore(highScore);
        }

        //We calculate the new level
        level = (int)((score + levelProgression + level * progressionMaxPerLevel) / progressionMaxPerLevel);
        SaveData.Instance.SaveLevel(level);
        levelProgression = (score + levelProgression) % progressionMaxPerLevel;
        SaveData.Instance.SaveLevelProgression(levelProgression);

        UIController.Instance.SetHighScore(highScore);
        UIController.Instance.SetLevel(level, levelProgression, progressionMaxPerLevel);
        UIController.Instance.Death();
        UIController.Instance.VideoPlayerOff();

    }

    //When the player restarts the game
    public void Restart()
    {
        //We unpause the game
        Time.timeScale = 1;
        PlayerController.Instance.Restart();

        score = 0;
        UIController.Instance.HideScore();
        UIController.Instance.UpdateScore(0);
        UIController.Instance.VideoPlayerOff();

        //We destroy the previous objects
        Obstacle[] arrayObstacles = FindObjectsOfType<Obstacle>();
        for (int i = arrayObstacles.Length - 1; i >= 0; i--)
            arrayObstacles[i].DestroyObstacle();

        Money[] arrayMoney = FindObjectsOfType<Money>();
        for (int i = arrayMoney.Length - 1; i >= 0; i--)
            arrayMoney[i].DestroyObstacle();

        Fever[] arrayFever = FindObjectsOfType<Fever>();
        for (int i = arrayFever.Length - 1; i >= 0; i--)
            arrayFever[i].DestroyObstacle();

        Particle[] arrayParticle = FindObjectsOfType<Particle>();
        for (int i = arrayParticle.Length - 1; i >= 0; i--)
            arrayParticle[i].DestroyObstacle();

        UIController.Instance.ShowAnimHand();
    }

    public void AddMoney(int moneyValue)
    {
        money += moneyValue;
        UIController.Instance.UpdateMoney(money);
        SaveData.Instance.SaveMoneyCount(money);
    }

    public void FeverMode()
    {
        if (!feverActivated)
            StartCoroutine(FeverTime());
    }

    IEnumerator FeverTime()
    {
        feverActivated = true;
        Time.timeScale = 2.5f;
        UIBackgroundController.Instance.IncreaseSpeed();
        PlayerController.Instance.ActivateFeverMode();

        yield return new WaitForSeconds(feverTime);

        Time.timeScale = 1.0f;
        UIBackgroundController.Instance.DecreaseSpeed();
        PlayerController.Instance.DisableFeverMode();

        //We give the player some time to prepare
        yield return new WaitForSeconds(0.5f);
        feverActivated = false;
    }
}
