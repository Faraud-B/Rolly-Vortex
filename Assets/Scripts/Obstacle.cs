using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float speed = 0.5f;
    private Vector3 destination;

    private float startPositionZ = 5f;
    private float endPositionZ = -0.25f;

    private bool updateCounter = true;

    public bool alwaysRotate = false;
    private float rotationDirection = 1;

    private void Awake()
    {
        transform.position = new Vector3(0, 0, startPositionZ);
        transform.rotation = Quaternion.Euler(0, 0, RandomRotation());
        destination = new Vector3(0, 0, -0.5f);
    }

    private void Start()
    {
        if (alwaysRotate && Random.Range(0, 2) == 0)
            rotationDirection = -1;

        StartCoroutine(Move());
        StartCoroutine(Rotate());
    }

    IEnumerator Move()
    {
        while (this.transform.position.z > destination.z)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);

            //When the obstacle is unreachable, we update the score
            if (this.transform.position.z < -0.05 && updateCounter)
            {
                updateCounter = false;
                GameController.Instance.UpdateCounter();
            }

            //Destroy obstacle
            if (this.transform.position.z < endPositionZ)
                DestroyObstacle();

            yield return null;
        }
    }

    IEnumerator Rotate()
    {
        //the object rotate at spawn
        if (!alwaysRotate)
        {
            int value = 1;
            if (transform.rotation.z > 0)
                value = -1;

            float time = 0.33f;
            float originalTime = time;
            Quaternion originalRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + 90 * value);
            Quaternion newRotation = transform.rotation;

            while (time > 0.0f)
            {
                time -= Time.deltaTime;
                transform.rotation = Quaternion.Lerp(newRotation, originalRotation, 1 - (time / originalTime));
                yield return null;
            }
        }
        //the object rotate all the time
        else
        {
            while(true)
            {
                transform.Rotate(0, 0, 0.15f * rotationDirection);
                //"WaitForSeconds" and not "null" so that Time.timeScale will stop the rotation
                yield return new WaitForSeconds(0.001f); ;
            }
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

    public void SetMaterials(Material m1, Material m2)
    {
        var materialTemp = this.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials;
        materialTemp[0] = m1;
        materialTemp[1] = m2;
        this.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials = materialTemp;
    }

    public void TriggerEnter()
    {
        GameController.Instance.EndGame();
    }

    public void DestroyObstacle()
    {
        Destroy(this.gameObject);
    }
}
