using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class CameraChangeController : MonoBehaviour {
    public GameObject canvas;
    private bool openedToFullscreen = false;

    private RectTransform rectTransform;
    private RectTransform canvasRect;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (openedToFullscreen)
            {
                rectTransform.sizeDelta = new Vector2(320, 180);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(canvasRect.rect.width, canvasRect.rect.height);
            }
            openedToFullscreen = !openedToFullscreen;
        }
    }
}