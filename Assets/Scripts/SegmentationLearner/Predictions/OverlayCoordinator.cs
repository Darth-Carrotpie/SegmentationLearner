using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayCoordinator : Singleton<OverlayCoordinator> {

    public RawImage singleImage;

    public static void RenderToPanelSingle(byte[] inputBytes) {
        Instance.RenderToPanel(Instance.singleImage, inputBytes);
    }

    void RenderToPanel(RawImage image, byte[] inputBytes) {
        Vector2 dims = CameraCapture.GetDimentions();
        Texture2D tex = new Texture2D((int)(dims.x), (int)dims.y, TextureFormat.R8, true);
        tex.LoadImage(inputBytes);
        tex.Apply();
        image.texture = tex;
    }
}