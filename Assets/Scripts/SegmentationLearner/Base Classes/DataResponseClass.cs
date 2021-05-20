[System.Serializable]
public class DataResponseClass {
    public string mime;
    public string image64;
    public int[] labels;
    public int[] labelPositionsX;
    public int[] labelPositionsY;
    public float[] confidences;
}
