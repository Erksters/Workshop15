using System;
using System.Collections.Generic;
using System.Text;
using GameArchitectureExample.Protagonists;
using GameArchitectureExample.Screens;
using Microsoft.Xna.Framework;
using ParticleSystemExample;

namespace GameArchitectureExample
{
    public static class GameConstants
    {
        /// <summary>
        /// For Development purposes
        /// True will make the game easier to test
        /// False will should be ready for production
        /// </summary>
        public static bool inDevelopment = true;

        /// <summary>
        /// The width of the game world
        /// </summary>
        public static int GAME_WIDTH = 760;

        /// <summary>
        /// The height of hte game world
        /// </summary>
        public static int GAME_HEIGHT = 480;

        /// <summary>
        /// Get the current Game Screen
        /// </summary>
        public static GameArchitectureExample.GamePlayScreens.IGamePlayScreen currentGameScreen;

        //public static void ResetGame()
        //{
        //    currentGameScreen.Reset();
        //}

        /// <summary>
        /// Set the current GameScreen
        /// </summary>
        /// <param name="gameScreen"></param>
        public static void ChangeGameScreen(GameArchitectureExample.GamePlayScreens.IGamePlayScreen gameScreen)
        {
            currentGameScreen = gameScreen;
        }

        public static FireworkParticleSystem FPS;

        public static void AddFPSObject(FireworkParticleSystem fps)
        {
            FPS = fps;
        }



        public static List<Runner> results;
        public static  void LoadResults(List<Runner> list)
        {
            results = list;
        }

        public static string GenerateResultString()
        {
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < results.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        temp.Append("1st: " + myColorDict[results[i].dinoColor] + "\n");
                        break;
                    case 1:
                        temp.Append("2nd: " + myColorDict[results[i].dinoColor] + "\n");
                        break;
                    case 2:
                        temp.Append("3rd: " + myColorDict[results[i].dinoColor] + "\n");
                        break;
                    case 3:
                        temp.Append("4th: " + myColorDict[results[i].dinoColor] + "\n");
                        break;
                }
                    
                
            }
            
            return temp.ToString();
        }

        private static Dictionary<Color, String> myColorDict = new Dictionary<Color, string>() 
        { 
            {Color.Red, "Red" },
            {Color.Blue, "Blue" },
            {Color.Green, "Green" },
            {Color.Yellow, "Yellow" }
        };

    }
}
