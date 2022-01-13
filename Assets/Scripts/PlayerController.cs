using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private bool keyPressed = false;

    //Use to move the player depending on the mouse x axis position
    private float lastPositionX = 0;
    private float newPositionX = 0;

    private float speed = 0.25f;

    private bool stopGame = false;

    //Trail
    public GameObject prefabTrail;
    public int nbTrail;
    public GameObject trailParent;
    private List<GameObject> listTrail;

    public GameObject prefabParticle;
    private int nbParticle = 20;

    //Fever mode
    private bool feverModeActivated = false;
    Color[] arrayColors = new Color[] { Color.red, Color.green, Color.magenta, Color.yellow, Color.cyan }; //use during fever mode
    private Color initColor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        listTrail = new List<GameObject>();
        initColor = this.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
    }

    private void Update()
    {
        if (!stopGame)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                lastPositionX = Input.mousePosition.x;
                keyPressed = true;
                GameController.Instance.StartSpawnObstacles();
                StartCoroutine(Trail());
            }

            if (Input.GetButton("Fire1") && keyPressed)
            {
                newPositionX = Input.mousePosition.x;

                float value = newPositionX - lastPositionX;
                transform.Rotate(0, 0, value * speed);

                lastPositionX = newPositionX;

            }

            if (Input.GetButtonUp("Fire1") && keyPressed)
            {
                keyPressed = false;
            }
        }
    }

    IEnumerator Trail()
    {
        GameObject temp;
        for (int i = 0; i < nbTrail; i++)
        {
            temp = Instantiate(prefabTrail, GetChildPosition(), Quaternion.identity);
            temp.transform.SetParent(trailParent.transform);
            listTrail.Add(temp);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DestroyTrail()
    {
        GameObject temp;
        for (int i = listTrail.Count - 1; i >= 0; i--)
        {
            temp = listTrail[i];
            listTrail.RemoveAt(i);
            temp.GetComponent<Trail>().DestroyTrail();
        }
    }

    public void Death()
    {
        stopGame = true;
    }

    public void Restart()
    {
        stopGame = false;
        this.transform.rotation = Quaternion.identity;
        EnablePlayer();
    }

    public Vector3 GetChildPosition()
    {
        return this.transform.GetChild(0).transform.position;
    }

    public void ActivateFeverMode()
    {
        feverModeActivated = true;
        StartCoroutine(FeverModeColor());
    }

    public void DisableFeverMode()
    {
        feverModeActivated = false;
    }

    IEnumerator FeverModeColor()
    {
        while (feverModeActivated)
        {
            for (int i = 0; i < arrayColors.Length; i++)
            {
                this.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = arrayColors[i];
                yield return new WaitForSeconds(0.15f);
            }
        }
        this.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = initColor;
    }

    public void SpawnParticle()
    {
        GameObject goParticle;
        DisablePlayer();
        for(int i = 0; i < nbParticle; i++)
        {
            goParticle = Instantiate(prefabParticle);
            goParticle.transform.parent = this.transform;
            goParticle.transform.position = this.transform.GetChild(0).transform.position;
        }
    }

    public void EnablePlayer()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DisablePlayer()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
