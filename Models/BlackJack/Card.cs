using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Logic;

namespace WebApplication1.Models.BlackJack
{
    public class Card : IComparable
    {
        public bool isJacksOrBetter()
        {
            if (_rank == RANK.RA)
                return true;
            if (_rank >= RANK.RJ)
                return true;
            return false;
        }

        public int Value
        {
            get {
                return ((int)_rank > 10 ) ? 10 : (int)_rank; 
            }
        }

        private RANK _rank;
        private SUIT _suit;

        // IComparable interface method
        public int CompareTo(object o)
        {
            if (o is Card)
            {
                Card c = (Card)o;
                if (_rank < c.rank)
                    return -1;
                else if (_rank > c.rank)
                    return 1;
                return 0;
            }
            throw new ArgumentException("Object is not a Card");
        }

        public Card(RANK rank, SUIT suit)
        {
            this._rank = rank;
            this._suit = suit;
        }

        public override string ToString()
        {
            return "['" + this._rank.GetDescription() + "','" + this._suit.GetDescription() + "']";
        }

        public RANK rank
        {
            get { return _rank; }
        }

        public SUIT suit
        {
            get { return _suit; }
        }
    }

}