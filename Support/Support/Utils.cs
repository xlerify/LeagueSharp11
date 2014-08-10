#region

using LeagueSharp;

#endregion

namespace Support {
    internal static class Utils {
        public static void PrintMessage(string message) {
            Game.PrintChat("<font color='#1600DB'>Support:</font> <font color='#FFFFFF'>" + message + "</font>");
        }

        public static bool HasBuff(Obj_AI_Hero target, string buffName) {

            foreach (BuffInstance buff in target.Buffs) {
                //Utils.PrintMessage(buff.Name);
                if (buff.Name.ToLower() == buffName.ToLower()) {
                    return true;
                }
            }

            return false;
        }

        public static bool HasBuff(Obj_AI_Hero target, string buffName, bool debug) {

            foreach (BuffInstance buff in target.Buffs) {
                if (debug) {
                    Utils.PrintMessage(buff.Name);
                }
                if (buff.Name.ToLower() == buffName.ToLower()) {
                    return true;
                }
            }

            return false;
        }
    }
}