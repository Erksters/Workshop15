using GameArchitectureExample.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureExample.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {
            /// <summary>
            /// Helps load in fonts
            /// </summary>
            private ContentManager _content;

            /// <summary>
            /// The game font to display useful info on the options screen
            /// </summary>
            private SpriteFont _gameFont;

            /// <summary>
            /// Where I want to show the Master Volume
            /// </summary>
            private Vector2 masterVolumePosition = new Vector2(30, 30);

            /// <summary>
            /// The string to display on screen
            /// </summary>
            private string masterVolume = AudioConstants.MenuMusicLooped.Volume.ToString("0");

            /// <summary>
            /// Languages we could try to support
            /// </summary>
            private static readonly string[] Languages = { "English", "French", "Spanish" };

            /// <summary>
            /// Current list of MenuEntries for this options screen
            /// </summary>
            private readonly MenuEntry _languageMenuEntry;
            private MenuEntry masterVolumeIncreaseEntry;
            private MenuEntry masterVolumeDecreaseEntry;

            /// <summary>
            /// Pointer to the current language in the Languages[]
            /// </summary>
            private static int _currentLanguage;

            /// <summary>
            /// Public constructor
            /// </summary>
            public OptionsMenuScreen() : base($"Options")
            {
                _languageMenuEntry = new MenuEntry(string.Empty);
                masterVolumeIncreaseEntry = new MenuEntry("Increase Master Volume");
                masterVolumeDecreaseEntry = new MenuEntry("Decrease Master Volume");
                var back = new MenuEntry("Back");

                SetMenuEntryText();
                SetVolumeMenuEntryText();

                _languageMenuEntry.Selected += LanguageMenuEntrySelected;
                masterVolumeIncreaseEntry.Selected += IncreaseAudioMenuEntrySelected;
                masterVolumeDecreaseEntry.Selected += DecreaseAudioMenuEntrySelected;
                back.Selected += OnCancel;

                MenuEntries.Add(_languageMenuEntry);
                MenuEntries.Add(masterVolumeIncreaseEntry);
                MenuEntries.Add(masterVolumeDecreaseEntry);
                MenuEntries.Add(back);
            }

            /// <summary>
            /// Method to decrease the MasterVolume of the game
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void DecreaseAudioMenuEntrySelected(object sender, PlayerIndexEventArgs e)
            {
                AudioConstants.DecreaseMasterVolume();
                SetVolumeMenuEntryText();
            }

            /// <summary>
            /// Method to increase the MasterVolume of the game
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void IncreaseAudioMenuEntrySelected(object sender, PlayerIndexEventArgs e)
            {
                AudioConstants.IncreaseMasterVolume();
                SetVolumeMenuEntryText();
            }

            /// <summary>
            /// Method to be used after adjusting the volume
            /// </summary>
            private void SetVolumeMenuEntryText()
            {
                float temp = AudioConstants.MenuMusicLooped.Volume * 10;
                masterVolume = $"Master Volume\n{temp.ToString("0")}";
            }

            /// <summary>
            /// Increments the _currentLanguge value and loops through the languages
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
            {
                _currentLanguage = (_currentLanguage + 1) % Languages.Length;
                SetMenuEntryText();
            }

            /// <summary>
            /// Helper method to load in the correct language
            /// </summary>
            private void SetMenuEntryText()
            {
                _languageMenuEntry.Text = $"Language: {Languages[_currentLanguage]}";
            }

            /// <summary>
            /// Help Draw the HUD for Master Volume
            /// </summary>
            /// <param name="gameTime"></param>
            public override void Draw(GameTime gameTime)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.DrawString(_gameFont, masterVolume, masterVolumePosition, Color.White);
                ScreenManager.SpriteBatch.End();
                base.Draw(gameTime);
            }

            /// <summary>
            /// Load in graphics for this Screen
            /// </summary>
            public override void Activate()
            {
                if (_content == null) { _content = new ContentManager(ScreenManager.Game.Services, "Content"); }
                _gameFont = _content.Load<SpriteFont>("gamefont");
            }
        }
    }
