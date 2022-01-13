using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    //The collision is handled by the children
    //when a collision is detected, they will call their parents
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (this.transform.parent.GetComponent<Obstacle>())
            this.transform.parent.GetComponent<Obstacle>().TriggerEnter();

        if (this.transform.parent.GetComponent<Money>())
            this.transform.parent.GetComponent<Money>().TriggerEnter();

        if (this.transform.parent.GetComponent<Fever>())
            this.transform.parent.GetComponent<Fever>().TriggerEnter();
    }
}
