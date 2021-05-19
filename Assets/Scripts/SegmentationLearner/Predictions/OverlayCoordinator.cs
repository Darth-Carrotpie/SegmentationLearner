using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayCoordinator : Singleton<OverlayCoordinator> {
    public GameObject imagePrefab;
    public Transform overlayContainer;

    public List<RawImage> images = new List<RawImage>();

    void Start() {
        //int screenWidth = Screen.width;
        //int screenHeight = Screen.height;
        Vector2 offset = new Vector2(0, 0);

        foreach (KeyValuePair<BaseLabel, int> pair in LabelsBucket.GetLabels()) {
            GameObject newImage = Instantiate(imagePrefab);
            newImage.transform.parent = overlayContainer;
            ///Rect fullRect = new Rect(0, 0, screenWidth, screenHeight);

            newImage.GetComponent<RectTransform>().offsetMax = offset;
            newImage.GetComponent<RectTransform>().offsetMin = offset;

            newImage.GetComponent<OverlayShaderController>().SetColorValues(pair.Key.encoderColor, pair.Value, pair.Key.labelName);
            Debug.Log(pair.Key.encoderColor + " " + pair.Value + " " + pair.Key.labelName);
        }
    }

    public static void RenderToPanel(byte[] inputBytes) {
        foreach (RawImage image in Instance.images) {
            Instance.RenderToPanel(image, inputBytes);
        }
    }

    void RenderToPanel(RawImage image, byte[] inputBytes) {
        Vector2 dims = CameraCapture.GetDimentions();
        Texture2D tex = new Texture2D((int)(dims.x), (int)dims.y, TextureFormat.R8, true);
        tex.LoadImage(inputBytes);
        tex.Apply();
        image.texture = tex;
    }
}