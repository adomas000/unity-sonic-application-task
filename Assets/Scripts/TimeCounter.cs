using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public GameObject displayTime;
    private Text text;

    private int secondsCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = displayTime.GetComponent<Text>();
        StartCoroutine(SecondsCounter());
    }

    IEnumerator SecondsCounter ()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            secondsCounter += 1;
            text.text = DisplayTime(secondsCounter);
        }
    }

    string DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        string strSeconds = seconds < 10 ? ("0" + seconds) : seconds.ToString();
        return string.Format("{0}:{1}",
            minutes,
            strSeconds
        );
    }
}
