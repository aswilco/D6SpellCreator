using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static D6SpellCreator.Models.Spell;

namespace D6SpellCreator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Incantations : ContentPage
    {
        Models.Spell thisSpell;
        List<Incantation> incantations;
        Incantation thisIncantation;

        string[] ComponentComplexity = { "Word or Phrase", "Sentence or Lengthy Phrase", "Complex Incantation (difficulty 11)",
            "Litany (difficulty 15)", "Complex Formula (difficulty 19)", "Extensive Complex Elements (difficulty 23)", "Unique" };

        public Incantations(Models.Spell spell)
        {
            thisSpell = spell;
            InitializeComponent();
            IncantationComplexity.ItemsSource = ComponentComplexity;
            SetValueLabel();
        }

        async void Preview(object sender, EventArgs e)
        {
            var navPage = new NavigationPage(new ItemsPage());
            IncantationChanged(null, null);
            thisSpell.ID = await ItemsPage.ConnectionSpells.Table<Models.Spell>().CountAsync();
            MessagingCenter.Send(this, "AddItem", thisSpell);
            Application.Current.MainPage = navPage;
        }

        private async void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + await thisSpell.GetDifficultyAsync();
        }

        private async void IncantationChanged(object sender, EventArgs e)
        {
            if (thisIncantation == null && IncantationEntry.Text != "None")
            {
                thisIncantation = new Models.Spell.Incantation
                {
                    ID = await ItemsPage.ConnectionSpells.Table<Incantation>().CountAsync(),
                    IncantationText = IncantationEntry.Text,
                    Value = IncantationComplexity.SelectedIndex,
                    Offensive = Offensive.IsToggled,
                    ForeignLanguage = FL.IsToggled,
                    Loud = Loud.IsToggled

                };
                thisSpell.MyIncantation = thisIncantation.ID;
                await ItemsPage.ConnectionSpells.InsertOrReplaceAsync(thisIncantation);


            }
            else if (IncantationEntry.Text != "None")
            {
                thisIncantation.Offensive = Offensive.IsToggled;
                thisIncantation.Loud = Loud.IsToggled;
                thisIncantation.ForeignLanguage = FL.IsToggled;
                thisIncantation.IncantationText = IncantationEntry.Text;
                thisIncantation.Value = IncantationComplexity.SelectedIndex;
                thisSpell.MyIncantation = thisIncantation.ID;
                await ItemsPage.ConnectionSpells.InsertOrReplaceAsync(thisIncantation);


            }
            SetValueLabel();
        }

        protected override async void OnAppearing()
        {
            await ItemsPage.ConnectionSpells.CreateTableAsync<Incantation>();
            incantations = await ItemsPage.ConnectionSpells.Table<Incantation>().ToListAsync();

            base.OnAppearing();
        }

        private void AddIncantation_Clicked(object sender, EventArgs e)
        {
            deleteIncantaion.IsVisible = true;
            AddIncantation.IsVisible = false;
            lblIncantation.IsVisible = true;
            IncantationEntry.IsVisible = true;
            IncantationComplexity.IsVisible = true;
            lblOffensive.IsVisible = true; 
            Offensive.IsVisible = true;
            lblFL.IsVisible = true;
            FL.IsVisible = true;
            lblLoud.IsVisible = true;
            Loud.IsVisible = true;
        }

        private void deleteIncantaion_Clicked(object sender, EventArgs e)
        {
            deleteIncantaion.IsVisible = false;
            AddIncantation.IsVisible = true;
            lblIncantation.IsVisible = false;
            IncantationEntry.IsVisible = false;
            IncantationComplexity.IsVisible = false;
            lblOffensive.IsVisible = false;
            Offensive.IsVisible = false;
            lblFL.IsVisible = false;
            FL.IsVisible = false;
            lblLoud.IsVisible = false;
            Loud.IsVisible = false;
        }
    }
}