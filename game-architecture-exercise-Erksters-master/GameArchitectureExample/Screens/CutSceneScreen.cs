using System;
using System.Collections.Generic;
using System.Text;
using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using GameArchitectureExample.GamePlayScreens;

namespace GameArchitectureExample.Screens
{
    public class CutSceneScreen : GameScreen
    {
        /// <summary>
        /// Has access to this Games contents. Helps find the videos i think.
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// Object to load the video
        /// </summary>
        private Video video;

        /// <summary>
        /// Video player to play the loaded video
        /// </summary>
        private VideoPlayer videoPlayer;

        /// <summary>
        /// Determines if we are actively playing a cutscene or not. Helps cut out the master music
        /// </summary>
        private bool isPlaying;

        /// <summary>
        /// If the user want's to skip the cutscene, then they may do so with this input action
        /// </summary>
        private InputAction skip = new InputAction(
            new Buttons[] { Buttons.A, Buttons.Start},
            new Keys[] { Keys.Space, Keys.Enter}, true); 
        
        /// <summary>
        /// Determines which scene to play and which gameplay screen to start
        /// </summary>
        private int whichScene;

        PlayerIndex player;


        /// <summary>
        /// public constructor
        /// </summary>
        /// <param name="sceneNumber"></param>
        public CutSceneScreen(int sceneNumber)
        {
            whichScene = sceneNumber;
            videoPlayer = new VideoPlayer();
        }

        /// <summary>
        /// Determines what to do upon being activated.
        /// Loads the video contents
        /// </summary>
        public override void Activate()
        {
            if(content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            if (whichScene == 1)
            {
                video = content.Load<Video>("liftoff_of_smap");
            }

            else if (whichScene == 2)
            {
                video = content.Load<Video>("Park");
            }

            else if (whichScene == 3)
            {
                video = content.Load<Video>("for Youtube");
            }
            else { }
        }

        /// <summary>
        /// If the user skips/replays/pauses the scene, handle that here
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="input"></param>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if(!isPlaying)
            {
                videoPlayer.Play(video);
                isPlaying = true;
            }
            if (skip.Occurred(input, null, out player))
            {
                videoPlayer.Stop();
                KillAllScreens();
                StartNextGameplayScreen();
            }
        }

        /// <summary>
        /// Continue playing the scene until it's over. Then kill all screens on the ScreenManager list and start the game
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //Once cutscene is over
            if (videoPlayer.PlayPosition >= video.Duration) {
                KillAllScreens();
                StartNextGameplayScreen();
            }
        }

        /// <summary>
        /// Helper method to play the next gameplay screen
        /// </summary>
        private void StartNextGameplayScreen()
        {
            if (whichScene == 1)
            {
                ScreenManager.AddScreen(new GamePlayScreen(), player);
            }
            if (whichScene == 2)
            {
                ScreenManager.AddScreen(new GamePlayScreen1(), player);
            }
            if (whichScene == 3)
            {
                ScreenManager.AddScreen(new GamePlayScreen3(), player);
            }
        }

        /// <summary>
        /// Helper method to kill all of the ScreenManagers screens
        /// </summary>
        private void KillAllScreens()
        {
            foreach(var screen in ScreenManager.GetScreens())
            {
                screen.ExitScreen();
            }
        }

        /// <summary>
        /// Ensures we stop playing the scene upon deactivation
        /// </summary>
        public override void Deactivate()
        {
            videoPlayer.Pause();
            isPlaying = false;
        }

        /// <summary>
        /// Method to draw the film on the screen. 
        /// If we are playing, then yes continue drawing the scene on the screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (isPlaying)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.Draw(videoPlayer.GetTexture(), Vector2.Zero, Color.White);
                ScreenManager.SpriteBatch.End();
            }

        }
    }
}
