using System.Collections.Generic;
using System.Linq;

public class EventName {
    public class UI {
        public static string LabelMaskChanged() { return "UI_LabelMaskChanged"; }
        public static string ScoreScreenShown() { return "UI_ScoreScreenShown"; }
        public static string ShowCooldownNotReady() { return "ShowCooldownNotReady"; }
        public static List<string> Get() { return new List<string> { LabelMaskChanged(), ScoreScreenShown(), ShowCooldownNotReady() }; }
    }

    public class System {
        public class Player {
            public static string ProfileCreated() { return "System_ProfileCreated"; }
            public static string ProfileUpdate() { return "System_ProfileUpdate"; }
            public static string PlayerCardsSorted() { return "System_PlayerCardsSorted"; }
            public static string Eliminated() { return "System_Eliminated"; }
            public static string PostElimination() { return "System_PostElimination"; }
            public static List<string> Get() { return new List<string> { ProfileCreated(), ProfileUpdate(), PlayerCardsSorted(), Eliminated() }; }
        }
        public static string StartInference() { return "System_StartInference"; }
        public static string StopInference() { return "System_StopInference"; }
        public static List<string> Get() {
            return new List<string> {
                //MapLayoutChanged(),
                //NextScene(), LoadScene(),s
                StartInference(),
                StopInference(),
            }.Concat(Player.Get()).ToList();
        }
    }
    public class AI {
        public static string UpdateOverlay() { return "AI_UpdateOverlay"; }
        public static string None() { return null; }
        public static List<string> Get() { return new List<string> { UpdateOverlay(), None() }; }
    }

    public static List<string> Get() {
        return new List<string> {}.Concat(UI.Get())
            .Concat(System.Get())
            .Concat(AI.Get())
            //.Concat(PCTest.Get())
            .Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
    }
}