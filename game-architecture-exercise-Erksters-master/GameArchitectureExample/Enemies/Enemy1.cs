using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ParticleSystemExample;

namespace GameArchitectureExample.Enemies
{
    public enum Direction
    {
        Down, Right, Up, Left
    }

    public class Enemy1
    {
        /// <summary>
        /// The direction of the bat.
        /// </summary>
        public Direction Direction;

        /// <summary>
        /// The position of the bat
        /// </summary>
        public Vector2 Position;

        private double directionTimer;

        private double animationTimer;

        private short animationFrame = 1;

        private Texture2D texture;

        public static FireworkParticleSystem FPS;

        private bool Died;

        public Enemy1(Vector2 position)
        {
            Position = position;
            Direction = Direction.Left;
            
        }

        /// <summary>
        /// Loads the bat sprite texture
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("32x32-bat-sprite");
        }

        /// <summary>
        /// Updated bat sprite to face in a direction
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Update Direction timer
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update direction every 2 seconds
            if (directionTimer > 2.0)
            {
                FlyLeftRightOnly();

                //to prevent turn every frame
                directionTimer -= 2;
            }

            MoveTheBat(gameTime);

        }

        /// <summary>
        /// Helper method to clean up the Update() and to move the bat
        /// depending on it's direction
        /// </summary>
        /// <param name="gameTime"></param>
        private void MoveTheBat(GameTime gameTime) 
        {
            switch (Direction)
            {
                case Direction.Up:
                    Position += new Vector2(0, -1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Down:
                    Position += new Vector2(0, 1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Left:
                    Position += new Vector2(-1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Right:
                    Position += new Vector2(1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
        }

        /// <summary>
        /// Helper method to help fly the bat horizontally only
        /// </summary>
        private void FlyLeftRightOnly()
        {
            switch (Direction)
            {
                case Direction.Left:
                    Direction = Direction.Right;
                    break;
                case Direction.Right:
                    Direction = Direction.Left;
                    break;
            }
        }

        public void Die()
        {
            Died = true;
        }
        /// <summary>
        /// Draws the animated bat sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (Died)
            {
                FPS.PlaceFirework(Position);
            }

            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (animationTimer > 0.3)
            {
                animationTimer -= 0.3;
                animationFrame++;
                if (animationFrame > 3)
                {
                    animationFrame = 1;
                }
            }

            var source = new Rectangle(animationFrame * 32, (int)Direction * 32, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White);
        }

    }
}
