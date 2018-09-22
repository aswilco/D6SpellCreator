using System;

using D6SpellCreator.Models;

namespace D6SpellCreator.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Spell Item { get; set; }
        public ItemDetailViewModel(Spell item = null)
        {
            Title = item?.Name;
            Item = item;
        }
    }
}
