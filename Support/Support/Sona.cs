using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Support {
    internal class Sona : Champion {

        public Spell Q;
        public Spell W;
        public Spell R;

        public Sona() {
            Utils.PrintMessage("Sona loaded.");

            Q = new Spell(SpellSlot.Q, 700);
            W = new Spell(SpellSlot.W, 1000);
            R = new Spell(SpellSlot.R, 1000);

            // Need to use best AOE position I think.
            R.SetSkillshot(0.5f, 125f, 2400f, false, Prediction.SkillshotType.SkillshotLine);

        }

        public override void Game_OnGameUpdate(EventArgs args) {
            
            if (GetValue<bool>("AutoQ")) {
                if (Q.IsReady()) {
                    // Cast Q when target is in range
                    var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                    if (Vector3.Distance(ObjectManager.Player.Position, t.Position) < Q.Range) {
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q);
                    }
                }
            }

            if (GetValue<bool>("AutoW")) {
                if (W.IsReady()) {
                    // Casts when an Ally is below the set HP threshold.
                    if (Utils.AllyBelowHP(GetValue<Slider>("AutoHeal").Value, W.Range)) {
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
                    }
                }
            }
 
            if ((!ComboActive && !HarassActive) || (!Orbwalking.CanMove(100)))
                return;
             
            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
            var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
            var useR = GetValue<bool>("UseRC");

            if (useQ && Q.IsReady()) {
                // Cast Q when target is in range
                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
                if (Vector3.Distance(ObjectManager.Player.Position, t.Position) < Q.Range) {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q);
                }
            }

            if (useW && W.IsReady()) {
                // Casts when an Ally is below the set HP threshold.
                if (Utils.AllyBelowHP(GetValue<Slider>("AutoHeal").Value, W.Range)) {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
                }
            }

            if (useR && R.IsReady() && ComboActive) {
                var t = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
                var ultTar = Utils.GetEnemyHitByR(R, GetValue<Slider>("CountR").Value);
                if (ultTar != null) {
                    R.Cast(ultTar, true, true);
                }
            }
                

        }

        public override void Drawing_OnDraw(EventArgs args) {
            Spell[] spellList = { Q, W, R };
            foreach (var spell in spellList) {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void ComboMenu(Menu config) {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("spacer", "--- Options ---"));
            config.AddItem(new MenuItem("CountR" + Id, "Min num of Enemy to hit in Ult").SetValue(new Slider(2, 5, 0)));
        }

        public override void HarassMenu(Menu config) {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
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
            config.AddItem(new MenuItem("AutoQ" + Id, "Auto cast Q").SetValue(true));
            config.AddItem(new MenuItem("AutoW" + Id, "Auto cast W").SetValue(true));
            config.AddItem(new MenuItem("AutoHeal" + Id, "Auto W when below % hp").SetValue(new Slider(60, 100, 0)));
        }

    
    }
}
