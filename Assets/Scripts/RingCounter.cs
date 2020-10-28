using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingCounter : MonoBehaviour
{
    private int ringCount = 0;
    public void IncreaseRingCounter()
    {
        ringCount += 1;
        transform.GetComponent<Text>().text = ringCount.ToString();
    }
}
