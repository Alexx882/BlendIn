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
            audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            audio.Load("sound.wav");
        }



  
    }
}
