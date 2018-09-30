using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace D6SpellCreator.Models
{
    
    public class Audio 
    {
        Persistence.IAudio audio;

        public Audio()
        {
            audio = DependencyService.Get<Persistence.IAudio>();
        }
    }
}
