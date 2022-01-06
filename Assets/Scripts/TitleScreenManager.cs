using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject TitleScreen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("return"))
        {
            if (TitleScreen.activeSelf)
            {
                // deactivate title screen when ENTER key is pressed
                TitleScreen.SetActive(false);
            }
        }
    }
}
