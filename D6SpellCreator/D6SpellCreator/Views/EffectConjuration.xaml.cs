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
    public partial class EffectConjuration : ContentPage
    {
        string sliderLabel = "Dice Value";

        string[] ConjurationEffects = {
        "Temporarily Gain a New Skill",
        "Damage or Protection",
        "Conjure Something Simple",
        "Conjure Something Complex"};

        string[] Weights =
        {
            "1kg or Less",
            "1kg to 5kg",
            "5kg to 100kg",
            "100kg to 1000kg"
        };
        Spell thisSpell;

        public EffectConjuration()
        {
            InitializeComponent();
        }
        public EffectConjuration(Spell spell)
        {
            thisSpell = spell;
            InitializeComponent();
            Difficulty.ItemsSource = ConjurationEffects;
            Difficulty.SelectedIndex = 0;
            WeightPicker.ItemsSource = Weights;
            WeightPicker.SelectedIndex = 0;
            SetValueLabel();
        }

        async void ToRangeDurationCastTime(object sender, EventArgs e)
        {
            thisSpell.Description = SpellEffect.Text;
            thisSpell.EffectValue = GetConjurationEffectValue();
            await Navigation.PushModalAsync(new NavigationPage(new RangeDurationCastTime(thisSpell)));
        }
        int StepValue = 1;

        private void Dice_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);
            Dice.Value = newStep * StepValue;
            DiceLabel.Text = sliderLabel + Dice.Value.ToString();
            thisSpell.EffectValue = GetConjurationEffectValue();
            SetValueLabel();
        }

        private void Difficulty_SelectedIndexChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (Difficulty.SelectedIndex > 1)
            {
                sliderLabel = "Conjured Items Weight in kg ";
                Dice.IsVisible = false;
                WeightPicker.IsVisible = true;
                DiceLabel.Text = sliderLabel;
            }
            else
            {
                Dice.Maximum = 20;
                sliderLabel = "Dice Value ";
                Dice.IsVisible = true;
                WeightPicker.IsVisible = false;
                DiceLabel.Text = sliderLabel + Dice.Value;
            }

            thisSpell.EffectValue = GetConjurationEffectValue();
            SetValueLabel();
            Dice.Value = 1;
        }

        private int GetConjurationEffectValue()
        {
            int returnValue;
            if (Difficulty.SelectedIndex <= 1) returnValue = 3 * (int)Dice.Value;
            else if (Difficulty.SelectedIndex == 2)
            {
                int value = WeightPicker.SelectedIndex;
                if (value == 0) returnValue = 1;
                else if (value == 1) returnValue = 4;
                else if (value == 2) returnValue = 10;
                else returnValue = 15;
            }
            else
            {
                
                int value = WeightPicker.SelectedIndex;
                if (value == 0) returnValue = 5;
                else if (value == 1) returnValue = 8;
                else if (value == 2) returnValue = 14;
                else returnValue = 19;
            }
            return returnValue;
        }

        void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + thisSpell.GetDifficultyAsync();

        }
    }
}