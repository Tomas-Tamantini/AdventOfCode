namespace AdventOfCode.Console.Models
{
    public record DamagedSprings(string ConditionRecords, int[] ContiguousGroups);
    public record HotSpringsState(int SpringPointer, int GroupPointer);
    public class HotSprings
    {
        private readonly DamagedSprings damagedSprings;
        private readonly Dictionary<HotSpringsState, long> memoizedStates;

        public HotSprings(DamagedSprings damagedSprings)
        {
            this.damagedSprings = damagedSprings;
            memoizedStates = new Dictionary<HotSpringsState, long>();
        }

        public long NumArrangements()
        {
            var state = new HotSpringsState(SpringPointer: 0, GroupPointer: 0);
            return NumArrangementsRecursive(state);
        }

        private char? CurrentSpring(HotSpringsState state)
        {
            return state.SpringPointer >= damagedSprings.ConditionRecords.Length ? null : damagedSprings.ConditionRecords[state.SpringPointer];
        }

        private int? CurrentGroup(HotSpringsState state)
        {
            return state.GroupPointer >= damagedSprings.ContiguousGroups.Length ? null : damagedSprings.ContiguousGroups[state.GroupPointer];
        }

        private static HotSpringsState IncrementSpringPointer(HotSpringsState state)
        {
            return new HotSpringsState(state.SpringPointer + 1, state.GroupPointer);
        }

        private HotSpringsState? StateAfterPlacingCurrentGroup(HotSpringsState state, int group)
        {
            if (state.SpringPointer + group > damagedSprings.ConditionRecords.Length) return null;
            for (int i = state.SpringPointer; i < state.SpringPointer + group; i++)
            {
                List<char> validCharacters = new() { '#', '?' };
                if (!validCharacters.Contains(damagedSprings.ConditionRecords[i])) return null;
            }

            if (state.SpringPointer + group < damagedSprings.ConditionRecords.Length && damagedSprings.ConditionRecords[state.SpringPointer + group] == '#') return null;

            return new HotSpringsState(SpringPointer: state.SpringPointer + group + 1, GroupPointer: state.GroupPointer + 1);

        }

        private bool ContainsDamagedSpring(HotSpringsState state)
        {
            return damagedSprings.ConditionRecords.Skip(state.SpringPointer).Any(chr => chr == '#');
        }

        private long NumArrangementsRecursive(HotSpringsState state)
        {
            if (memoizedStates.ContainsKey(state)) return memoizedStates[state];
            long returnValue = 0;

            char? currentSpring = CurrentSpring(state);
            int? currentGroup = CurrentGroup(state);
            if (currentGroup == null) returnValue = ContainsDamagedSpring(state) ? 0 : 1;
            else if (currentSpring == '.')
            {
                var newState = IncrementSpringPointer(state);
                returnValue = NumArrangementsRecursive(newState);
            }
            else if (currentSpring == '#')
            {
                HotSpringsState? stateWithPlacing = StateAfterPlacingCurrentGroup(state, currentGroup.Value);
                returnValue = stateWithPlacing == null ? 0 : NumArrangementsRecursive(stateWithPlacing);
            }
            else if (currentSpring == '?')
            {
                HotSpringsState stateWithoutPlacing = IncrementSpringPointer(state);
                long numArrangementsWithoutPlacing = NumArrangementsRecursive(stateWithoutPlacing);
                HotSpringsState? stateWithPlacing = StateAfterPlacingCurrentGroup(state, currentGroup.Value);
                long NumArrangementsWithPlacing = stateWithPlacing == null ? 0 : NumArrangementsRecursive(stateWithPlacing);
                returnValue = numArrangementsWithoutPlacing + NumArrangementsWithPlacing;

            }

            memoizedStates.Add(state, returnValue);
            return returnValue;
        }
    }
}