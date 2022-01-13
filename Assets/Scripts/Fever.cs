using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fever : MonoBehaviour
{
    private float speed = 0.5f;
    private Vector3 destination;

    private float startPositionZ = 5f;
    private float endPositionZ = -0.25f;

    private void Awake()
    {
        transform.position = new Vector3(0, 0, startPositionZ);
        transform.rotation = Quaternion.Euler(0, 0, RandomRotation());

        destination = new Vector3(0, 0, -0.5f);
    }

    private void Start()
    {
        StartCoroutine(Move());
    }

    //Move toward the player
    IEnumerator Move()
    {
        while (this.transform.position.z > destination.z)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);

            //if the object is behind the player, we destroy it
            if (this.transform.position.z < endPositionZ)
                DestroyObstacle();

            yield return null;
        }
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
        GameController.Instance.FeverMode();
        DestroyObstacle();
    }

    public void DestroyObstacle()
    {
        Destroy(this.gameObject);
    }
}
