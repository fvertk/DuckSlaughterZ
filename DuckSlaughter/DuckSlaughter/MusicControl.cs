using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DuckSlaughter
{
    class MusicControl
    {
        SoundEffectInstance titleMusic;
        SoundEffectInstance gameplayMusic;
        SoundEffectInstance awesomeMusic;

        public int currentSong;
        public int futureSong;
        public bool gameplayStarted;
        
        public MusicControl()
        {
            currentSong = 0;
            futureSong = 0;
            gameplayStarted = false;
        }
        public void Initialize(ContentManager Content)
        {
            titleMusic = Content.Load<SoundEffect>("titleMusic").CreateInstance();
            gameplayMusic = Content.Load<SoundEffect>("progressMusic").CreateInstance();
            awesomeMusic = Content.Load<SoundEffect>("actionMusic").CreateInstance();
        }
        public void Play()
        {
            titleMusic.IsLooped = true;
            gameplayMusic.IsLooped = true;
            awesomeMusic.IsLooped = true;

            titleMusic.Volume = 1.0f;
            gameplayMusic.Volume = 0.0f;
            awesomeMusic.Volume = 0.0f;

            currentSong = 1;
            
            titleMusic.Play();
            gameplayMusic.Play();
            awesomeMusic.Play();
        }
        public void Restart()
        {
            titleMusic.Volume = 1.0f;
            gameplayMusic.Volume = 0.0f;
            awesomeMusic.Volume = 0.0f;

            currentSong = 1;
        }
        public void changeToTitleMusic()
        {
            futureSong = 1;
        }
        public void changeToGameplayMusic()
        {
            futureSong = 2;
        }
        public void changeToAwesomeMusic()
        {
            futureSong = 3;
        }
        public void Update()
        {
            if (futureSong != currentSong)
            {
                // Changing to title music. 
                if (futureSong == 1)
                {
                    // Changing to title music from gameplay music.
                    if (currentSong == 2)
                    {
                        if (gameplayMusic.Volume - 0.01f > 0)
                        {
                            gameplayMusic.Volume -= 0.01f;
                        }
                        if (titleMusic.Volume + 0.01f < 1)
                        {
                            titleMusic.Volume += 0.01f;
                        }
                        if (titleMusic.Volume + 0.01f >= 1.0f)
                        {
                            titleMusic.Volume = 1.0f;
                            currentSong = futureSong;
                            gameplayMusic.Volume = 0.0f;
                            awesomeMusic.Volume = 0.0f;
                        }
                    }
                    // Changing to title music from awesome music. 
                    if (currentSong == 3)
                    {
                        if (awesomeMusic.Volume - 0.01f > 0)
                        {
                            awesomeMusic.Volume -= 0.01f;
                        }
                        if (titleMusic.Volume + 0.01f < 1)
                        {
                            titleMusic.Volume += 0.01f;
                        }
                        if (titleMusic.Volume + 0.01f >= 1.0f)
                        {
                            titleMusic.Volume = 1.0f;
                            currentSong = futureSong;
                            gameplayMusic.Volume = 0.0f;
                            awesomeMusic.Volume = 0.0f;
                        }
                    }

                }
                // Changing to gameplay music. 
                if (futureSong == 2)
                {
                    // Changing to gameplay music from title music.
                    if (currentSong == 1)
                    {
                        if (titleMusic.Volume - 0.001f > 0)
                        {
                            titleMusic.Volume -= 0.001f;
                        }
                        if (gameplayMusic.Volume + 0.001f < 1)
                        {
                            gameplayMusic.Volume += 0.001f;
                        }
                        if (gameplayMusic.Volume + 0.001f >= 1.0f)
                        {
                            gameplayMusic.Volume = 1.0f;
                            currentSong = futureSong;
                            titleMusic.Volume = 0.0f;
                            awesomeMusic.Volume = 0.0f;
                        }
                    }
                    // Changing to gameplay music from awesome music. 
                    if (currentSong == 3)
                    {
                        if (awesomeMusic.Volume - 0.01f > 0)
                        {
                            awesomeMusic.Volume -= 0.01f;
                        }
                        if (gameplayMusic.Volume + 0.01f < 1)
                        {
                            gameplayMusic.Volume += 0.01f;
                        }
                        if (gameplayMusic.Volume + 0.01f >= 1.0f)
                        {
                            gameplayMusic.Volume = 1.0f;
                            currentSong = futureSong;
                            titleMusic.Volume = 0.0f;
                            awesomeMusic.Volume = 0.0f;
                        }
                    }
                }
                // Changing to awesome music. 
                if (futureSong == 3)
                {
                    // Changing to awesome music from title music.
                    if (currentSong == 1)
                    {
                        if (titleMusic.Volume - 0.01f > 0)
                        {
                            titleMusic.Volume -= 0.01f;
                        }
                        if (awesomeMusic.Volume + 0.01f < 1)
                        {
                            awesomeMusic.Volume += 0.01f;
                        }
                        if (awesomeMusic.Volume + 0.01f >= 1.0f)
                        {
                            awesomeMusic.Volume = 1.0f;
                            currentSong = futureSong;
                            titleMusic.Volume = 0.0f;
                            gameplayMusic.Volume = 0.0f;
                        }
                    }
                    // Changing to awesome music from gameplay music. 
                    if (currentSong == 2)
                    {
                        if (gameplayMusic.Volume - 0.01f > 0)
                        {
                            gameplayMusic.Volume -= 0.01f;
                        }
                        if (awesomeMusic.Volume + 0.01f < 1)
                        {
                            awesomeMusic.Volume += 0.01f;
                        }
                        if (awesomeMusic.Volume + 0.01f >= 1.0f)
                        {
                            awesomeMusic.Volume = 1.0f;
                            currentSong = futureSong;
                            titleMusic.Volume = 0.0f;
                            gameplayMusic.Volume = 0.0f;
                        }
                    }
                }
            } 
        }
    }
}
