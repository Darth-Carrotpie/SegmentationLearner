using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayCoordinator : Singleton<OverlayCoordinator>
{
    public RawImage image;
    public static void RenderToPanel(byte[] inputBytes)
    {
        Debug.Log(inputBytes.Length);
        Texture2D tex = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        tex.LoadRawTextureData(inputBytes);
        tex.Apply();
        Instance.image.texture = tex;
        //GetComponent<Renderer>().material.mainTexture = tex;
    }
}
