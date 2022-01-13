using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIBackgroundController : MonoBehaviour
{
    public static UIBackgroundController Instance;

    public VideoPlayer videoPlayer;
    public GameObject imgBackground;

    private float alphaValue = 50;

    private void Awake()
    {
        Instance = this;
    }

    public void IncreaseSpeed()
    {
        videoPlayer.playbackSpeed *= 2;
    }

    public void DecreaseSpeed()
    {
        videoPlayer.playbackSpeed /= 2;
    }

    public void ShowBackground()
    {
        imgBackground.SetActive(true);
    }

    public void HideBackground()
    {
        imgBackground.SetActive(false);
    }

    public void ChangeBackgroundColor(Color c)
    {
        Color color = new Color(c.r, c.g, c.b, alphaValue);
        imgBackground.GetComponent<Image>().color = color;
    }

}
