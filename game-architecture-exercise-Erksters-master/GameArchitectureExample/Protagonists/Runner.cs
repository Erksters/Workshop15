using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework.Input;


namespace GameArchitectureExample.Protagonists
{
    public class Runner : Game
    {
        public Game game;

        #region Animation
        /// <summary>
        /// Helps flip the animation in the Draw()
        /// </summary>
        SpriteEffects spriteEffect;

        /// <summary>
        /// Determines which frame to draw in the template
        /// </summary>
        Rectangle source;

        /// <summary>
        /// texture object to display in the Draw()
        /// </summary>
        private Texture2D idleTexture;

        /// <summary>
        /// texture object to display in the Draw()
        /// </summary>
        private Texture2D WalkingTexture;

        /// <summary>
        /// Helps animate the sprite
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// Helps animate the sprite
        /// </summary>
        private short idleFrame;

        /// <summary>
        /// Helps animate the sprite
        /// </summary>
        private short walkingFrame;

        /// <summary>
        /// Helps animate the sprite
        /// </summary>
        private short attackingFrame;

        /// <summary>
        /// Helps animate the sprite
        /// will help loop the animation sprite back to frame 0
        /// </summary>
        private int idleTotalFrames = 3;

        /// <summary>
        /// Helps animate the sprite
        /// will help loop the animation sprite back to frame 0
        /// </summary>
        private int WalkingTotalFrames = 5;

        /// <summary>
        /// Helps animate the sprite
        /// Determines how quickly the draw() goes through frames
        /// Smaller is faster
        /// </summary>
        private float idleAnimationSpeed = (float)0.15;

        /// <summary>
        /// Helps animate the sprite
        /// Determines how quickly the draw() goes through frames
        /// Smaller is faster
        /// </summary>
        private float walkAnimationSpeed = (float)0.15;

        /// <summary>
        /// height of the animations sprite
        /// </summary>
        public static int idleHeight = 24;

        /// <summary>
        /// width of the animations sprite
        /// </summary>
        public static int idleWidth = 24;

        /// <summary>
        /// width of the animations sprite
        /// </summary>
        private int movingWidth = 24;

        /// <summary>
        /// width of the animations sprite
        /// </summary>
        private int movingHeight = 24;
        #endregion

        #region State Of Character
        public Color dinoColor;

        public bool AutoMated;

        /// <summary>
        /// Which way is the player facing
        /// When holding the Left Key, it is true.
        /// When holding the Right Key, it is false.
        /// </summary>
        public bool Flipped;

        /// <summary>
        /// Enumeration for which animation should we draw
        /// </summary>
        public enum AnimateStatus { Idle, Walking}

        /// <summary>
        /// Determines which animation we should draw for the protagonist
        /// </summary>
        public AnimateStatus ProtagonistStatus = AnimateStatus.Walking;
        #endregion

        #region Velocity Constants
        /// <summary>
        /// Used for constant application of speed onto Position property
        /// </summary>
        private float BaseSpeed;

        /// <summary>
        /// Helper attribute for ResetGame()
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Velocity helper and used for speed item upgrades
        /// </summary>
        private int SpeedMultiplier = 1;

        private float stepLag = (float)0.05;
        #endregion

        #region Inputs
        InputAction GoLeft = new InputAction(
            new Buttons[] { Buttons.LeftThumbstickLeft },
            new Keys[] { Keys.A, Keys.Left }, false);

        InputAction GoRight = new InputAction(
            new Buttons[] { Buttons.LeftThumbstickRight },
            new Keys[] { Keys.D, Keys.Right }, false);

        InputAction GoUp = new InputAction(
            new Buttons[] { Buttons.LeftThumbstickUp },
            new Keys[] { Keys.W, Keys.Up }, false);

        InputAction Boost = new InputAction(
            new Buttons[] { Buttons.RightTrigger },
            new Keys[] { Keys.Space, Keys.Z }, false);
        #endregion

        private Random rand;

        /// <summary>
        /// public constructor
        /// </summary>
        public Runner(bool autoMated, float baseSpeed, Color colorDino, Vector2 initialPosition)
        {
            rand = new Random();
            Position = initialPosition;
            dinoColor = colorDino;
            AutoMated = autoMated;
            BaseSpeed = baseSpeed;
            ProtagonistStatus = AnimateStatus.Idle;
        }

        PlayerIndex player;

        /// <summary>
        /// Handles the movement of the protagonist
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="input"></param>
        public void Update(GameTime gameTime, InputState input)
        {
            //Check inputs
            if (started)
            {
                ProtagonistStatus = AnimateStatus.Walking;
                if (finished)
                {
                    ProtagonistStatus = AnimateStatus.Idle;
                }
                else
                {
                    HandleSteps(gameTime, input, player);
                }
            }
        }

        public bool finished;

        public bool started;

