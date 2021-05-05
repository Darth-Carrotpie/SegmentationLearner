using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraCapture : Singleton<CameraCapture> {
    public int FileCounter = 0;

    private Camera normalCamera;
    private Camera labelCamera;

    public static void Setup(Camera normal, Camera label) {
        Instance.normalCamera = normal;
        Instance.labelCamera = label;
    }

    public void SavePics(Camera Cam, string subfolder) {

        //RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();
        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();
        //RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);
        string itemPath = Application.dataPath + "/Data/" + subfolder + FileCounter + ".png";
        Debug.Log("saving image to:" + itemPath);
        File.WriteAllBytes(itemPath, Bytes);
    }

    public void DeletePics(string subfolder) {
        string itemPath = Application.dataPath + "/Data/" + subfolder;
        var picFile = Directory.GetFiles(itemPath);
        int deletedCount = 0;
        for (int i = 0; i < picFile.Length; i++) {
            if (File.Exists(picFile[i])) {
                File.Delete(picFile[i]);
                deletedCount++;
            }
        }
        Debug.Log("Deleted " + deletedCount + " files in " + subfolder);
    }

    public static void Capture() {
        Instance.SavePics(Instance.normalCamera, "screenshots/");
        Instance.SavePics(Instance.labelCamera, "labels/");
        Instance.FileCounter++;
    }
    public static void RemoveCaptured() {
        Instance.DeletePics("screenshots/");
        Instance.DeletePics("labels/");
    }
}