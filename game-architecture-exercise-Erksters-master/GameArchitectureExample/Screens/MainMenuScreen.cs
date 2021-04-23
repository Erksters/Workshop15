using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GameArchitectureExample.StateManagement;
using GameArchitectureExample.Screens;
using GameArchitectureExample.GamePlayScreens;

namespace GameArchitectureExample.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
        private ContentManager _content;

        /// <summary>
        /// Menu Music without loop
        /// </summary>
        public SoundEffect MenuMusic;

        /// <summary>
        /// MenuMusic 
        /// </summary>
        public SoundEffectInstance MenuMusicLooped;

        public MainMenuScreen() : base("Main Menu")
        {
            var playGameMenuEntry = new MenuEntry("New Game");
            var LevelSelectMenuEntry = new MenuEntry("Level Select");
            var optionsMenuEntry = new MenuEntry("Options");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += NewGameEntrySelected;
            LevelSelectMenuEntry.Selected += LevelSelectMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(LevelSelectMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);

        }

        public override void Activate()
        {
            if (_content == null) { _content = new ContentManager(ScreenManager.Game.Services, "Content"); }
            if (!AudioConstants.IsPlaying)
            {
                AudioConstants.LoadContent(_content);
                AudioConstants.PlayMenuMusic();                
            }
        }

        private void NewGameEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,  new Tutorial5());
            AudioConstants.StopMenuMusic();
            AudioConstants.PlayRacingMusic();
        }

        private void LevelSelectMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LevelSelectScreen(), null);
        }

        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
