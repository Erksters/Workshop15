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

namespace GameArchitectureExample.GamePlayScreens
{
    public class GamePlayScreen1 : GameScreen
    {
    
        private ContentManager _content;
        private SpriteFont _gameFont;

        #region Screen Transition animations
        private float _pauseAlpha;
        private readonly InputAction _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape, Keys.P }, true);

        private readonly InputAction reset = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.R }, true);
        #endregion

        #region Game Contents
        private Protagonist1 protagonist;
        private World world;
        private Vector2 initialPosition = new Vector2(300, 300);
        private Vector2 gravityForce = new Vector2(0, 150);

        /// <summary>
        /// flag attribute to determine if we have won the game.
        /// </summary>
        public bool win;

        /// <summary>
        /// Once the timer reaches 0, the player will lose the game. 
        /// </summary>
        public double timer = 10;
        #endregion

        /// <summary>
        /// My public constructor
        /// </summary>
        public GamePlayScreen1()
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

            world = new World();
            world.Gravity = gravityForce;
            GenerateBoundaries();
            protagonist = new Protagonist1(CreateProtagonistBody());
            GameConstants.ChangeGameScreen(this);
        }

        /// <summary>
        /// Method to help reset this game. 
        /// </summary>
        public void Reset()
        {
            world = new World();
            world.Gravity = gravityForce;
            GenerateBoundaries();
            protagonist.Reset(CreateProtagonistBody());
            protagonist.ProtagonistBody.Position = initialPosition;
            win = false;
            timer = 10;
        }

        /// <summary>
        /// Helper method to clean up my public constructor
        /// Creates the body for the protagonist.
        /// </summary>
        /// <returns></returns>
        private Body CreateProtagonistBody()
        {
            Body rigidBody = world.CreateRectangle(Protagonist1.idleWidth, Protagonist1.idleHeight,
                10, initialPosition, 0, BodyType.Dynamic);
            rigidBody.SetRestitution(0);

            return (rigidBody);
        }

        /// <summary>
        /// Load in graphics for this Game Plays Screen
        /// </summary>
        public override void Activate()
        {
            if (_content == null) { _content = new ContentManager(ScreenManager.Game.Services, "Content"); }

            _gameFont = _content.Load<SpriteFont>("gamefont");
            protagonist.LoadContent(_content);
        }

        /// <summary>
        /// Helper method to create boundaries for our world.
        /// Currently invisible. I need them to collide with my protagonist so that he knows
        /// he may jump again.
        /// </summary>
        private void GenerateBoundaries()
        {
            var top = 0;
            var bottom = GameConstants.GAME_HEIGHT - 50;
            var left = 0;
            var right = GameConstants.GAME_WIDTH;
            var edges = new Body[]
            {
                world.CreateEdge(new Vector2(left, top), new Vector2(right, top)),
                world.CreateEdge(new Vector2(right, top), new Vector2(right, bottom)),
                world.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom)),
                world.CreateEdge(new Vector2(left, top), new Vector2(left, bottom )),
            };
            foreach (var edge in edges)
            {
                edge.BodyType = BodyType.Static;
                edge.SetRestitution(0);
            };
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            HandlePauseTransition(coveredByOtherScreen);


            ///If we've won the game return;
            if (win)
            {
                return;
            }

            //If we ran out of time return;
            if (timer < 0.01)
            {
                return;
            }

            CheckWinCondition();

            //If this screen is active
            if (IsActive)
            {
                //TODO: Add your games update methods here 

                //I wanted to speed up this game motion.
                world.Step(time);
                world.Step(time);
                world.Step(time);

                timer -= time;
            }
        }

        private void CheckWinCondition()
        {
            if (protagonist.ProtagonistBody.Position.X > GameConstants.GAME_WIDTH)
            {
                win = true;
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

            //Do we need to pause?
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

            if (reset.Occurred(input, ControllingPlayer, out player))
            {
                Reset();
            }
            if (win) { return; }
            if (timer < 0) { return; }
            protagonist.Update(gameTime, input);
        }

        /// <summary>
        /// Method for Game drawing
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            ScreenManager.SpriteBatch.Begin();
            protagonist.Draw(gameTime, ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.DrawString(_gameFont, "Move     Right", new Vector2(50, 50), Color.White);
            ScreenManager.SpriteBatch.DrawString(_gameFont, $"Time     Left {timer.ToString("0.00")}", new Vector2(50, 100), Color.White);
            if (timer < 0) { ScreenManager.SpriteBatch.DrawString(_gameFont, "You     Lost", new Vector2(100, 150), Color.White); }
            if (timer < 0) { ScreenManager.SpriteBatch.DrawString(_gameFont, "Press     R     to     restart", new Vector2(100, 200), Color.White); }
            if (win) { ScreenManager.SpriteBatch.DrawString(_gameFont, $"You     win", new Vector2(100, 150), Color.White); }

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
