using RimWorld;
using Verse;

using Mastery.Core.Utility;
using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Settings.Level_Framework;

namespace Mastery.Core.Data.Level_Framework.StatParts
{
    public class Mastery_StatPart : StatPart
    {
        public string LevelKey;

        public string field;

        public Pawn pawn;

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (pawn != null)
            {
                val = pawn.GetComp<Level_Comp_Manager>().CalculateField(LevelKey, req.Thing.def.defName, field, val);
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (pawn != null)
            {
                var config = Level_Settings_Manager.Instances[LevelKey].IGetConfig(req.Thing.def.defName);

                var curve = ClassUtility.GetField<UtilityCurve>(config, $"{field}Curve");

                var type = ClassUtility.GetField<OperationType>(config, $"{field}Type");

                var symbol = type == OperationType.Additive ? "+" : type == OperationType.Subtractive ? "-" :
                    type == OperationType.Multiplicative ? "*" : type == OperationType.Divisive ? "/" : "";


                var evaluation = curve.Evaluate(pawn.GetComp<Level_Comp_Manager>().Comps[LevelKey].GetOrAdd(req.Thing.def.defName).level);

                var value = curve.Percentage ? req.Thing.GetStatValue(parentStat) * evaluation : evaluation; //I think this will be inaccurate but idk.

                if (value != 0)
                {
                    return $"{$"{LevelKey}_{field}".Translate()}: {symbol}{value}";
                }
            }

            return "";
        }
    }
}