﻿using D6SpellCreator.Models;
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
    public partial class EffectAlteration : ContentPage
    {
        string[] AlterationEffects = { "Easy (Add Dice to a skill)", "Dificult (Add Dice to an Attribute)" };
        Spell thisSpell;
        public EffectAlteration(Spell spell)
        {
            thisSpell = spell;
            InitializeComponent();
            Difficulty.ItemsSource = AlterationEffects;
        }

        async void ToRangeDurationCastTime(object sender, EventArgs e)
        {
            thisSpell.Description = SpellEffect.Text;
            thisSpell.EffectValue = (int)(Dice.Value * (Difficulty.SelectedIndex == 0 ? 4.5 : Difficulty.SelectedIndex == 1 ? 18 : 0));
            if (thisSpell.EffectValue == 0)
            {
                await DisplayAlert("Effect is Empty", "Please choose an effect", "OK");
                return;
            }
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("dice.mp3");
            player.Play();

            await Navigation.PushModalAsync(new NavigationPage(new RangeDurationCastTime(thisSpell)));
        }
        int StepValue = 1;

        private void Dice_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);
            Dice.Value = newStep * StepValue;
            thisSpell.EffectValue = (int)(Dice.Value * (Difficulty.SelectedIndex == 0 ? 4.5 : Difficulty.SelectedIndex == 1 ? 18 : 0));
            DiceLabel.Text = "Dice Value " + Dice.Value.ToString();
            SetValueLabel();
        }
        private async void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + await thisSpell.GetDifficultyAsync();
        }

        private void Difficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            thisSpell.EffectValue = (int)(Dice.Value * (Difficulty.SelectedIndex == 0 ? 4.5 : Difficulty.SelectedIndex == 1 ? 18 : 0));
            SetValueLabel();
        }
    }


}
