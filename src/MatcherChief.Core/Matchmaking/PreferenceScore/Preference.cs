using MatcherChief.Core.Models;

namespace MatcherChief.Core.Matchmaking.PreferenceScore
{
    public class Preference
    {
        public Preference(GameTitle title, GameMode mode)
        {
            Title = title;
            Mode = mode;
        }

        public GameTitle Title { get; private set; }
        public GameMode Mode { get; private set; }

        public bool Equals(Preference other)
        {
            return Title == other.Title && Mode == other.Mode;
        }

        public override int GetHashCode()
        {
            var hashCode = 181846194;
            hashCode = hashCode * -1521134295 + Title.GetHashCode();
            hashCode = hashCode * -1521134295 + Mode.GetHashCode();
            return hashCode;
        }
    }
}