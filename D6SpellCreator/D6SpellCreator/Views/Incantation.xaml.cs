using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D6SpellCreator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Incantations : ContentPage
    {
        Models.Spell thisSpell;

        string[] ComponentComplexity = { "Word or Phrase", "Sentence or Lengthy Phrase", "Complex Incantation (difficulty 11)",
            "Litany (difficulty 15)", "Complex Formula (difficulty 19)", "Extensive Complex Elements (difficulty 23)", "Unique" };

        public Incantations(Models.Spell spell)
        {
            string[] options = ComponentComplexity;
            thisSpell = spell;
            InitializeComponent();
            IncantationComplexity.ItemsSource = ComponentComplexity;
            SetValueLabel();
        }

        void Preview(object sender, EventArgs e)
        {
            var navPage = new NavigationPage(new ItemsPage());

            if (IncantationEntry.Text != "None")
            {
                thisSpell.MyIncantation = new Models.Spell.Incantation
                { incantation = IncantationEntry.Text, complexity = IncantationComplexity.SelectedIndex, offensive = Offensive.IsToggled, foreignLanguage = FL.IsToggled, loud = Loud.IsToggled };
                thisSpell.MyIncantation.value = (thisSpell.MyIncantation.complexity + 1) + (thisSpell.MyIncantation.offensive ? 1 : 0) + (thisSpell.MyIncantation.foreignLanguage ? 1 : 0) + (thisSpell.MyIncantation.loud ? 1 : 0);
            }
            MessagingCenter.Send(this, "AddItem", thisSpell);
            Application.Current.MainPage = navPage;
        }

        private void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + thisSpell.GetDifficultyAsync();
        }


        private void IncantationChanged(object sender, EventArgs e)
        {
            if (thisSpell.MyIncantation == null) thisSpell.MyIncantation = new Models.Spell.Incantation
            { incantation = IncantationEntry.Text, complexity = IncantationComplexity.SelectedIndex, offensive = Offensive.IsToggled, foreignLanguage = FL.IsToggled, loud = Loud.IsToggled };
            else
            {
                thisSpell.MyIncantation.offensive = Offensive.IsToggled;
                thisSpell.MyIncantation.loud = Loud.IsToggled;
                thisSpell.MyIncantation.foreignLanguage = FL.IsToggled;
                thisSpell.MyIncantation.incantation = IncantationEntry.Text;
                thisSpell.MyIncantation.complexity = IncantationComplexity.SelectedIndex;
            }
            SetValueLabel();
        }
    }
}