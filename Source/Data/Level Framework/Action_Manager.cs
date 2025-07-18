using System.Collections.Generic;

namespace Mastery.Core.Data.Level_Framework.Extensions
{
    public static class Action_Manager // This is meant to be used well patching to insure we don't have overlapping ActionEvents causing excess Exp.
    {
        public static Dictionary<string, List<string>> Actions = new Dictionary<string, List<string>>();

        public static bool AddAction(string actionType, string actionPoint)
        {
            if (Actions.ContainsKey(actionType) == false)
            {
                Actions.Add(actionType, new List<string>() { actionPoint });

                return false; //It Doesn't Already Contain This ActionPoint.
            }
            else if (Actions[actionType].Contains(actionPoint) == false)
            {
                Actions[actionType].Add(actionPoint);

                return false; //It Doesn't Already Contain This ActionPoint.
            }

            return true; //It Does Already Contain This ActionPoint.
        }
    }
}