using System;

namespace Contracts.Menu
{
    public abstract class BaseMenu : BaseMenuActions
    {
        protected void DisplayProvider(Action contentLoop)
        {
            Console.Clear();
            contentLoop();
            Console.Clear();
        }
        
        protected static void ResetCursorPosition()
        {
            Console.SetCursorPosition(0, 0);
        }
    }
}
