using System;
using System.Collections.Generic;
using System.Text;
using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureExample.Screens
{
    public class SplashScreen : GameScreen
    {
        ContentManager content;
        Texture2D background;
        TimeSpan displayTime;

        public override void Activate()
        {
            base.Activate();

            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");
            background = content.Load<Texture2D>("splash");
            displayTime = TimeSpan.FromSeconds(2);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            displayTime -= gameTime.ElapsedGameTime;

            if (displayTime <= TimeSpan.Zero) ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(background, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.End();
        }
    }
}
