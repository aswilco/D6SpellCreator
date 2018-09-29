using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static D6SpellCreator.Models.Spell;

namespace D6SpellCreator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Gestures : ContentPage
    {
        private Models.Spell thisSpell;
        private List<Gesture> gestures;
        private Gesture thisGesture;
        private string[] ComponentComplexity = { "Simple (ex. point finger)", "Fairly Simple (ex. make circles with fingers)", "Complex (difficulty 11)",
            "Very Complex (difficulty 15)", "Extremely Complex (difficulty 19)", "Challenging and Extremely Complex (difficulty 23)", "Unique" };

        public Gestures(Models.Spell spell)
        {
            string[] options = ComponentComplexity;
            thisSpell = spell;
            InitializeComponent();

        }

        private async void ToIncantations(object sender, EventArgs e)
        {
            var navPage = new NavigationPage(new ItemsPage());
            await Navigation.PushModalAsync(new NavigationPage(new Incantations(thisSpell)));

        }

        private async void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + await thisSpell.GetDifficultyAsync();
        }

        private void Offensive_Toggled(object sender, ToggledEventArgs e)
        {
            thisGesture.Offensive = e.Value;
            thisGesture.Value = thisGesture.Complexity + (thisGesture.Offensive ? 1 : 0);
            SetValueLabel();
        }

        private async void GestureChanged(object sender, EventArgs e)
        {

            if (thisSpell.MyGestureID == null && gestureEntry.Text != "None")
            {
                thisGesture = new Gesture { ID = await ItemsPage.ConnectionSpells.Table<Gesture>().CountAsync(), GestureAction = gestureEntry.Text, Complexity = complexityPicker.SelectedIndex, Offensive = offensiveSwitch.IsToggled };
                thisSpell.MyGestureID = thisGesture.ID;
                int x = await ItemsPage.ConnectionSpells.InsertOrReplaceAsync(thisGesture);
                thisGesture.Value = thisGesture.Complexity + (thisGesture.Offensive ? 1 : 0);

            }
            else if (thisGesture != null)
            {
                thisGesture.GestureAction = gestureEntry.Text;
                thisGesture.Complexity = complexityPicker.SelectedIndex;
                thisSpell.MyGestureID = thisGesture.ID;
                int x = await ItemsPage.ConnectionSpells.InsertOrReplaceAsync(thisGesture);
                thisGesture.Value = thisGesture.Complexity + (thisGesture.Offensive ? 1 : 0);

            }
            SetValueLabel();
        }

        protected override async void OnAppearing()
        {
            await ItemsPage.ConnectionSpells.CreateTableAsync<Gesture>();
            gestures = await ItemsPage.ConnectionSpells.Table<Gesture>().ToListAsync();

            base.OnAppearing();
        }

        private Entry gestureEntry;
        private Picker complexityPicker;
        private Switch offensiveSwitch;
        private Label label;
        private Label label2;
        private void AddGesture_Clicked(object sender, EventArgs e)
        {
            AddGesture.IsVisible = false;
            label = new Label { Text = "Gesture" };
            gestureEntry = new Entry { Text = "None", FontSize = 10 };
            gestureEntry.Unfocused += (object me, FocusEventArgs ea) => { GestureChanged(sender, e); };
            complexityPicker = new Picker { FontSize = 10, ItemsSource = ComponentComplexity };
            complexityPicker.SelectedIndexChanged += new EventHandler(GestureChanged);
            label2 = new Label { Text = "Offensive" };
            offensiveSwitch = new Switch();
            offensiveSwitch.Toggled += new EventHandler<ToggledEventArgs>(Offensive_Toggled);
            Button delete = new Button { Text = "Delete Gesture" };
            delete.Clicked += new EventHandler(Delete_clicked);
            GesturesStack.Children.Add(label);
            GesturesStack.Children.Add(gestureEntry);
            GesturesStack.Children.Add(complexityPicker);
            GesturesStack.Children.Add(label2);
            GesturesStack.Children.Add(offensiveSwitch);
            GesturesStack.Children.Add(delete);

        }

        private void Delete_clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsVisible = false;
            label.IsVisible = false;
            gestureEntry.IsVisible = false;
            complexityPicker.IsVisible = false;
            label2.IsVisible = false;
            offensiveSwitch.IsVisible = false;
            AddGesture.IsVisible = true;
            ItemsPage.ConnectionSpells.DeleteAsync<Gesture>(thisGesture.ID);
        }
    }
}