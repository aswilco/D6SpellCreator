using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using D6SpellCreator.Models;

namespace D6SpellCreator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Spell spell { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            spell = new Spell();
        }

        async void ToEffects(object sender, EventArgs e)
        {
            spell.Name = SpellName.Text;
            if (picker.SelectedItem == null)
            {
                await DisplayAlert("No Skill Selected", "Please select a skill for your spell", "OK");
                return;
            }
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("dice.mp3");
            player.Play();
            switch (spell.Skill)
            {
                case Spell.SkillType.Apportation:
                    await Navigation.PushModalAsync(new NavigationPage(new EffectApportation(spell)));
                    break;
                case Spell.SkillType.Alteration:
                    await Navigation.PushModalAsync(new NavigationPage(new EffectAlteration(spell)));
                    break;
                case Spell.SkillType.Divination:
                    await Navigation.PushModalAsync(new NavigationPage(new EffectDivination(spell)));
                    break;
                case Spell.SkillType.Conjuration:
                    await Navigation.PushModalAsync(new NavigationPage(new EffectConjuration(spell)));
                    break;
                default:
                    break;

            }
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = (int)picker.SelectedIndex;
            spell.SetSkill(selectedIndex);
            spell.SkillType1 = picker.SelectedItem.ToString();
        }
    }
}