using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LabelName {
    public class Building {
        public static string Wall() { return "building_wall"; }
        public static string Floor() { return "building_floor"; }
        public static string Ceiling() { return "building_ceiling"; }
        public static string Door() { return "building_door"; }
        public static List<string> Get() { return new List<string> { Wall(), Floor(), Ceiling(), Door() }; }
    }
    public class Furniture {
        public static string DoubleBed() { return "furniture_doubleBed"; }
        public static string Carpet() { return "furniture_carpet"; }
        public static string Armchair() { return "furniture_armchair"; }
        public static string NightTable() { return "furniture_nightTable"; }
        public static string TvStand() { return "furniture_tvStand"; }
        public static string Shelf() { return "furniture_shelf"; }
        public static string Commode() { return "furniture_commode"; }
        public static string Rug() { return "furniture_rug"; }
        public static string Drawer() { return "furniture_drawer"; }
        public static string Painting() { return "furniture_painting"; }
        public static List<string> Get() { return new List<string> { DoubleBed(), Carpet(), Armchair(), NightTable(), TvStand(), Shelf(), Commode(), Rug(), Drawer(), Painting() }; }
    }
    public class Items {
        public static string Mug() { return "items_mug"; }
        public static string NightLamp() { return "items_nightLamp"; }
        public static string Vase() { return "items_vase"; }
        public static string Cup() { return "items_cup"; }
        public static string Bowl() { return "items_bowl"; }
        public static string Plate() { return "items_plate"; }
        public static string TV() { return "items_tv"; }
        public static List<string> Get() { return new List<string> { Cup(), NightLamp(), Vase(), Bowl(), Mug(), Cup(), Plate(), TV() }; }
    }
    public class Live {
        public static string Man() { return "live_man"; }
        public static string Dog() { return "live_dog"; }
        public static List<string> Get() { return new List<string> { Man(), Dog() }; }
    }
    public static List<string> Get() {
        return new List<string> {}.Concat(Building.Get())
            .Concat(Furniture.Get())
            .Concat(Items.Get())
            .Concat(Live.Get())
            .Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
    }
}