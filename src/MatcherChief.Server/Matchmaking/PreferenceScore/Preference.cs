using MatcherChief.Shared.Enums;

namespace MatcherChief.Server.Matchmaking.PreferenceScore
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

        public override bool Equals(object other)
        {
            var pref = (Preference) other;
            return Title == pref.Title && Mode == pref.Mode;
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