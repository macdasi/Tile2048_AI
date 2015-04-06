using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.BlackJack
{
    /// <summary>
    /// [ 'bet-bt','stand-bt' , 'hit-bt', 'double-bt', 'split-bt' ];
    /// </summary>
    public enum Actions
    {
        BET = 0,
        /// <summary>
        /// draw in jacks or better
        /// </summary>
        STAND = 1,
        HIT = 2,
        DOUBLE = 3,
        SPLIT = 4,
        STOP = -1
    }
}