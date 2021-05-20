using System.Collections.Generic;
[System.Serializable]
public class DataResponseClass {
    public string mime;
    public string image64;
    public List<LabelResponseClass> labels;
    public List<float> confidences;

    public override string ToString(){
        return mime+" labelsLen:"+labels.Count+" \n label0"+
            labels[0] + "  \n confs len:"+
            confidences.Count+ " \n";
    }
}
