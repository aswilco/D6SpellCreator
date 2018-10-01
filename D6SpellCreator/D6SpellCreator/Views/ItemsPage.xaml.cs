using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using D6SpellCreator.Models;
using D6SpellCreator.Views;
using D6SpellCreator.ViewModels;

using System.IO;
using D6SpellCreator.Persistence;
using SQLite;
using System.Collections.ObjectModel;

namespace D6SpellCreator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        private static ObservableCollection<Spell> _spells;
        private static SQLiteAsyncConnection connectionSpells;
        //public static Audio audio = new Audio();

        public static SQLiteAsyncConnection ConnectionSpells { get => connectionSpells; set => connectionSpells = value; }

        public ItemsPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<Incantations, Spell>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Spell;
                await ConnectionSpells.InsertOrReplaceAsync(newItem);
                _spells.Add(newItem);
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Spell item))
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("dice.mp3");
            player.Play();
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override async void OnAppearing()
        {
            ConnectionSpells = DependencyService.Get<ISQLLiteDB>().GetConnection();
            await ConnectionSpells.CreateTableAsync<Spell>();
           
            ConnectionSpells.CreateTableAsync<Models.Spell.Component>();
            ConnectionSpells.CreateTableAsync<Models.Spell.Incantation>();
            ConnectionSpells.CreateTableAsync<Models.Spell.Gesture>();
            List<Spell> spells = await ConnectionSpells.Table<Spell>().ToListAsync();
            _spells = new ObservableCollection<Spell>(spells);
            ItemsListView.ItemsSource = _spells;
            base.OnAppearing();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}