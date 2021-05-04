using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class LabelsBucket : Singleton<LabelsBucket> {
    [Header("Building")]
    [Tooltip("Structural things like walls, floors, etc.")]
    [SerializeField] List<BaseLabel> buildingLabels;
    public static List<BaseLabel> BuildingLabels { get { return Instance.buildingLabels; } }
    public static BaseLabel GetBuildingLabel(string labelName) { return BuildingLabels.Where(x => x.labelName == labelName).FirstOrDefault(); }

    [Header("Furniture")]
    [Tooltip("Furniture things like chairs, tables...")]
    [SerializeField] List<BaseLabel> furnitureLabels;
    public static List<BaseLabel> FurnitureLabels { get { return Instance.furnitureLabels; } }
    public static BaseLabel GetFurnitureLabel(string labelName) { return FurnitureLabels.Where(x => x.labelName == labelName).FirstOrDefault(); }

    [Header("Doodads")]
    [Tooltip("Small items like cups, clothes, ...")]
    [SerializeField] List<BaseLabel> itemLabels;
    public static List<BaseLabel> ItemLabels { get { return Instance.itemLabels; } }
    public static BaseLabel GetItemLabel(string labelName) { return ItemLabels.Where(x => x.labelName == labelName).FirstOrDefault(); }

    [Header("Living")]
    [Tooltip("People, animals, etc...")]
    [SerializeField] List<BaseLabel> animalsLabels;
    public static List<BaseLabel> AnimalsLabels { get { return Instance.animalsLabels; } }
    public static BaseLabel GetAnimalLabel(string labelName) { return AnimalsLabels.Where(x => x.labelName == labelName).FirstOrDefault(); }

    public static List<BaseLabel> GetLabels() {
        List<BaseLabel> labels = new List<BaseLabel>();
        labels.AddRange(BuildingLabels);
        labels.AddRange(FurnitureLabels);
        labels.AddRange(ItemLabels);
        labels.AddRange(AnimalsLabels);
        return labels;
    }

    public static BaseLabel GetLabel(string labelName) {
        BaseLabel lb;
        lb = GetBuildingLabel(labelName);
        if (lb != null)return lb;
        lb = GetFurnitureLabel(labelName);
        if (lb != null)return lb;
        lb = GetItemLabel(labelName);
        if (lb != null)return lb;
        lb = GetAnimalLabel(labelName);
        if (lb != null)return lb;
        Debug.LogError("Label not found: " + labelName);
        return null;
    }
    public static BaseLabel GetLabel(LabelIdentity labelIdentity) {
        string labelName = labelIdentity.labelName;
        return GetLabel(labelName);
    }
}