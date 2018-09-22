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
    public partial class OtherAspects : ContentPage
    {
        Models.Spell thisSpell;
        string[] ComponentComplexity = { "Ordinary", "Very Common", "Common", "Uncommon", "Very Rare", "Extremely Rare", "Unique" };

        OtherAspects()
        {
            InitializeComponent();
        }

        public OtherAspects(Models.Spell spell)
        {
            string[] options = ComponentComplexity;
            thisSpell = spell;
            InitializeComponent();
            Complexity.ItemsSource = ComponentComplexity;
            SetValueLabel();
        }

        async void ToGestures(object sender, EventArgs e)
        {
            thisSpell.Components.Clear();
            foreach (Entry entry in Components.Children.Where(c => c is Entry))
            {
                if (entry.Text != "" && entry.Text != "None") thisSpell.Components.Add(new Models.Spell.Component { component = entry.Text });
                
            }
            if (thisSpell.Components.Count() > 0)
            {
                int i = 0;
                foreach (Picker picker in Components.Children.Where(c => c is Picker))
                {
                    thisSpell.Components[i].complexity = picker.SelectedIndex;
                    i++;
                }
                i = 0;
                foreach (Xamarin.Forms.Switch toggle in Components.Children.Where(c => c is Xamarin.Forms.Switch))
                {
                    thisSpell.Components[i].destroyedOnUse = toggle.IsToggled;
                    thisSpell.Components[i].value = thisSpell.Components[i].complexity * (thisSpell.Components[i].destroyedOnUse ? 2 : 1);
                    i++;
                }
                
            }

            await Navigation.PushModalAsync(new NavigationPage(new Gestures(thisSpell)));

        }

        void AddComponent(object sender, EventArgs e)
        {
            Entry entry = new Entry { Text = "None", FontSize= 10 };
            Picker picker = new Picker { ItemsSource = ComponentComplexity,  SelectedIndex = 0, FontSize = 10};
            picker.SelectedIndexChanged += new EventHandler(Complexity_SelectedIndexChanged);
            Label label = new Label { Text = "Destroy on Use", FontSize = 10};
            Xamarin.Forms.Switch destroy = new Xamarin.Forms.Switch();
            SetSpellDifficulty(null, null);
            Components.Children.Add(entry);
            Components.Children.Add(picker);
            Components.Children.Add(label);
            Components.Children.Add(destroy);

        }

        private void Complexity_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSpellDifficulty(null, null);
            SetValueLabel();

        }

        private void SetSpellDifficulty(object sender, EventArgs e)
        {
            thisSpell.Components.Clear();
            foreach (Entry entry in Components.Children.Where(c => c is Entry))
            { 
                if (entry.Text != "" && entry.Text != "None") thisSpell.Components.Add(new Models.Spell.Component { component = entry.Text });
            }
            if (thisSpell.Components.Count() > 0)
            {
                int i = 0;
                foreach (Picker picker in Components.Children.Where(c => c is Picker))
                {
                    thisSpell.Components[i].complexity = picker.SelectedIndex;
                    i++;
                }
                i = 0;
                foreach (Xamarin.Forms.Switch toggle in Components.Children.Where(c => c is Xamarin.Forms.Switch))
                {
                    thisSpell.Components[i].destroyedOnUse = toggle.IsToggled;
                    thisSpell.Components[i].value = thisSpell.Components[i].complexity * (thisSpell.Components[i].destroyedOnUse ? 2 : 1);
                    i++;
                }

            }
            SetValueLabel();

        }

        private void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + thisSpell.GetDifficultyAsync();
        }

    }
}