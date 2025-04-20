using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("Take High-Res Screenshot")]
    void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("Screenshot.png", superSize: 2);
        Debug.Log("Screenshot saved!");
    }
}