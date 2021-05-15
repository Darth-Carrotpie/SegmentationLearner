using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayCoordinator : Singleton<OverlayCoordinator>
{
    public RawImage image;
    public static void RenderToPanel(byte[] inputBytes)
    {
        Vector2 dims = CameraCapture.GetDimentions();
        Texture2D tex = new Texture2D((int)(dims.x), (int)dims.y, TextureFormat.R8, true);
        tex.LoadImage(inputBytes);
        tex.Apply();
        Instance.image.texture = tex;
        //GetComponent<Renderer>().material.mainTexture = tex;
    }
}
