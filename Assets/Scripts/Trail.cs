using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    private Vector3 destination;
    private float endPositionZ = -1f;
    private float speed = 0.5f;

    private void Start()
    {
        destination = new Vector3(0, 0, -2f);
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (this.transform.position.z > destination.z)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);

            //if the trail is off screen, it will return near the player
            if (this.transform.position.z < endPositionZ)
                this.transform.position = PlayerController.Instance.GetChildPosition();

            yield return null;
        }
    }

    public void DestroyTrail()
    {
        Destroy(this.gameObject);
    }
}
