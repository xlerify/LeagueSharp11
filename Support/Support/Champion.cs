﻿#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support {
    internal class Champion {

        public bool ComboActive;
        public Menu Config;
        public bool HarassActive;
        public string Id = "";
        public Orbwalking.Orbwalker Orbwalker;

        public T GetValue<T>(string item) {
            return Config.Item(item + Id).GetValue<T>();
        }

        public virtual void ComboMenu(Menu config) {
        }

        public virtual void HarassMenu(Menu config) {
        }

        public virtual void ItemMenu(Menu config) {
        }

        public virtual void MiscMenu(Menu config) {
        }

        public virtual void DrawingMenu(Menu config) {
        }

        public virtual void ManaMenu(Menu config) {
        }

        public virtual void MainMenu(Menu config) {
        }

        public virtual void Drawing_OnDraw(EventArgs args) {
        }

        public virtual void Game_OnGameUpdate(EventArgs args) {
        }

        public virtual void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target) {

        }

        public virtual void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell) {

        }

        public virtual void AntiGapcloser_OnEnemyGapCloser(ActiveGapcloser gapcloser) {
            
        }
    }
}