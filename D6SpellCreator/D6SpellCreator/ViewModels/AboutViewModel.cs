using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace D6SpellCreator.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

        }

        public ICommand OpenWebCommand { get; }
    }
}