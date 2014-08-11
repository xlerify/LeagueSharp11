﻿#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

// Based on the popular marksman framework :D
namespace Support {
    internal class Program {

        public static Menu Config;
        public static Champion champClass = null;

        static void Main(string[] args) {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args) {
            Config = new Menu("Support", "Support", true);

            champClass = new Champion();
            if (ObjectManager.Player.ChampionName == "Thresh")
                champClass = new Thresh();

            if (ObjectManager.Player.ChampionName == "Morgana")
                champClass = new Morgana();

            if (champClass == null)
                Utils.PrintMessage("Champion not supported!");

            champClass.Id = ObjectManager.Player.BaseSkinName;
            champClass.Config = Config;

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            var orbwalking = Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            champClass.Orbwalker = new Orbwalking.Orbwalker(orbwalking);

            var items = Config.AddSubMenu(new Menu("Items", "Items"));
            items.AddItem(new MenuItem("---- To Be added ----", "added"));

            var combo = Config.AddSubMenu(new Menu("Combo", "Combo"));
            champClass.ComboMenu(combo); 

            var harass = Config.AddSubMenu(new Menu("Harass", "Harass"));
            champClass.HarassMenu(harass);

            var misc = Config.AddSubMenu(new Menu("Misc", "Misc"));
            champClass.MiscMenu(misc);

            var drawing = Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            champClass.DrawingMenu(drawing);

            champClass.MainMenu(Config);

            Config.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            Interrupter.OnPosibleToInterrupt += Interrupter_OnPosibleToInterrupt;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;

        }

        static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser) {
            champClass.AntiGapcloser_OnEnemyGapCloser(gapcloser);
        }

        private static void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell) {
            champClass.Interrupter_OnPossibleToInterrupt(unit, spell);
        }

        private static void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target) {
            champClass.Orbwalking_AfterAttack(unit, target);
        }

        private static void Drawing_OnDraw(EventArgs args) {
            throw new NotImplementedException();
        }

        private static void Game_OnGameUpdate(EventArgs args) {
            // Update the combo and harass values.
            champClass.ComboActive = champClass.Config.Item("Orbwalk").GetValue<KeyBind>().Active;
            champClass.HarassActive = champClass.Config.Item("Farm").GetValue<KeyBind>().Active;
            champClass.Game_OnGameUpdate(args);
        }
    }
}
