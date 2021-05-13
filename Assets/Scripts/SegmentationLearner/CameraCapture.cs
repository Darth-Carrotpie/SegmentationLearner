using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
public class CameraCapture : Singleton<CameraCapture>
{
    public int FileCounter = 0;

    private Camera normalCamera;
    private Camera labelCamera;

    public RenderTexture rendTex;
    public RenderTexture labTex;

    public static void Setup(Camera normal, Camera label)
    {
        Instance.normalCamera = normal;
        Instance.labelCamera = label;

        Instance.rendTex = new RenderTexture(Instance.normalCamera.targetTexture.width,
            Instance.normalCamera.targetTexture.height,
            16, RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.sRGB);
        Instance.labTex = new RenderTexture(Instance.labelCamera.targetTexture.width,
            Instance.labelCamera.targetTexture.height,
            16, RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.sRGB);
    }

    public byte[] CaptureRend()
    {
        Camera Cam = normalCamera;
        Cam.targetTexture = rendTex;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();
        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height, TextureFormat.RGBA32, false);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();

        return Image.EncodeToPNG();
    }

    public void SavePics(Camera Cam, string subfolder, TextureFormat format, bool linear)
    {
        //RenderTexture currentRT = RenderTexture.active;
        if (subfolder == "labels/")
        {
            Cam.targetTexture = labTex;
        }
        else
        {
            Cam.targetTexture = rendTex;
        }
        RenderTexture.active = Cam.targetTexture;


        Cam.Render();
        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height, format, linear);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        //RenderTexture.active = currentRT;
        if (subfolder == "labels/")
        {
            //Debug.Log("sRGB: "+RenderTexture.active.sRGB);
            //Debug.Log("mipmap: "+RenderTexture.active.useMipMap);
            List<Color> colors = new List<Color>(Image.GetPixels().Distinct().ToList());
            string output = FileCounter + ".png mask vals: ";
            foreach (Color32 col in colors)
            {
                output += col.r + "; ";
            }
            Debug.Log(output);
        }
        Image.Apply();

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);
        string itemPath = Application.dataPath + "/Data~/" + subfolder + FileCounter + ".png";
        Debug.Log("saving image to:" + itemPath);
        File.WriteAllBytes(itemPath, Bytes);
    }

    public void DeletePics(string subfolder)
    {
        string itemPath = Application.dataPath + "/Data~/" + subfolder;
        var picFile = Directory.GetFiles(itemPath);
        int deletedCount = 0;
        for (int i = 0; i < picFile.Length; i++)
        {
            if (File.Exists(picFile[i]))
            {
                File.Delete(picFile[i]);
                deletedCount++;
            }
        }
        Debug.Log("Deleted " + deletedCount + " files in " + subfolder);
    }

    public static void Capture()
    {
        Instance.SavePics(Instance.normalCamera, "screenshots/", TextureFormat.RGBA32, false);
        Instance.SavePics(Instance.labelCamera, "labels/", TextureFormat.R8, true);
        Instance.FileCounter++;
    }
    public static byte[] CaptureScreenshot()
    {
        return Instance.CaptureRend();
    }
    public static void RemoveCaptured()
    {
        Instance.DeletePics("screenshots/");
        Instance.DeletePics("labels/");
    }
}