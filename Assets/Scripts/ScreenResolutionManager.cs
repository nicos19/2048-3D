using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolutionManager : MonoBehaviour
{
    public GameObject button800x1000;
    public GameObject button400x500;

    private void Start()
    {
        if (Screen.width == 400)
        {
            button400x500.GetComponent<Image>().color = Color.yellow;
        } 
        else if (Screen.width == 800)
        {
            button800x1000.GetComponent<Image>().color = Color.yellow;
        }
    }


    /// <summary>
    /// This method sets the screen resolution to <c>800x1000</c>.
    /// </summary>
    public void SetScreenResolution800x1000()
    {
        button800x1000.GetComponent<Image>().color = Color.yellow;
        button400x500.GetComponent<Image>().color = Color.white;
        Screen.SetResolution(800, 1000, false);
    }

    /// <summary>
    /// This method sets the screen resolution to <c>400x500</c>.
    /// </summary>
    public void SetScreenResolution400x500()
    {
        button800x1000.GetComponent<Image>().color = Color.white;
        button400x500.GetComponent<Image>().color = Color.yellow;
        Screen.SetResolution(400, 500, false);
    }
}
