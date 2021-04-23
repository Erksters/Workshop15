﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameArchitectureExample.GamePlayScreenBackgrounds.GamePlayScreen5
{
    public class Background5 : Game
    {
        private Texture2D _background;

        public void LoadContent(ContentManager content)
        {
            _background = content.Load<Texture2D>("Track");
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">An object representing time in-game</param>
        public void Draw(GameTime gameTime, float offset, SpriteBatch _spriteBatch)
        {
            Matrix transform;

            //Background
            transform = Matrix.CreateTranslation(offset * 0.333f, 0, 0);
            _spriteBatch.Begin(transformMatrix: transform);
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
