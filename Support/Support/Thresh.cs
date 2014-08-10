#region

using System;
using System.Linq;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace Support {
    internal class Thresh : Champion {

        public Spell Q;
        public Spell W;
        public Spell E;
        public Spell R;

        public Thresh() {
            Utils.PrintMessage("Thresh Loaded");

            Q = new Spell(SpellSlot.Q, 1100f);
            W = new Spell(SpellSlot.W, 950f);
            E = new Spell(SpellSlot.E, 400f);
            R = new Spell(SpellSlot.R, 450f);

            Q.SetSkillshot(0.5f, 60f, 1200f, true, Prediction.SkillshotType.SkillshotLine);
            E.SetSkillshot(0.5f, 160f, float.MaxValue, false, Prediction.SkillshotType.SkillshotLine);
            R.SetTargetted(0.3f, float.MaxValue);
        }

        public override void Drawing_OnDraw(EventArgs args) {
            Spell[] spellList = { Q, W, E, R };
            foreach (var spell in spellList) {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell) {
            if (!GetValue<bool>("InterruptSpells")) return;

            Utils.PrintMessage(spell.ChampionName + ": " + spell.SpellName);

            if (ObjectManager.Player.Distance(unit) < E.Range && E.IsReady()) {
                E.Cast(unit);
            } else if (ObjectManager.Player.Distance(unit) < Q.Range && Q.IsReady()) {
                Q.Cast(unit);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args) {
            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
            var useE = GetValue<bool>("UseE" + (ComboActive ? "C" : "H"));
            //var useR = GetValue<bool>("UseR" + (ComboActive ? "C" : "H"));

            if ((!ComboActive && !HarassActive) || (!Orbwalking.CanMove(100)))
                return;

            if (useE && E.IsReady()) {
                // Pull in on combo, out on harrass.
                if (ComboActive) {
                    var t = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                    if (t != null) {
                        E.Cast(E.GetPrediction(t).Position.To2D().Extend(ObjectManager.Player.ServerPosition.To2D(), Vector3.Distance(ObjectManager.Player.Position, t.Position) + 30));
                    }
                } else {
                    var t = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);
                    E.Cast(t);
                }
            }

            if (useQ && Q.IsReady()) {
                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (Utils.HasBuff(t, "threshqleap") && GetValue<bool>("UseSQ")) {
                    Q.Cast(t);
                } else if (!GetValue<bool>("UseSQ")) {
                    if (Utils.HasBuff(t, "ThreshQ")) {
                        return;
                    }
                }
                Q.Cast(t);
            }
        }

        public override void ComboMenu(Menu config) {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("spacer", "--- Options ---"));
            config.AddItem(new MenuItem("UseSQ" + Id, "Use Second Q").SetValue(true));
            //config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
        }

        public override void HarassMenu(Menu config) {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(false));
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
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
        }

        public override void MiscMenu(Menu config) {
            config.AddItem(new MenuItem("UseRM" + Id, "Use R to Killsteal").SetValue(true));
            config.AddItem(new MenuItem("InterruptSpells" + Id, "Use E to Interrupt Spells").SetValue(true));
        }
    }
}
