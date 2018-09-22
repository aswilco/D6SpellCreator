using D6SpellCreator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace D6SpellCreator.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EffectApportation : ContentPage
    {
        string[] ApportationEffects = { "Move you or something 1 meter or less",
            "Medium (Move you or something 10 meters or less)",
            "Dificult (Move you or something 1 kilometer or less)"
        };
        Spell thisSpell;
        public EffectApportation(Spell spell)
        {
            thisSpell = spell;
            InitializeComponent();
            Difficulty.ItemsSource = ApportationEffects;
        }

        async void ToRangeDurationCastTime(object sender, EventArgs e)
        {
            thisSpell.Description = SpellEffect.Text;
            thisSpell.EffectValue = (Difficulty.SelectedIndex == 0 ? 2 : Difficulty.SelectedIndex == 1 ? 10 : 30);
            await Navigation.PushModalAsync(new NavigationPage(new RangeDurationCastTime(thisSpell)));
        }

        private void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + thisSpell.GetDifficultyAsync();
        }

        private void Difficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            thisSpell.EffectValue = (Difficulty.SelectedIndex == 0 ? 2 : Difficulty.SelectedIndex == 1 ? 10 : 30);
            SetValueLabel();
        }
    }
}