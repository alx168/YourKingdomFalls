using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField]
    GameObject reminder;
    [SerializeField]
    GameObject text;
    [SerializeField]
    GameObject menuImage;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            reminder.SetActive(false);
            menuImage.SetActive(true);
            text.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab)) {
            reminder.SetActive(true);
            menuImage.SetActive(false);
            text.SetActive(false);
        }
    }
}
