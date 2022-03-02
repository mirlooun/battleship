using System;
using System.Threading;

namespace GameSetupUiProviders.Utils
{
    public class WarningUi
    {
        public static void ShowWarningMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Wait();
            Console.Clear();
        }
        
        public static void ShowValidatorResponse(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Wait();
            Console.Clear();
        }
        
        private static void Wait(){
            Thread.Sleep(2000);
        } 
    }
}
