using Verse;

namespace Mastery.Core.Data.Level_Framework.Data.Extensions
{
    public class Level_Data_Extension : IExposable
    {
        public string label;

        public virtual void ExposeData()
        {

        }

        public virtual string Description()
        {
            return $"\n{label} ";
        }
    }
}