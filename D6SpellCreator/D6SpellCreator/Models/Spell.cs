using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using D6SpellCreator.Persistence;
using D6SpellCreator.Views;
using SQLite;
using Xamarin.Forms;

namespace D6SpellCreator.Models
{
    [Table("Spells")]
    public class Spell
    {

        public Spell()
        {
            components = new List<int?>();
        }

        private string name;
        private int complexity = 0;
        private int difficulty = 0;

        private int id;

        public enum SkillType
        {
            Apportation,
            Alteration,
            Divination,
            Conjuration
        }

        private SkillType skill;

        private enum Target
        {
            self,
            friendly,
            enemy
        }

        private string duration;
        private string castingTime;
        private string range;
        public string Description { get =>description; set => description = value; }

        private string skillType;
        private string sideEffect;
        private int effectValue = 0;
        private int castingTimeValue = 0;
        private int rangeValue = 0;
        private int durationValue = 0;
        private int feedback;
        private int areaEffect;
        private int areaEffectValue = 0;
        private int speed;
        private string description;
        private List<int?> components;
        public string componentsString;
        private int? gesture;
        private int? incantation;

        [MaxLength(255)]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public SkillType Skill
        {
            get
            {
                return skill;
            }
            set
            {
                skill = value;
            }
        }

        public string Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }

        public int Complexity { get => complexity; set => complexity = value; }
        public int Difficulty { get => difficulty; set => difficulty = value; }
        public string CastingTime { get => castingTime; set => castingTime = value; }
        public string Range { get => range; set => range = value; }
        public string SideEffect { get => sideEffect; set => sideEffect = value; }
        public int RangeValue { get => rangeValue; set => rangeValue = value; }

        [Ignore]
        public List<int?> Components
        {
            get
            {
                return components;
            }
            set
            {
                components = value;
                componentsString = "";
                foreach (int? i in components)
                {
                    if (componentsString.Length == 0) componentsString += i;
                    else componentsString += i + " ";
                }
            }
        }

        public string ComponentsString
        {
            get
            {

                return componentsString;
            }
            set
            {
                componentsString = value;
            }
        }

        public int Feedback { get => feedback; set => feedback = value; }
        public int AreaEffect { get => areaEffect; set => areaEffect = value; }
        public int Speed { get => speed; set => speed = value; }
        public int? MyGestureID { get => gesture; set => gesture = value; }
        public int? MyIncantation { get => incantation; set => incantation = value; }
        public int EffectValue { get => effectValue; set => effectValue = value; }
        public int CastingTimeValue { get => castingTimeValue; set => castingTimeValue = value; }
        public int DurationValue { get => durationValue; set => durationValue = value; }
        public int AreaEffectValue { get => areaEffectValue; set => areaEffectValue = value; }
        public string SkillType1 { get => skillType; set => skillType = value; }

        public void SetSkill(int index)
        {
            switch (index)
            {
                case 0:
                    skill = SkillType.Apportation;
                    break;
                case 1:
                    skill = SkillType.Alteration;
                    break;
                case 2:
                    skill = SkillType.Divination;
                    break;
                case 3:
                    skill = SkillType.Conjuration;
                    break;
                default:
                    break;

            }
        }

        [Table("Components")]
        public class Component
        {
            private bool destroyedOnUse;
            private string componentName;
            private int complexity;
            private int value = 0;
            private int id;
            [PrimaryKey, NotNull]
            public int ID
            {
                get
                {
                    return id;
                }
                set
                {
                    id = value;
                }
            }

            public bool DestroyedOnUse { get => destroyedOnUse; set => destroyedOnUse = value; }
            public string ComponentName { get => componentName; set => componentName = value; }
            public int Complexity { get => complexity; set => complexity = value; }
            public int Value { get => value; set => this.value = value; }
        }
        [Table("Gestures")]
        public class Gesture
        {

            private int id;
            [PrimaryKey, NotNull]
            public int ID { get => id; set => id = value; }
            public string GestureAction { get => gesture; set => gesture = value; }
            public int Complexity { get => complexity; set => complexity = value; }
            public bool Offensive { get => offensive; set => offensive = value; }
            public int Value { get => value; set => this.value = value; }

            private string gesture;
            private int complexity;
            private bool offensive;
            private int value = 0;
        }

        [Table("Incantations")]
        public class Incantation
        {
            private int id;
            [PrimaryKey, NotNull]
            public int ID { get => id; set => id = value; }
            public string IncantationText { get => incantationText; set => incantationText = value; }
            public bool Offensive { get => offensive; set => offensive = value; }
            public bool ForeignLanguage { get => foreignLanguage; set => foreignLanguage = value; }
            public bool Loud { get => loud; set => loud = value; }
            public int Value { get => value; set => this.value = value; }

            private string incantationText;
            private bool offensive = false;
            private bool foreignLanguage = false;
            private bool loud;
            private int value = 0;
        }

        public async Task<int> GetDifficultyAsync()
        {
            difficulty = (int)Math.Ceiling((double)((EffectValue + rangeValue  + DurationValue + AreaEffectValue - CastingTimeValue -
                await GetComponentsValue()  - (MyGestureID != null ? await GetGestureValue() : 0) - (MyIncantation != null ? await GetIncantationValue() : 0)) / 2));
            return difficulty;
        }

        public async Task<int> GetComponentsValue()
        {
            List<Component> componentsDB = await ItemsPage.ConnectionSpells.Table<Component>().ToListAsync();

            int value = 0;
            foreach (int comp in Components)
            {
                var component = componentsDB.Find(c => comp == c.ID);
                if (component != null) value += component.Value * (component.DestroyedOnUse ? 2 : 1);
            }
            value = (int)(value * (Components.Count > 6 ? .75 : Components.Count > 3 ? .5 : 1));
            return value;
        }

        public async Task<int> GetGestureValue()
        {
            List<Gesture> gestureList = await ItemsPage.ConnectionSpells.Table<Gesture>().ToListAsync();
            Gesture thisgesture = gestureList.Find(g => g.ID == gesture);
            if (thisgesture != null) return thisgesture.Value + (thisgesture.Offensive ? 1 : 0);
            else return 0;

        }

        public async Task<int> GetIncantationValue()
        {

            List<Incantation> incantationList = await ItemsPage.ConnectionSpells.Table<Incantation>().ToListAsync();
            Incantation thisIncantation = incantationList.Find(g => g.ID == incantation);

            if (thisIncantation != null) return thisIncantation.Value + (thisIncantation.Offensive ? 1 : 0) +
                    (thisIncantation.ForeignLanguage ? 1 :0) + (thisIncantation.Loud ? 1 : 0);
            else return 0;
        }



    }
}
