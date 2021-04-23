using GameArchitectureExample.GamePlayScreens;
using GameArchitectureExample.Protagonists;
using GameArchitectureExample.StateManagement;
using System;
using System.Collections.Generic;

namespace GameArchitectureExample.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class RaceResultsScreen : MenuScreen
    {
        public RaceResultsScreen() : base("Results")
        {
            var returnToMainMenu = new MenuEntry("Continue");
            var Results = new MenuEntry(GameConstants.GenerateResultString());
         
            returnToMainMenu.Selected += ReturnToMainMenu;
            Results.Selected += ReturnToMainMenu;

            MenuEntries.Add(returnToMainMenu);
            MenuEntries.Add(Results);
        }

        private void ReturnToMainMenu(object sender, PlayerIndexEventArgs e)
        {
            AudioConstants.StopRacingMusic();
            AudioConstants.PlayMenuMusic();
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }
     }
}
