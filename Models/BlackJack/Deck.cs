using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.BlackJack
{
    public class Deck
    {
        public static List<Card> NewDeck()
        {
            IEnumerable<SUIT> suits = Enum.GetValues(typeof(SUIT)).Cast<SUIT>();
            IEnumerable<RANK> ranks = Enum.GetValues(typeof(RANK)).Cast<RANK>();

            return (from s in suits from r in ranks select new Card(r, s)).ToList();
        }
    }
}