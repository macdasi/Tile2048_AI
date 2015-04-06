using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.BlackJack;

namespace WebApplication1.Models.JacksOrBetter
{
    public class JoBmoves
    {
        public Actions move { get; set; }
        public int[] hold { get; set; }
    }
}