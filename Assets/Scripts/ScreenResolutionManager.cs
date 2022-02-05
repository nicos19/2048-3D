using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolutionManager : MonoBehaviour
{
    public GameObject button800x1000;
    public GameObject button400x500;
    public GameObject Camera;

    private Vector2Int _resolution;
    private float _targetAspectRatio;

    void Start()
    {
        // Adjust camera if needed to show the whole playfield
        /*if ((float)Screen.width / (float)Screen.height < 0.8f)
        {
            double cameraYChange = 0.8 / ((double)Screen.width / (double)Screen.height);
            Camera.transform.position = new Vector3(Camera.transform.position.x, 
                                                    Camera.transform.position.y * (float)cameraYChange, 
                                                    Camera.transform.position.z);
        }*/

        _resolution = new Vector2Int(Screen.width, Screen.height);

        // standard resolution / target window size is 400x500
        _targetAspectRatio = 400f / 500f;

        if (!Mathf.Approximately((float)Screen.width / Screen.height, _targetAspectRatio))
        {
            // initial window size is not similar to target window size
            UpdateCameraViewport();
        }
    }

    void Update()
    {
        // check if game window size / resolution changed
        if (Screen.width != _resolution.x || Screen.height != _resolution.y)
        {
            // resolution changed -> update camera's viewport rect
            UpdateCameraViewport();

            // remember new resolution
            _resolution.x = Screen.width;
            _resolution.y = Screen.height;
        }
    }

    /// <summary>
    /// Updates camera's viewport depending on current resolution.
    /// </summary>
    private void UpdateCameraViewport()
    {
        float screenAspectRatio = (float)Screen.width / Screen.height;  // currently active aspect ratio

        if (Mathf.Approximately(screenAspectRatio, _targetAspectRatio))
        {
            // current aspect ratio is similar to target aspect ratio -> camera shows the whole area
            Camera.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        }
        else if (screenAspectRatio > _targetAspectRatio)
        {
            // game window is wider than target window -> create pillarbox effect
            float viewportRectWidth = _targetAspectRatio / screenAspectRatio;
            float viewportRectX = (1f - viewportRectWidth) / 2f;
            Camera.GetComponent<Camera>().rect = new Rect(viewportRectX, 0, viewportRectWidth, 1);
        }
        else if (screenAspectRatio < _targetAspectRatio)
        {
            // game window is narrower than target window -> create letterbox effect
            float viewportRectHeight = screenAspectRatio / _targetAspectRatio;
            float viewportRectY = (1f - viewportRectHeight) / 2f;
            Camera.GetComponent<Camera>().rect = new Rect(0, viewportRectY, 1, viewportRectHeight);
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
