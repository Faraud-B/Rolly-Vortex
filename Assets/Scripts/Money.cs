using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    private float speed = 0.5f;
    private Vector3 destination;

    private float startPositionZ = 5f;
    private float endPositionZ = -0.25f;

    private bool moneyTaken = false;

    public GameObject models;
    public GameObject canvas;

    private int moneyValue = 1;

    public Material matBronze;
    public Material matSilver;
    public Material matGold;

    private void Awake()
    {
        transform.position = new Vector3(0, 0, startPositionZ);
        transform.rotation = Quaternion.Euler(0, 0, RandomRotation());

        //so that the text does not rotate 
        canvas.transform.rotation = Quaternion.Euler(0, 0, -this.transform.rotation.z);
        destination = new Vector3(0, 0, -0.5f);

        //We disable the text
        canvas.SetActive(false);
        models.SetActive(true);

        //We change the rarity (bronze, silver or gold)
        ChangeRarity();
    }

    private void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (this.transform.position.z > destination.z && !moneyTaken)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);

            if (this.transform.position.z < endPositionZ)
                DestroyObstacle();

            yield return null;
        }

        if(moneyTaken)
        {
            //We make the text appear 1 second before destroying the object
            canvas.transform.GetChild(0).GetComponent<TMP_Text>().text = "+" + moneyValue.ToString();
            canvas.SetActive(true);
            models.SetActive(false);

            yield return new WaitForSeconds(1.0f);
            DestroyObstacle();
        }
    }

    public void ChangeRarity()
    {
        int nb = Random.Range(0, 20);
        var materialTemp = this.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials;
        
        //GOLD 5%
        if (nb  < 1)
        {
            moneyValue = 10;
            materialTemp[0] = matGold;
            materialTemp[1] = matGold;
        }
        //SILVER 20%
        else if (nb < 5)
        {
            moneyValue = 5;
            materialTemp[0] = matSilver;
            materialTemp[1] = matSilver;
        }
        //BRONZE 75%
        else
        {
            moneyValue = 1;
            materialTemp[0] = matBronze;
            materialTemp[1] = matBronze;
        }

        this.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials = materialTemp;
    }

    private float RandomRotation()
    {
        float rotation = Random.Range(-90, 91);
        return rotation;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void TriggerEnter()
    {
        GameController.Instance.AddMoney(moneyValue);
        moneyTaken = true;
    }

    public void DestroyObstacle()
    {
        Destroy(this.gameObject);
    }
}
