
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantsBucket : Singleton<ConstantsBucket>
{
    [Header("Level Generation")]
    [SerializeField] float heightBias = 5f;
    public static float HeightBias { get { return Instance.heightBias; } }
    [SerializeField] float heightDelta = 2f;
    public static float HeightDelta { get { return Instance.heightDelta; } }


    [Header("Screen Capture")]
    [SerializeField] int texWidth = 960;//640;
    public static int TexWidth { get { return Instance.texWidth; } }
    [SerializeField] int texHeight = 540;//360;
    public static int TexHeight { get { return Instance.texHeight; } }

    [Header("Inference")]
    [SerializeField] float targetFPS = 20;//4;
    public static float TargetFPS { get { return Instance.targetFPS; } }

}
