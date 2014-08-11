﻿using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Linq;

namespace Support {
    class Morgana :Champion {

        public Spell Q;
        public Spell W;
        public Spell R;

        public Morgana() {
            Utils.PrintMessage("Morgana Loaded");

            Q = new Spell(SpellSlot.Q, 1175f);
            W = new Spell(SpellSlot.W, 900f);
            R = new Spell(SpellSlot.R, 600f);

            Q.SetSkillshot(0.5f, 110f, 1200f, true, Prediction.SkillshotType.SkillshotLine);
            W.SetTargetted(0.5f, float.MaxValue);
            R.SetTargetted(0.5f, float.MaxValue);
        }

        public override void Drawing_OnDraw(EventArgs args) {
            Spell[] spellList = { Q, W, R };
            foreach (var spell in spellList) {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void AntiGapcloser_OnEnemyGapCloser(ActiveGapcloser gapcloser) {
            Q.Cast(gapcloser.Sender);    
        }

        public override void Game_OnGameUpdate(EventArgs args) {
            if ((!ComboActive && !HarassActive) || (!Orbwalking.CanMove(100)))
                return;    

            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseR" + (ComboActive ? "C" : "H"));

            if (useQ && Q.IsReady()) {
                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                Q.Cast(t);
            }

            if (useW && W.IsReady()) {
                var t = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Physical);
                if (Utils.HasBuff(t, "DarkBindingMissile")) {
                    W.Cast(t);
                }
            }

            if (useR && Utils.EnemyInRange(GetValue<Slider>("CountR").Value, R.Range)) {
                // Cast R is enemies are in range :D
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.R);
            }
        }

        public override void ComboMenu(Menu config) {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("spacer", "--- Options ---"));
            config.AddItem(new MenuItem("CountR" + Id, "Num of Enemy in Range to Ult").SetValue(new Slider(1, 5, 0)));
        }

        public override void HarassMenu(Menu config) {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
            //config.AddItem(new MenuItem("UseRH" + Id, "Use R").SetValue(true));
        }

        public override void DrawingMenu(Menu config) {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
        }

        public override void MiscMenu(Menu config) {
            config.AddItem(new MenuItem("AntiGap" + Id, "Anti-Gapclose with E").SetValue(true));
        }

    }
}
