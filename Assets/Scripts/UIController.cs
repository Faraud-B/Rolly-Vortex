using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("GameObject")]
    public GameObject goProgressBar;

    [Header("Text")]
    public GameObject textScore;
    public GameObject textHighScore;
    public GameObject textMoney;
    public GameObject textNbLevel;

    [Header("Button")]
    public GameObject buttonRestart;

    [Header("Animation")]
    public GameObject animHand;
    public GameObject animLevel;

    [Header("Video Player")]
    public GameObject videoPlayer;

    [Header("Slider")]
    public GameObject sliderLevelProgression;

    private int currentLevel;
    private int currentLevelProgression;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        textScore.GetComponent<TMP_Text>().text = "0";
        buttonRestart.SetActive(false);
        textHighScore.SetActive(false);
        goProgressBar.SetActive(false);
        ShowAnimHand();
    }

    public void UpdateScore(int score)
    {
        textScore.GetComponent<TMP_Text>().text = score.ToString();
    }

    public void UpdateMoney(int money)
    {
        textMoney.GetComponent<TMP_Text>().text = money.ToString();
    }

    public void Death()
    {
        buttonRestart.SetActive(true);
        textHighScore.SetActive(true);
        goProgressBar.SetActive(true);
    }

    public void ButtonRestart()
    {
        buttonRestart.SetActive(false);
        textHighScore.SetActive(false);
        goProgressBar.SetActive(false);
        GameController.Instance.Restart();
    }

    public void ShowScore()
    {
        textScore.SetActive(true);
    }

    public void HideScore()
    {
        textScore.SetActive(false);
    }

    public void ShowAnimHand()
    {
        animHand.SetActive(true);
    }

    public void HideAnimHand()
    {
        animHand.SetActive(false);
    }

    public void VideoPlayerOn()
    {
        videoPlayer.GetComponent<VideoPlayer>().Play();
    }

    public void VideoPlayerOff()
    {
        videoPlayer.GetComponent<VideoPlayer>().Pause();
    }

    public void SetHighScore(int highScore)
    {
        textHighScore.GetComponent<TMP_Text>().text = "Record : " + highScore.ToString();
    }

    public void SetLevel(int level, int levelProgression, int progressionMaxPerLevel)
    {
        StartCoroutine(AnimateLevel(level, levelProgression, progressionMaxPerLevel));
    }

    //Level animation
    IEnumerator AnimateLevel(int level, int levelProgression, int progressionMaxPerLevel)
    {
        textNbLevel.GetComponent<TMP_Text>().text = currentLevel.ToString();
        sliderLevelProgression.GetComponent<Slider>().value = currentLevelProgression;
        animLevel.SetActive(false);
        yield return new WaitForSecondsRealtime(0.25f);
        
        //if the level is still the same
        if (currentLevel == level)
        {
            //We animate the slider
            while (sliderLevelProgression.GetComponent<Slider>().value != levelProgression)
            {
                sliderLevelProgression.GetComponent<Slider>().value += 1;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        //if the level has increased 
        else
        {
            //We animate the slider
            while (sliderLevelProgression.GetComponent<Slider>().value != progressionMaxPerLevel)
            {
                sliderLevelProgression.GetComponent<Slider>().value += 1;
                yield return new WaitForSecondsRealtime(0.01f);
            }

            //We update the level
            textNbLevel.GetComponent<TMP_Text>().text = level.ToString();
            //We activate the level animation
            animLevel.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            sliderLevelProgression.GetComponent<Slider>().value = 0;

            //We animate the slider
            while (sliderLevelProgression.GetComponent<Slider>().value != levelProgression)
            {
                sliderLevelProgression.GetComponent<Slider>().value += 1;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        currentLevel = level;
        currentLevelProgression = levelProgression;
    }

    //Cheat button used to show the animation
    public void ButtonShowAnimation()
    {
        SetLevel(currentLevel + 1, 150, 200);
    }

}
