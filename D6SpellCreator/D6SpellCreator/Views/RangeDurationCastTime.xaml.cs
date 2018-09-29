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
	public partial class RangeDurationCastTime : ContentPage
	{
        Models.Spell thisSpell;
		public RangeDurationCastTime (Models.Spell spell)
		{
            thisSpell = spell;
			InitializeComponent ();
            SetValueLabel();
		}

        async void ToOtherAspects(object sender, EventArgs e)
        {
            await ItemsPage.ConnectionSpells.UpdateAsync(thisSpell);
            await Navigation.PushModalAsync(new NavigationPage(new OtherAspects(thisSpell)));
        }

        private void rangePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRangeValue();
        }

        private void durationPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDurationValue() ;
        }

        private void castTimePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCastingTimeValue();
        }

        private async void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + await thisSpell.GetDifficultyAsync();
        }

        private void SetCastingTimeValue()
        {
            int index = castTimePicker.SelectedIndex;
            switch (index)
            {
                case 0:
                    thisSpell.CastingTimeValue = -1;
                    thisSpell.CastingTime = castTimePicker.SelectedItem.ToString();
                    break;
                case 1:
                    thisSpell.CastingTimeValue = 0;
                    thisSpell.CastingTime = castTimePicker.SelectedItem.ToString();
                    break;
                case 2:
                    thisSpell.CastingTimeValue = 4;
                    thisSpell.CastingTime = castTimePicker.SelectedItem.ToString();
                    break;
                case 3:
                    thisSpell.CastingTimeValue = 9;
                    thisSpell.CastingTime = castTimePicker.SelectedItem.ToString();
                    break;
                case 4:
                    thisSpell.CastingTimeValue = 18;
                    thisSpell.CastingTime = castTimePicker.SelectedItem.ToString();
                    break;
                default:
                    break;
            }
            SetValueLabel();
        }

        private void SetDurationValue()
        {
            int index = durationPicker.SelectedIndex;
            switch (index)
            {
                case 0:
                    thisSpell.DurationValue = 0;
                    thisSpell.Duration = durationPicker.SelectedItem.ToString();
                    break;
                case 1:
                    thisSpell.DurationValue = 4;
                    thisSpell.Duration = durationPicker.SelectedItem.ToString();
                    break;
                case 2:
                    thisSpell.DurationValue = 5;
                    thisSpell.Duration = durationPicker.SelectedItem.ToString();
                    break;
                case 3:
                    thisSpell.DurationValue = 9;
                    thisSpell.Duration = durationPicker.SelectedItem.ToString();
                    break;
                case 4:
                    thisSpell.DurationValue = 18;
                    thisSpell.Duration = durationPicker.SelectedItem.ToString();
                    break;
                default:
                    break;
            }
            SetValueLabel();
        }

        private void SetRangeValue()
        {
            int index = rangePicker.SelectedIndex;
            switch (index)
            {
                case 0:
                    thisSpell.RangeValue = 0;
                    thisSpell.Range = rangePicker.SelectedItem.ToString();
                    break;
                case 1:
                    thisSpell.RangeValue = 8;
                    thisSpell.Range = rangePicker.SelectedItem.ToString();
                    break;
                case 2:
                    thisSpell.RangeValue = 10;
                    thisSpell.Range = rangePicker.SelectedItem.ToString();
                    break;
                case 3:
                    thisSpell.RangeValue = 20;
                    thisSpell.Range = rangePicker.SelectedItem.ToString();
                    break;
                case 4:
                    thisSpell.RangeValue = 30;
                    thisSpell.Range = rangePicker.SelectedItem.ToString();
                    break;
                default:
                    break;
            }
            SetValueLabel();
        }
    }
}