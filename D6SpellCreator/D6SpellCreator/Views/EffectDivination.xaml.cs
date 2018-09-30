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
    public partial class EffectDivination : ContentPage
    {
        string sliderLabel = "Dice Value ";

        string[] DivinationEffects = {
        "See The Past or Future",
        "Search an Area With a Search of 4D"
        };

        string[] SearchValues =
        {
            "1 Meter",
            "Less than 10 Meters",
            "Less than 100 Meters",
            "Less than 1000 Meters"
        };

        string[] TimeStepValue =
        {
            "Minutes",
            "Hours",
            "Days"
        };

        string[] SearchValue =
        {
            "2D Circle",
            "3D Sphere"
        };
        Spell thisSpell;

        public EffectDivination()
        {
            InitializeComponent();
        }

        public EffectDivination(Spell spell)
        {
            thisSpell = spell;
            InitializeComponent();
            Difficulty.ItemsSource = DivinationEffects;
            Step.ItemsSource = TimeStepValue;
            SearchRangePicker.ItemsSource = SearchValues;
        }

        async void ToRangeDurationCastTime(object sender, EventArgs e)
        {
            thisSpell.Description = SpellEffect.Text;
            thisSpell.EffectValue = GetDivinationEffectValue();
            if (thisSpell.EffectValue == 0)
            {
                await DisplayAlert("Effect is Empty", "Please choose an effect", "OK");
                return;
            }
            await Navigation.PushModalAsync(new NavigationPage(new RangeDurationCastTime(thisSpell)));
        }
        int StepValue = 1;

        private void Dice_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);
            Dice.Value = newStep * StepValue;
            DiceLabel.Text = Dice.Value.ToString() + sliderLabel;
            thisSpell.EffectValue = GetDivinationEffectValue();
            SetValueLabel();
        }

        private void Difficulty_SelectedIndexChanged(object sender, SelectedItemChangedEventArgs e)
        {
            Step.IsEnabled = true;
            if (Difficulty.SelectedIndex == 0)
            {
                Dice.IsEnabled = false;
                Dice.IsVisible = false;
                SearchRangePicker.IsVisible = true;
                SearchRangePicker.IsEnabled = true;

                sliderLabel = Step.SelectedItem != null ? Step.SelectedItem.ToString() + " In the Past or Future " : "";
                Step.ItemsSource = TimeStepValue;
                if (Difficulty.SelectedIndex == 0) Step.SelectedIndex = 0;
                else Dice.Maximum = 1000;

            }
            else
            {
                Dice.IsEnabled = false;
                Dice.IsVisible = false;
                SearchRangePicker.IsVisible = true;
                SearchRangePicker.IsEnabled = true;

                Step.ItemsSource = SearchValue;
                sliderLabel = Step.SelectedItem != null ? Step.SelectedItem.ToString() + " Search Radius in Meters " : "";
            }
            thisSpell.Description = SpellEffect.Text;
            thisSpell.EffectValue = GetDivinationEffectValue();
            SetValueLabel();
        }

        private int GetDivinationEffectValue()
        {
            if (Difficulty.SelectedIndex == 0)
            {

                if (Step.SelectedIndex == 0) return 9 * (int)Dice.Value;
                else if (Difficulty.SelectedIndex == 1) return 18 * (int)Dice.Value;
                else return 25 * (int)Dice.Value;
            }
            else if (Difficulty.SelectedIndex == 1)
            {
                int value = (int)SearchRangePicker.SelectedIndex;
                if (Step.SelectedIndex == 0)
                {
                    if (value == 0) return 18;
                    else if (value == 1) return 27;
                    else if (value == 2) return 37;
                    else if (value == 3) return 47;
                }
                else if (Step.SelectedIndex == 1)
                {
                    if (value == 0) return 19;
                    else if (value == 1) return 32;
                    else if (value == 2) return 47;
                    else if (value == 3) return 62;
                }
                else return 0;
            }
            return 0;
        }

        private void TimeStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Difficulty.SelectedIndex == 0)
            {
                if (Step.SelectedIndex == 0)
                {
                    sliderLabel = " Minutes ";
                    Dice.Maximum = 60;
                    Dice.Value = 1;
                }
                else if (Step.SelectedIndex == 1)
                {
                    sliderLabel = " Hours ";
                    Dice.Maximum = 24;
                    Dice.Value = 1;
                }
                else if (Step.SelectedIndex == 2)
                {
                    sliderLabel = " Days ";
                    Dice.Maximum = 14;
                    Dice.Value = 1;
                }

            }
            else
            {
                sliderLabel = " Meters Search Radius";
            }
            thisSpell.Description = SpellEffect.Text;
            thisSpell.EffectValue = GetDivinationEffectValue();
            SetValueLabel();
        }

        private async void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + await thisSpell.GetDifficultyAsync();
        }
    }
}