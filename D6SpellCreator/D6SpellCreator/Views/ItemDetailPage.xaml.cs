using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using D6SpellCreator.Models;
using D6SpellCreator.ViewModels;

namespace D6SpellCreator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            foreach (Models.Spell.Component comp in viewModel.Item.Components)
            {
                Comps.Children.Add(new Label { Text = comp.component, FontSize = 10});
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
    }
}