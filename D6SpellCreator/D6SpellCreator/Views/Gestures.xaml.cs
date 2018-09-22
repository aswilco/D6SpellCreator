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
    public partial class Gestures : ContentPage
    {
        Models.Spell thisSpell;

        string[] ComponentComplexity = { "Simple (ex. point finger)", "Fairly Simple (ex. make circles with fingers)", "Complex (difficulty 11)",
            "Very Complex (difficulty 15)", "Extremely Complex (difficulty 19)", "Challenging and Extremely Complex (difficulty 23)", "Unique" };

        public Gestures(Models.Spell spell)
        {
            string[] options = ComponentComplexity;
            thisSpell = spell;
            InitializeComponent();
            GestureComplexity.ItemsSource = ComponentComplexity;
            
        }

        async void ToIncantations(object sender, EventArgs e)
        {
            var navPage = new NavigationPage(new ItemsPage());

            if (Gesture.Text != "None")
            {
                thisSpell.MyGesture.value = thisSpell.MyGesture.complexity + (thisSpell.MyGesture.offensive ? 1 : 0);
            }

            await Navigation.PushModalAsync(new NavigationPage(new Incantations(thisSpell)));

        }

        private void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + thisSpell.GetDifficulty();
        }

        private void Offensive_Toggled(object sender, ToggledEventArgs e)
        {
            thisSpell.MyGesture.offensive = Offensive.IsToggled;
            thisSpell.MyGesture.value = thisSpell.MyGesture.complexity + (thisSpell.MyGesture.offensive ? 1 : 0);
            SetValueLabel();
        }

        private void GestureComplexity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (thisSpell.MyGesture == null) thisSpell.MyGesture = new Models.Spell.Gesture { gesture = Gesture.Text, complexity = GestureComplexity.SelectedIndex, offensive = Offensive.IsToggled };
            else {
                thisSpell.MyGesture.gesture = Gesture.Text;
                thisSpell.MyGesture.complexity = GestureComplexity.SelectedIndex;
                    }
            thisSpell.MyGesture.value = thisSpell.MyGesture.complexity + (thisSpell.MyGesture.offensive ? 1 : 0);
            SetValueLabel();
        }
    }
}