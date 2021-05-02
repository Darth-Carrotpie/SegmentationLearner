using System.Collections.Generic;
using System.Linq;

public class EventName {
    public class UI {
        public static string ShowScoreScreen() { return "UI_ShowScoreScreen"; }
        public static string ScoreScreenShown() { return "UI_ScoreScreenShown"; }
        public static string ShowCooldownNotReady() { return "ShowCooldownNotReady"; }
        public static List<string> Get() { return new List<string> { ShowScoreScreen(), ScoreScreenShown(), ShowCooldownNotReady() }; }
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
        public static string SceneLoaded() { return "SceneLoaded"; }
        public static List<string> Get() {
            return new List<string> {
                //MapLayoutChanged(),
                //NextScene(), LoadScene(),s
                SceneLoaded()
            }.Concat(Player.Get()).ToList();
        }
    }
    public class AI {
        public static string None() { return null; }
        public static List<string> Get() { return new List<string> { None() }; }
    }

    public static List<string> Get() {
        return new List<string> {}.Concat(UI.Get())
            .Concat(System.Get())
            .Concat(AI.Get())
            //.Concat(PCTest.Get())
            .Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
    }
}