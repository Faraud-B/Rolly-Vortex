using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

    private string strHighScore = "HighScore";
    private string strMoneyCount = "MoneyCount";
    private string strLevel = "Level";
    private string strLevelProgression = "LevelProgression";

    private void Awake()
    {
        Instance = this;
    }

    [ContextMenu("Delete Datas")]
    void DoSomething()
    {
        PlayerPrefs.SetInt(strHighScore, 0);
        PlayerPrefs.SetInt(strMoneyCount, 0);
        PlayerPrefs.SetInt(strLevel, 0);
        PlayerPrefs.SetInt(strLevelProgression, 0);
    }

    public void SaveHighScore(int highScore)
    {
        PlayerPrefs.SetInt(strHighScore, highScore);
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(strHighScore);
    }

    public void SaveMoneyCount(int moneyCount)
    {
        PlayerPrefs.SetInt(strMoneyCount, moneyCount);
    }

    public int GetMoneyCount()
    {
        return PlayerPrefs.GetInt(strMoneyCount);
    }

    public void SaveLevel(int level)
    {
        PlayerPrefs.SetInt(strLevel, level);
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt(strLevel);
    }

    public void SaveLevelProgression(int levelProgression)
    {
        PlayerPrefs.SetInt(strLevelProgression, levelProgression);
    }

    public int GetLevelProgression()
    {
        return PlayerPrefs.GetInt(strLevelProgression);
    }
}
