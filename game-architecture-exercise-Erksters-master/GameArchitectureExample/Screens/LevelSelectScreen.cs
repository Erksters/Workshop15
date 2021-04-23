using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GameArchitectureExample.StateManagement;
using GameArchitectureExample.Screens;
using GameArchitectureExample.GamePlayScreens;

namespace GameArchitectureExample.Screens
{
    public class LevelSelectScreen : MenuScreen
    {
        public LevelSelectScreen() : base("Level Select")
        {
            var Level1Entry = new MenuEntry("1");
            var Level2Entry = new MenuEntry("2");
            var Level3Entry = new MenuEntry("3");
            var Level4Entry = new MenuEntry("4");
            var Level5Entry = new MenuEntry("5");
            var exitMenuEntry = new MenuEntry("Back");

            Level1Entry.Selected += Level1EntrySelected;
            Level2Entry.Selected += Level2EntrySelected;
            Level3Entry.Selected += Level3EntrySelected;
            Level4Entry.Selected += Level4EntrySelected;
            Level5Entry.Selected += Level5EntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(Level1Entry);
            MenuEntries.Add(Level2Entry);
            MenuEntries.Add(Level3Entry);
            MenuEntries.Add(Level4Entry);
            MenuEntries.Add(Level5Entry);
            MenuEntries.Add(exitMenuEntry);

        }

        /// <summary>
        /// Send the user to a GamePlayScreen #
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Level1EntrySelected(object Sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GamePlayScreen());

        }

        /// <summary>
        /// Send the user to a GamePlayScreen #
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Level2EntrySelected(object Sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GamePlayScreen1());
        }

        /// <summary>
        /// Send the user to a GamePlayScreen #
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Level3EntrySelected(object Sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GamePlayScreen3()) ;
        }

        /// <summary>
        /// Send the user to a GamePlayScreen #
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Level4EntrySelected(object Sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GamePlayScreen4());
        }

        /// <summary>
        /// Send the user to a GamePlayScreen #
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Level5EntrySelected(object Sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GamePlayScreen5());
        }
    }
}
