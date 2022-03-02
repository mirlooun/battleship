using System.Collections.Generic;
using DAL.DAL.Entities.Enums.Rules;

namespace DAL.Utils
{
    public static class GameRuleProvider
    {
        private static readonly Dictionary<EBoatsCanTouch, string> TouchRules =
            new()
            {
                { EBoatsCanTouch.BoatsCanTouch, "Boats can touch" },
                { EBoatsCanTouch.BoatsCantTouch, "Boats can't touch" }
                
            };
        
        private static readonly Dictionary<EHitContinuousMove, string> HitRules =
            new()
            {
                { EHitContinuousMove.HitContinuousMove, "Hit continuous a move" },
                { EHitContinuousMove.HitDoesntContinueMove, "Hit doesn't continue a move" }
            };
        
        private static readonly Dictionary<ESavingMethod, string> SavingRules =
            new()
            {
                { ESavingMethod.SaveIsNewSave, "Saving creates a new save" },
                { ESavingMethod.SaveIsUpdate, "Saving updates current version" }
            };

        public static string GetUiName(EBoatsCanTouch type)
        {
            return TouchRules[type];
        }
        
        public static string GetUiName(EHitContinuousMove type)
        {
            return HitRules[type];
        }

        public static string GetUiName(ESavingMethod type)
        {
            return SavingRules[type];
        }
    }
}