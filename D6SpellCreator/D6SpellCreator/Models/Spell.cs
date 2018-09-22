using System;
using System.Collections.Generic;
using SQLite;

namespace D6SpellCreator.Models
{
    [Table("Spells")]
    public class Spell
    {

        public Spell()
        {
            components = new List<Component>();
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
        private List<Component> components;
        private Gesture gesture;
        private Incantation incantation;

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
        public List<Component> Components { get => components; set => components = value; }
        public int Feedback { get => feedback; set => feedback = value; }
        public int AreaEffect { get => areaEffect; set => areaEffect = value; }
        public int Speed { get => speed; set => speed = value; }
        [Ignore]
        public Gesture MyGesture { get => gesture; set => gesture = value; }
        [Ignore]
        public Incantation MyIncantation { get => incantation; set => incantation = value; }
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

        public class Component
        {
            public bool destroyedOnUse;
            public string component;
            public int complexity;
            public int value = 0;
        }

        public class Gesture
        {
            public string gesture;
            public int complexity;
            public bool offensive;
            public int value = 0;
        }

        public class Incantation
        {
            public string incantation;
            public int complexity = 0;
            public bool offensive = false;
            public bool foreignLanguage = false;
            public bool loud;
            public int value = 0;
        }

        public int GetDifficulty()
        {
            difficulty = (int)Math.Ceiling((double)((EffectValue - GetComponentsValue() + AreaEffectValue + rangeValue +
                DurationValue - CastingTimeValue - (MyGesture != null ? MyGesture.value : 0) - (MyIncantation != null ? MyIncantation.value : 0)) / 2));
            return difficulty;
        }

        public int GetComponentsValue()
        {
            int value = 0;
            foreach (Component comp in Components)
            {
                value += comp.value;
            }
            value = (int)(value * (Components.Count > 6 ? .75 : Components.Count > 3 ? .5 : 1));
            return value;
        }
    }
}
