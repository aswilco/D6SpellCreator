using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using D6SpellCreator.Persistence;
using Xamarin.Forms;
using static Android.Provider.MediaStore;

[assembly:Dependency(typeof(Audio))]
namespace D6SpellCreator.Droid
{
    public class Audio : IAudio
    {
        private MediaPlayer _mediaPlayer;

        public bool PlayAudio()
        {
            _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.dice);
            _mediaPlayer.Start();
            return true;
        }
    }
}