using System;
using System.Threading;

namespace Contracts.Menu
{
    public abstract class BaseMenuActions
    {
        protected static bool NotExit(string userChoice)
        {
            return !userChoice.Equals("Exit");
        }

        protected static bool NotReturn(string userChoice)
        {
            return !userChoice.Equals("Return");
        }
        
        protected static bool NotReturnToMain(string userChoice)
        {
            return !userChoice.Equals("Return to main");
        }

        protected static bool IsDefault(string userChoice)
        {
            return userChoice.Equals("");
        }

        protected static void Wait()
        {
            Thread.Sleep(2000);
        }
    }
}
