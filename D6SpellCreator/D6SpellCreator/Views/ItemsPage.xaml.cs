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
        private static SQLiteAsyncConnection _connection;
        private static ObservableCollection<Spell> _spells;

        public ItemsPage()
        {
            InitializeComponent();
            _connection = DependencyService.Get<ISQLLiteDB>().GetConnection();

            MessagingCenter.Subscribe<Incantations, Spell>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Spell;
                await _connection.InsertOrReplaceAsync(newItem);
                _spells.Add(newItem);
            });

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Spell;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Spell>();
            var spells = await _connection.Table<Spell>().ToListAsync();
            _spells = new ObservableCollection<Spell>(spells);
            ItemsListView.ItemsSource = _spells;
            base.OnAppearing();
        }

      
    }
}