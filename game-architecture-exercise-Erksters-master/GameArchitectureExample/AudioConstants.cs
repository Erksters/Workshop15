using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameArchitectureExample
{
    public static class AudioConstants
    {
        /// <summary>
        /// Menu Music without loop
        /// </summary>
        public static SoundEffect MenuMusic;

        /// <summary>
        /// MenuMusic 
        /// </summary>
        public static SoundEffectInstance MenuMusicLooped;

        /// <summary>
        /// Racing Music without loop
        /// </summary>
        public static SoundEffect RacingMusic;

        /// <summary>
        /// Racing Music with Loop
        /// </summary>
        public static SoundEffectInstance RacingMusicLooped;

        /// <summary>
        /// MenuMusic Volume
        /// </summary>
        public static SoundEffectInstance MasterVolume;

        public static bool IsPlaying;

        /// <summary>
        /// Add the content of the sound
        /// </summary>
        /// <param name="content"></param>
        public static void LoadContent(ContentManager content)
        {
            MenuMusic = content.Load<SoundEffect>("Grand-Adventure");
            MenuMusicLooped = MenuMusic.CreateInstance();
            MenuMusicLooped.IsLooped = true;

            RacingMusic = content.Load<SoundEffect>("Trimed_DDD");
            RacingMusicLooped= RacingMusic.CreateInstance();
            RacingMusicLooped.IsLooped = true;
        }

        public static void SetNewMenuMusicVolume(int newVolume)
        {
            MenuMusicLooped.Volume = newVolume;
            RacingMusicLooped.Volume = newVolume;
        }

        public static void StopMenuMusic()
        {
            MenuMusicLooped.Stop();
            IsPlaying = false;
        }

        public static void PlayMenuMusic()
        {
            MenuMusicLooped.Play();
            IsPlaying = true;
        }

        public static void StopRacingMusic()
        {
            RacingMusicLooped.Stop();
            IsPlaying = false;
        }

        public static void PlayRacingMusic()
        {
            RacingMusicLooped.Play();
            IsPlaying = true;
        }

        public static void DecreaseMasterVolume()
        {
            if (MenuMusicLooped.Volume >= (float)0.10) { MenuMusicLooped.Volume += (float)-0.10; }
            else
            {
                MenuMusicLooped.Volume = 0;
            }

            if (RacingMusicLooped.Volume >= (float)0.10) { RacingMusicLooped.Volume += (float)-0.10; }
            else
            {
                RacingMusicLooped.Volume = 0;
            }
        }

        public static void IncreaseMasterVolume()
        {
            if (MenuMusicLooped.Volume <= (float)0.90) { MenuMusicLooped.Volume += (float)0.10; }
            else
            {
                MenuMusicLooped.Volume = 1;
            }
            if (RacingMusicLooped.Volume <= (float)0.90) { RacingMusicLooped.Volume += (float)0.10; }
            else
            {
                RacingMusicLooped.Volume = 1;
            }
        }
    }
}
