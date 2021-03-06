﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using D6SpellCreator.Views;
using System.IO;
using SQLite;
using D6SpellCreator.Persistence;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace D6SpellCreator
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new D6SpellCreator.Views.SplashScreen());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
