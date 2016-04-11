using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Prototype.ScreenManager
{
    public static class MusicManager
    {
        private static Dictionary<String, Song> songs;
        private static Boolean starting;
        private static Boolean ending;
        private static Boolean transitioning;
        private static String nextSong;

        public static void Initialize()
        {
            songs = new Dictionary<String, Song>();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsMuted = false;
        }

        public static void LoadContent(ContentManager content)
        {
            Song song;
            song = content.Load<Song>("Audio/Songs/menu");
            songs.Add("menuSong", song);
            song = content.Load<Song>("Audio/Songs/inGame");
            songs.Add("inGameSong", song);
            song = content.Load<Song>("Audio/Songs/endANDgameOver");
            songs.Add("gameOverSong", song);

            //hornEffect = content.Load<SoundEffect>("Audio/SoundEffects/hornCallBagpipe");
        }



        public static void startSong(String songName)
        {
            if (songs.ContainsKey(songName))
            {
                Song song;
                songs.TryGetValue(songName, out song);
                MediaPlayer.Volume = 0;
                MediaPlayer.Play(song);
                starting = true;
            }
        }

        public static void endSong()
        {
            MediaPlayer.Volume = 1;
            ending = true;
        }

        public static void changeSong(String songName)
        {
            MediaPlayer.Volume = 1;
            transitioning = true;
            nextSong = songName;
        }
        public static void Update(GameTime gameTime)
        {
            if (starting == true)
            {
                MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume + 0.01f, 0, 1);
                if (MediaPlayer.Volume == 1.0)
                {
                    starting = false;
                }
            }
            else if (ending == true)
            {
                MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume - 0.01f, 0, 1);
                if (MediaPlayer.Volume == 0.0)
                {
                    ending = false;
                    MediaPlayer.Stop();
                }
            }
            else if (transitioning == true)
            {
                MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume - 0.01f, 0, 1);
                if (MediaPlayer.Volume == 0.0)
                {
                    transitioning = false;
                    MediaPlayer.Stop();
                    startSong(nextSong);
                }
            }
        }
    }
}
