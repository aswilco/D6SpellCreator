
using D6SpellCreator.Models;
using D6SpellCreator.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static D6SpellCreator.Models.Spell;

namespace D6SpellCreator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        private ItemDetailViewModel viewModel;
        private List<Component> components = new List<Component>();

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

            GetComponents();

            foreach (Models.Spell.Component comp in components)
            {
                Comps.Children.Add(new Label { Text = comp.ComponentName, FontSize = 10 });
            }
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Spell
            {

            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        async void GetComponents()
        {
            await ItemsPage.ConnectionSpells.CreateTableAsync<Component>();

            string[] tempstring;
            if (viewModel.Item.ComponentsString.Contains(",")){
                tempstring = viewModel.Item.ComponentsString?.Split(',');
            }
            else
            {
                tempstring = viewModel.Item.ComponentsString?.Split(' ');

            }
            List<int> compIDs = new List<int>();
            int x;
            if (tempstring != null) {
                foreach (string s in tempstring)
                {
                    x = -1;
                    int.TryParse(s, out x);
                    compIDs.Add(x);
                }
                List<Component> componentsList = await ItemsPage.ConnectionSpells.Table<Component>().ToListAsync();
                foreach (int id in compIDs)
                {
                    components.Add(componentsList.Find(c => c.ID == id));


                }
            }
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Button_Clicked_1(object sender, System.EventArgs e)
        {
            ItemsPage.ConnectionSpells.DeleteAsync<Spell>(viewModel.Item);
        }

    }
}