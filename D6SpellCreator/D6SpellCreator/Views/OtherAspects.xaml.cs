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
        List<Models.Spell.Component> thisComponentsList = new List<Models.Spell.Component>();
        List<Models.Spell.Component> componentsList;
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
            SetValueLabel();
        }

        async void ToGestures(object sender, EventArgs e)
        {
            SetSpellDifficulty(null, null);

            await Navigation.PushModalAsync(new NavigationPage(new Gestures(thisSpell)));

        }

        int numComponent;
        void AddComponent(object sender, EventArgs e)
        {
            Entry entry = new Entry { Text = "None", FontSize= 10 };
            entry.Unfocused += (object me, FocusEventArgs ea) => { SetSpellDifficulty(sender, e); };
            Picker picker = new Picker { ItemsSource = ComponentComplexity,  SelectedIndex = 0, FontSize = 10};
            picker.SelectedIndex = 0;
            picker.SelectedIndexChanged += new EventHandler(Complexity_SelectedIndexChanged);
            Label label = new Label { Text = "Destroy on Use", FontSize = 10};
            Xamarin.Forms.Switch destroy = new Xamarin.Forms.Switch();
            Button destroyButton = new Button { Text = "Destroy Component " + numComponent++ };
            destroyButton.Clicked += new EventHandler(DestroyButton_Clicked);
            Components.Children.Add(entry);
            Components.Children.Add(picker);
            Components.Children.Add(label);
            Components.Children.Add(destroy);
            Components.Children.Add(destroyButton);
            SetSpellDifficulty(sender, null);

        }

        private async void DestroyButton_Clicked(object sender, EventArgs e)
        {
            int number;
            Int32.TryParse(((Button)sender).Text.Replace("Destroy Component ", ""), out number);
            thisComponentsList.RemoveAt(number);
            foreach (Models.Spell.Component comp in thisComponentsList)
            {
                List<int?> tempList = new List<int?>();
                tempList.Add(comp.ID);
                thisSpell.Components = tempList;
                await ItemsPage.ConnectionSpells.InsertOrReplaceAsync(comp);

            }
            Components.Children.RemoveAt((number * 5));
            Components.Children.RemoveAt((number * 5));
            Components.Children.RemoveAt((number * 5));
            Components.Children.RemoveAt((number * 5));
            Components.Children.RemoveAt((number * 5));
            SetSpellDifficulty(sender, e);

        }

        private void Complexity_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSpellDifficulty(null, null);
            SetValueLabel();

        }

        private async void SetSpellDifficulty(object sender, EventArgs e)
        {
            thisComponentsList.Clear();
            var entries = Components.Children.Where(c => c is Entry);
            var pickers = Components.Children.ToList().Where(c => c is Picker).ToList();
            var switches = Components.Children.Where(c => c is Xamarin.Forms.Switch).ToList();
            foreach (Entry entry in entries)
            {
                if (entry.Text != "" && entry.Text != "None")
                {
                    int i = 0;
                    thisComponentsList.Add(new Models.Spell.Component { ID = await ItemsPage.ConnectionSpells.Table<Models.Spell.Component>().CountAsync(), ComponentName = entry.Text });
                    thisComponentsList[i].Complexity = ((Picker)pickers[i]).SelectedIndex;
                    thisComponentsList[i].DestroyedOnUse = ((Xamarin.Forms.Switch)switches[i]).IsToggled;
                    thisComponentsList[i].Value = thisComponentsList[i].Complexity * (thisComponentsList[i].DestroyedOnUse ? 2 : 1);
                    i++;

                }

            }

            foreach (Models.Spell.Component comp in thisComponentsList)
            {
                List<int?> tempList = new List<int?>();
                tempList.Add(comp.ID);
                thisSpell.Components = tempList;
                await ItemsPage.ConnectionSpells.InsertOrReplaceAsync(comp);

            }
            SetValueLabel();

        }

        protected override async void OnAppearing()
        {
            await ItemsPage.ConnectionSpells.CreateTableAsync<Models.Spell.Component>();
            componentsList = await ItemsPage.ConnectionSpells.Table<Models.Spell.Component>().ToListAsync();

            base.OnAppearing();
        }


        private async void SetValueLabel()
        {
            SpellValue.Text = "Spell Difficulty: " + await thisSpell.GetDifficultyAsync();
        }

    }
}