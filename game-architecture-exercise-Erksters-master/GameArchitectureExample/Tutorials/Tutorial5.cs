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
    public class Tutorial5 : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        #region Screen Transition animations
        private float _pauseAlpha;
        private readonly InputAction _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape, Keys.P }, true);
        #endregion

        #region Tutorial Contents        
        private Texture2D WASDAnimation;

        public double timer = 5;
        #endregion

        #region Animation
        private float animationSpeed = (float)0.15;
        private int totalFrames = 2;
        private int animationHeight = 755;
        private int animationWidth = 750;
        private double animationTimer;

        Rectangle source;

        #endregion


        /// <summary>
        /// My public constructor
        /// </summary>
        public Tutorial5()
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

            GameConstants.ChangeGameScreen(this);
        }


        /// <summary>
        /// Load in graphics for this Game Plays Screen
        /// </summary>
        public override void Activate()
        {
            if (_content == null) { _content = new ContentManager(ScreenManager.Game.Services, "Content"); }

            _gameFont = _content.Load<SpriteFont>("gamefont");
            WASDAnimation = _content.Load<Texture2D>("Instructions");
        
        }


        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, false);
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            HandlePauseTransition(coveredByOtherScreen);

                //If we ran out of time return;
                if (timer < 0)
                {
                LoadingScreen.Load(ScreenManager, false, null, new GamePlayScreen5());
            }

            timer -= time;
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

        }

        public Vector2 InstuctionalTextPosition = new Vector2(20, GameConstants.GAME_HEIGHT / 3 );
        public Vector2 WASDPosition = new Vector2(20, GameConstants.GAME_HEIGHT / 5);

        private int tutorialFrame = 0;
        SpriteEffects spriteEffect;

        /// <summary>
        /// Method for Game drawing
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > animationSpeed)
            {
                animationTimer -= animationSpeed;
                tutorialFrame++;
            }

            if(tutorialFrame > totalFrames -1 )
            {
                tutorialFrame = 0;
            }

            source = new Rectangle(tutorialFrame * animationWidth, 0, animationWidth, animationHeight);

            //Draw onto the screen
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(
                WASDAnimation,
                WASDPosition,
                source,
                Color.White,
                0f,
                new Vector2(0, 0),
                1f,
                spriteEffect, 0);

            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            
            ScreenManager.SpriteBatch.DrawString(_gameFont, "Press     the     Left     and     Right     Keys     quickly     to     move!", InstuctionalTextPosition, Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0);
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
