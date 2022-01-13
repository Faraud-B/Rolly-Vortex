using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float scale = 0.01f;

    private void Awake()
    {
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

    void Start()
    {
        StartCoroutine(ErraticMovement());
    }

    //little random movement for particles
    IEnumerator ErraticMovement()
    {
        Vector3 destination;
        float distanceMax = 0.02f;

        int nbMovement = Random.Range(75, 125);
        int cptMovement = 0;
        int nbMovementIteration = 5;
        int cptMovementIteration = 0;

        int valueX = 1;
        if (Random.Range(0, 2) == 0)
            valueX = -1;

        int valueY = 1;
        if (Random.Range(0, 2) == 0)
            valueY = -1;

        while (cptMovement != nbMovement)
        {
            float posX = Random.Range(0, distanceMax);
            float posY = Random.Range(0, distanceMax);
            destination = this.transform.position + new Vector3(posX * valueX, posY * valueY, 0);

            float time = Random.Range(1.0f, 2.0f);
            float originalTime = time;

            while (cptMovementIteration < nbMovementIteration)
            {
                time -= Time.unscaledDeltaTime;
                transform.position = Vector3.Lerp(transform.position, destination, 1 - (time / originalTime));

                yield return null;
                cptMovementIteration++;
            }
            cptMovementIteration = 0;
            cptMovement++;
        }
        yield return null;
        DestroyObstacle();
    }

    public void DestroyObstacle()
    {
        Destroy(this.gameObject);
    }
}
