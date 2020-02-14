using System;

namespace MatcherChief.Core.Models
{
    public class Player
    {
        public Player(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Player(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }
}