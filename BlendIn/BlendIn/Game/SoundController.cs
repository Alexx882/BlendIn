using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlendIn.Game
{
    public class SoundController
    {
        public ISimpleAudioPlayer audio;

        public SoundController()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            audio = CrossSimpleAudioPlayer.Current;
        }

        public void loadStun()
        {
            audio.Load("sound.wav");
            audio.Loop = true;
        }

        public void loadCloak()
        {
            audio.Load("cloak.wav");
            audio.Loop = false;
        }

    }
}
