using System;
using Contracts.Menu;

namespace Menu
{
    public class MenuItem : BaseMenuItem<int, Func<string>>
    {

        public MenuItem(int itemIndex, string itemLabel, Func<string> methodToExecute) 
            : base(itemIndex, itemLabel, methodToExecute)
        {
        }
        
        public override string ToString()
        {
            return ItemIndex + ") " + Label;
        }
    }
}
