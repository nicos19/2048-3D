using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject TitleScreen;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (TitleScreen.activeSelf)
            {
                // deactivate title screen when any key or mouse button is pressed
                TitleScreen.SetActive(false);
            }
        }
    }
}
