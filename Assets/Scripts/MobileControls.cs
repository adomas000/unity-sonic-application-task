using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    public GameObject player;
    public GameObject upButton;
    
    public string btnKeyPressed;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            transform.gameObject.SetActive(false);
            upButton.SetActive(false);
        }
    }

    public void ButtonPressHandler(string btnKey)
    {
        if (btnKey == "W")
        {
            player.GetComponent<SonicController>().HandleUpButton();
        } else
        {
            btnKeyPressed = btnKey;
        }
    }

    public void ButtonUpHandler(string btnKey)
    {
        if (btnKeyPressed == btnKey)
        {
            btnKeyPressed = "";
            player.GetComponent<SonicController>().inputAxis = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (btnKeyPressed == "D")
        {
            player.GetComponent<SonicController>().inputAxis = 1;
        }
        else if (btnKeyPressed == "A")
        {
            player.GetComponent<SonicController>().inputAxis = -1;
        }
    }
}
