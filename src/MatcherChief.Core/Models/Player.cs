using System;

namespace MatcherChief.Core.Models
{
    public class Player
    {
        public Player()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
    }
}