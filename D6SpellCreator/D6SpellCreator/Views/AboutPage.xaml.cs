using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D6SpellCreator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            OpenGLText.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OpenGLClicked())
            });
            BTTText.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => BTTClicked())
            });

        }

        private void BTTClicked()
        {
            Device.OpenUri(new Uri("https://bonethrowerstheater.com/"));
        }

        private void OpenGLClicked()
        {
            Device.OpenUri(new Uri("http://opend6project.org/"));
        }
    }
}