        public void StartRunning()
        {
            started = true;
            ProtagonistStatus = AnimateStatus.Walking;
        }

        public void finishRunning()
        {
            finished = true;
            ProtagonistStatus = AnimateStatus.Idle;
        }

        /// <summary>
        /// Handles the movement of the protagonist
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="input"></param>
        public void Update(GameTime gameTime)
        {
            //Check inputs

            if (started)
            {
                ProtagonistStatus = AnimateStatus.Walking;


                if (finished)
                {
                    ProtagonistStatus = AnimateStatus.Idle;
                }
                else
                {
                    HandleAutomatedSteps(gameTime);
                }
            }
        }

        private void HandleAutomatedSteps(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (stepLag < -rand.NextDouble() / 10)
            {
                TakeAStep();
                resetStepLag();
            }
            
            stepLag -= time;
        }

        private bool stepRight = true;
        private bool stepLeft = true;

        private void HandleSteps(GameTime gameTime, InputState input, PlayerIndex Player)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(stepRight && GoRight.Occurred(input, null, out Player) && stepLag < 0)
            {
                TakeAStep();
                resetStepLag();
                stepLeft = true;
                stepRight = false;
            }

            if (stepLeft && GoLeft.Occurred(input, null, out Player) && stepLag < 0)
            {
                resetStepLag();
                TakeAStep();
                stepLeft = false;
                stepRight = true;
            }

            stepLag -= time;    
        }

        private void resetStepLag()
        {
            stepLag = (float)0.05;
        }

        private void TakeAStep()
        {
            Position.X += BaseSpeed * SpeedMultiplier;
        }


        /// <summary>
        /// Load the texture sprite for the animation
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            if(dinoColor == Color.Red) 
            {
                idleTexture = content.Load<Texture2D>("red");
                WalkingTexture = content.Load<Texture2D>("red");
            }
            if (dinoColor == Color.Yellow)
            {
                idleTexture = content.Load<Texture2D>("yellow");
                WalkingTexture = content.Load<Texture2D>("yellow");
            }
            if (dinoColor == Color.Green)
            {
                idleTexture = content.Load<Texture2D>("green");
                WalkingTexture = content.Load<Texture2D>("green");
            }
            if (dinoColor == Color.Blue)
            {
                idleTexture = content.Load<Texture2D>("blue");
                WalkingTexture = content.Load<Texture2D>("blue");
            }
        }

        /// <summary>
        /// The drawing method to display the protagonist
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            handleSpriteEffect();

            if (ProtagonistStatus == AnimateStatus.Walking)
            {
                DrawWalk(spriteBatch);
            }
            else
            {
                DrawIdle(spriteBatch);
            }

        }

        /// <summary>
        /// Helper function to determine if we need to flip the direction of the animation
        /// Used in Draw()
        /// </summary>
        /// <param name="flipped"></param>
        private void handleSpriteEffect()
        {
            if (Flipped)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffect = SpriteEffects.None;
            }
        }

        /// <summary>
        /// Helper method to help condense Draw()
        /// Will draw the Idle animation for the protagonist
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawIdle(SpriteBatch spriteBatch)
        {
            //reset the other frames for animations cleanliness
            walkingFrame = 0;
            attackingFrame = 0;

            //Update the frame
            if (animationTimer > idleAnimationSpeed)
            {
                animationTimer -= idleAnimationSpeed;
                idleFrame++;
            }

            //loop back down to the first frame in the template
            if (idleFrame > idleTotalFrames) { idleFrame = 0; }

            //Redefine the source rectangle because we are using a different template texture
            source = new Rectangle(idleFrame * idleWidth, 0, idleWidth, idleHeight);


            //Draw onto the screen
            spriteBatch.Draw(
                idleTexture,
                new Vector2((int)Position.X, (int)Position.Y),
                source,
                Color.White,
                0f,
                new Vector2(0, 0),
                3f,
                spriteEffect, 0);

        }

        /// <summary>
        /// Helper method to help condense Draw()
        /// Will draw the Walking animation for the protagonist
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawWalk(SpriteBatch spriteBatch)
        {
            //reset the other frames for animations cleanliness
            attackingFrame = 0;
            idleFrame = 0;

            //Update the frame
            if (animationTimer > walkAnimationSpeed)
            {
                animationTimer -= walkAnimationSpeed;
                walkingFrame++;
            }

            //loop back down to the first frame in the template
            if (walkingFrame > WalkingTotalFrames) { walkingFrame = 0; }

            //redefine the source rectangle because we are using a different template texture
            source = new Rectangle(walkingFrame * movingWidth + 24 * 4, 0, movingWidth, movingHeight);

            //Draw onto the screen
            spriteBatch.Draw(
                WalkingTexture,
                new Vector2((int)Position.X, (int)Position.Y),
                source,
                Color.White,
                0f,                
                new Vector2(0, 0),
                3f,
                spriteEffect, 0);
        }
    }
}
