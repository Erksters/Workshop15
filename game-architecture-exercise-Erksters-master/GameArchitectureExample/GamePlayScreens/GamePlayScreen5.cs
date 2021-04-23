using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameArchitectureExample.StateManagement;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Joints;
using GameArchitectureExample.Screens;
using GameArchitectureExample.Protagonists;
using GameArchitectureExample.GamePlayScreenBackgrounds.GamePlayScreen5;
using GameArchitectureExample.Enemies;
using ParticleSystemExample;

namespace GameArchitectureExample.GamePlayScreens
{
    public class GamePlayScreen5 : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        #region Screen Transition animations
        private float _pauseAlpha;
        private readonly InputAction _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape, Keys.P }, true);

        #endregion

        #region Game Contents        
        private Vector2 initialPosition = new Vector2(30, 275);

        private int finishLine = GameConstants.inDevelopment ? 7800: 7800;

        public bool started;

        public bool finished;

        public double timer = 450;
        #endregion

        #region Background, MidGround, ForeGround Textures

        Background5 _background;
        #endregion

        #region Runners
        private Runner protagonist;
        private Runner Enemy1;
        private Runner Enemy2;
        private Runner Enemy3;

        private Runner[] runners = new Runner[4];
        private List<Runner> results = new List<Runner>();
        #endregion

        /// <summary>
        /// My public constructor
        /// </summary>
        public GamePlayScreen5()
        {
            if (!GameConstants.inDevelopment)
            {
                TransitionOnTime = TimeSpan.FromSeconds(1.5);
                TransitionOffTime = TimeSpan.FromSeconds(0.5);
            }
            else
            {
                TransitionOnTime = TimeSpan.FromSeconds(0);
                TransitionOffTime = TimeSpan.FromSeconds(0);
            }

            //Initialized Runners
            protagonist = new Runner(false, 42, Color.Red, initialPosition);
            Vector2 Enemy1Position = initialPosition + new Vector2(0, 90);
            Vector2 Enemy2Position = initialPosition + new Vector2(0, 165);
            Vector2 Enemy3Position = initialPosition + new Vector2(0, 250);
            Enemy1 = new Runner(true, 44, Color.Green, Enemy1Position);
            Enemy2 = new Runner(true, 44, Color.Blue, Enemy2Position);
            Enemy3 = new Runner(true, 44 , Color.Yellow, Enemy3Position);

            runners[0] = protagonist;
            runners[1] = Enemy1;
            runners[2] = Enemy2;
            runners[3] = Enemy3;

            GameConstants.ChangeGameScreen(this);
            _background = new Background5();
        }


        /// <summary>
        /// Load in graphics for this Game Plays Screen
        /// </summary>
        public override void Activate()
        {
            if (_content == null) { _content = new ContentManager(ScreenManager.Game.Services, "Content"); }

            _gameFont = _content.Load<SpriteFont>("gamefont");
            _background.LoadContent(_content);
            
            protagonist.LoadContent(_content);

            Enemy1.LoadContent(_content);
            Enemy2.LoadContent(_content);
            Enemy3.LoadContent(_content);
        }

        float startTimer = 0;

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, false);
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            HandlePauseTransition(coveredByOtherScreen);

            GameConstants.FPS.PlaceFirework(new Vector2(RandomHelper.Next(1400), 30));

            //If the race has started
            if(startTimer > 3.00)
            {
                ///If we've won the game return;
                if (finished)
                {
                    GameConstants.LoadResults(results);
                    LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new RaceResultsScreen());
                }

                //If we ran out of time return;
                if (timer < 0)
                {
                    return;
                }

                CheckWinCondition();

                foreach (var participant in runners)
                {
                    participant.StartRunning();
                }

                //If this screen is active
                if (IsActive)
                {
                    //TODO: Add your games update methods here 
                    Enemy1.Update(gameTime);
                    Enemy2.Update(gameTime);
                    Enemy3.Update(gameTime);

                    timer -= time;
                }

            }
            
            startTimer += time;            
        }

        private void CheckWinCondition()
        {
            foreach(var participant in runners)
            {
                if(!participant.finished && participant.Position.X > finishLine)
                {
                    participant.finishRunning();
                    results.Add(participant);
                }                
            }

            if(results.Count == 4)
            {
                finished = true;
            }
        }

        /// <summary>
        /// Method to handle some screen focused inputs such as pause.
        /// I'm sure triple A games will have the "camera mode" handled here too
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="input"></param>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex player;

            if (timer < 0) { return; }

            //Do we need to pause?
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

            protagonist.Update(gameTime, input);

        }

        public float offset;

        public Vector2 startingTextposition = new Vector2(GameConstants.GAME_WIDTH / 2 + 5, GameConstants.GAME_HEIGHT / 2 + 5) ;
        public Vector2 startingTextposition2 = new Vector2(GameConstants.GAME_WIDTH / 2, GameConstants.GAME_HEIGHT / 2);

        /// <summary>
        /// Method for Game drawing
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Calculate our offset vector
            float playerX = MathHelper.Clamp(protagonist.Position.X, 400, finishLine );
            offset = 400 - playerX;

            _background.Draw(gameTime, offset, ScreenManager.SpriteBatch);

            Matrix transform;
            transform = Matrix.CreateTranslation(offset, 0, 0);
            ScreenManager.SpriteBatch.Begin(transformMatrix: transform);
            Enemy1.Draw(gameTime, ScreenManager.SpriteBatch);
            Enemy2.Draw(gameTime, ScreenManager.SpriteBatch);
            Enemy3.Draw(gameTime, ScreenManager.SpriteBatch);
            protagonist.Draw(gameTime, ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();

            ScreenManager.SpriteBatch.Begin(transformMatrix: transform);
            if (finished) { ScreenManager.SpriteBatch.DrawString(_gameFont, "FINISH", new Vector2(protagonist.Position.X - 100, 150), Color.White); }
            ScreenManager.SpriteBatch.End();

            GameConstants.FPS.PlaceFirework(new Vector2(RandomHelper.Next(1400), 30));

            ScreenManager.SpriteBatch.Begin();
            if (0 < startTimer && startTimer < 1)
            {
                ScreenManager.SpriteBatch.DrawString(_gameFont, "3", startingTextposition2, Color.Black, 0f, new Vector2(0,0), 2.2f, SpriteEffects.None, 0);
                ScreenManager.SpriteBatch.DrawString(_gameFont, "3", startingTextposition, Color.White, 0f, new Vector2(0,0), 2f, SpriteEffects.None, 0);
            }
            if (1 < startTimer && startTimer < 2)
            {
                ScreenManager.SpriteBatch.DrawString(_gameFont, "2", startingTextposition2, Color.Black, 0f, new Vector2(0, 0), 2.2f, SpriteEffects.None, 0);
                ScreenManager.SpriteBatch.DrawString(_gameFont, "2", startingTextposition, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
            }
            if (2 < startTimer && startTimer < 3)
            {
                ScreenManager.SpriteBatch.DrawString(_gameFont, "1", startingTextposition2, Color.Black, 0f, new Vector2(0, 0), 2.2f, SpriteEffects.None, 0);
                ScreenManager.SpriteBatch.DrawString(_gameFont, "1", startingTextposition, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
            }
            if (3 < startTimer && startTimer < 4)
            {
                ScreenManager.SpriteBatch.DrawString(_gameFont, "GO!", startingTextposition2, Color.Black, 0f, new Vector2(0, 0), 2.2f, SpriteEffects.None, 0);
                ScreenManager.SpriteBatch.DrawString(_gameFont, "GO!", startingTextposition, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
            }
            ScreenManager.SpriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            HandleScreenTransition();
        }

        /// <summary>
        /// Helper method to animate a transition from another screen (MenuScreen)
        /// Used in Draw()
        /// </summary>
        private void HandleScreenTransition()
        {
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        /// <summary>
        /// Helper method to handle animations for pausing the gamescreen
        /// Used in HandleInput()
        /// </summary>
        /// <param name="coveredByOtherScreen">Tell this method if we are fading in or out</param>
        private void HandlePauseTransition(bool coveredByOtherScreen)
        {
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);
        }

        /// <summary>
        /// Windows phone requirement
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Windows phone requirement
        /// </summary>
        public override void Unload()
        {
            _content.Unload();
        }
    }
}
