using XStudio.SchoolSchedule.Enums;
using XStudio.SchoolSchedule.Rules;

namespace XStudio.SchoolSchedule.Constraints {

    [Serializable]
    public class Constraint : IRule, IConstraint {

        public Constraint(string id) : base(id) {
        }

        public Constraint(PriorityMode priority)
            : this(Guid.NewGuid().ToString()) {
            Priority = priority;
        }

        public Constraint(PriorityMode priority, RuleMode mode)
            : this(priority) {
            @Mode = mode;
        }

        public Constraint(PriorityMode priority, RuleMode mode, RuleType type)
            : this(priority, mode) {
            @Type = type;
        }

        public Constraint(PriorityMode priority, RuleMode mode, RuleType type, ActionRangeType rangeType, List<string> actionRange)
            : this(priority, mode, type) {
            RangeType = rangeType;
            ActionRange = actionRange;
        }
    }
}