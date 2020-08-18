using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RandomCoffeeMachine.Models
{
    public class Pair : IEquatable<Pair>
    {
        public int Round { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }
        public bool Equals(Pair other)
        {
            if(other == null)
            {
                return false;
            }               
            return (User1 == other.User1 && User2 == other.User2) || (User1 == other.User2 && User2 == other.User1);
        }

        public bool HasCrossing(Pair other)
        {
            return User1 == other.User1 || User1 == other.User2 || User2 == other.User1 || User2 == other.User2;
        }
    }
}
