using System.Collections;
using System.IO;
using UnityEngine;
using System;

public static class SHARE_INFO
{
    public static string SUBJECT = $"¿ÃπÃ¡ˆ";
}

public class ShareComponent : MonoBehaviour
{
    public Action OnScreenshootStart { get; set; }
    public Action OnScreenshootEnd { get; set; }

    public void Initialize(Action OnScreenshootStart, Action OnScreenshootEnd)
    {
        this.OnScreenshootStart = OnScreenshootStart;
        this.OnScreenshootEnd = OnScreenshootEnd;
    }

    public void OnShareButtonClick()
    {
        StartCoroutine(TakeScreenshotAndShare());
    }

    private string MakeSampleScreenShotImage()
    {
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        Destroy(ss);

        return filePath;
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        OnScreenshootStart?.Invoke();

        yield return new WaitForEndOfFrame();

        string filePath = MakeSampleScreenShotImage();
        string shareText = "Test your Memory! \n https://play.google.com/store/apps/details?id=com.mozi.colormemory";

        new NativeShare()
            .AddFile(filePath)
            .SetSubject(SHARE_INFO.SUBJECT)
            .SetText(shareText)
            .Share();

        if (NativeShare.TargetExists("com.whatsapp"))
        {
            new NativeShare()
                .AddFile(filePath)
                .SetText(shareText)
                .AddTarget("com.whatsapp")
                .Share();
        }

        yield return new WaitForEndOfFrame();

        OnScreenshootEnd?.Invoke();
    }
}
