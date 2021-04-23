using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameArchitectureExample.Screens;
using GameArchitectureExample.StateManagement;
using ParticleSystemExample;

namespace GameArchitectureExample
{
    // Sample showing how to manage different game states, with transitions
    // between menu screens, a loading screen, the game itself, and a pause
    // menu. This main game class is extremely simple: all the interesting
    // stuff happens in the ScreenManager component.
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;
        public static FireworkParticleSystem FPS;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferredBackBufferWidth = 913;
            _graphics.PreferredBackBufferHeight = 685;
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);
            FPS = new FireworkParticleSystem(this, 20) ;

            Components.Add(FPS);
            GameConstants.AddFPSObject(FPS);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);
            if (!GameConstants.inDevelopment)
            {
                _screenManager.AddScreen(new SplashScreen(), null);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);    // The real drawing happens inside the ScreenManager component
        }
    }
}
