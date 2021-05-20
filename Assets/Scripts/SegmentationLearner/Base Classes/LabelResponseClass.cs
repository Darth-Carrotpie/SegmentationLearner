[System.Serializable]
public class LabelResponseClass {
    public int label;
    public int labelPositionX;
    public int labelPositionY;

    public override string ToString(){
        return "LabelResponseClass: "+
            label+",  \n X:"+
            labelPositionX+",  \n Y:"+
            labelPositionY+",  \n";

    }
}
