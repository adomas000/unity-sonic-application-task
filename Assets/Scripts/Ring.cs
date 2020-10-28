using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject ringCounter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ringCounter.GetComponent<RingCounter>().IncreaseRingCounter();
            Destroy(transform.gameObject);
        }
    }
}
