using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using D6SpellCreator.Persistence;
using SQLite;
using Xamarin.Forms;

namespace D6SpellCreator.Models
{
    [Table("Spells")]
    public class Spell
    {

        public Spell()
        {
            components = new List<int>();
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
        private List<int> components;
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
        public List<int> Components { get => components; set => components = value; }
        public int Feedback { get => feedback; set => feedback = value; }
        public int AreaEffect { get => areaEffect; set => areaEffect = value; }
        public int Speed { get => speed; set => speed = value; }
        public int? MyGesture { get => gesture; set => gesture = value; }
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
            public int id;
            public bool destroyedOnUse;
            public string component;
            public int complexity;
            public int value = 0;
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
        }
        [Table("Gestures")]
        public class Gesture
        {

            private int id;
            [PrimaryKey, AutoIncrement]
            public int ID { get => id; set => id = value; }
            public string gesture;
            public int complexity;
            public bool offensive;
            public int value = 0;
        }

        [Table("Incantations")]
        public class Incantation
        {
            private int id;
            public int ID { get => id; set => id = value; }
            public string incantation;
            public int complexity = 0;
            public bool offensive = false;
            public bool foreignLanguage = false;
            public bool loud;
            public int value = 0;
        }

        public async Task<int> GetDifficultyAsync()
        {
            int compValue = await GetComponentsValue();
            difficulty = (int)Math.Ceiling((double)((EffectValue - await GetComponentsValue() + AreaEffectValue + rangeValue +
                DurationValue - CastingTimeValue - (MyGesture != null ? await GetGestureValue() : 0) - (MyIncantation != null ? await GetIncantationValue() : 0)) / 2));
            return difficulty;
        }

        public async Task<int> GetComponentsValue()
        {
            SQLiteAsyncConnection _connection = DependencyService.Get<ISQLLiteDB>().GetConnection();
            await _connection.CreateTableAsync<Component>();
            List<Component> componentList = await _connection.Table<Component>().ToListAsync();

            int value = 0;
            foreach (int comp in Components)
            {
                var component = componentList.Find(c => c.ID == comp);
                value += component.value;
            }
            value = (int)(value * (Components.Count > 6 ? .75 : Components.Count > 3 ? .5 : 1));
            return value;
        }

        public async Task<int> GetGestureValue()
        {
            SQLiteAsyncConnection _connection = DependencyService.Get<ISQLLiteDB>().GetConnection();
            await _connection.CreateTableAsync<Gesture>();
            List<Gesture> gestureList = await _connection.Table<Gesture>().ToListAsync();

            return gestureList.Find(g => g.ID == gesture).value;
        }

        public async Task<int> GetIncantationValue()
        {
            SQLiteAsyncConnection _connection = DependencyService.Get<ISQLLiteDB>().GetConnection();
            await _connection.CreateTableAsync<Incantation>();
            List<Incantation> incantationList = await _connection.Table<Incantation>().ToListAsync();

            return incantationList.Find(g => g.ID == incantation).value;
        }



    }
}
