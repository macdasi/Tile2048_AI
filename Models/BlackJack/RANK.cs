using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.BlackJack
{
    public enum RANK
    {
        [Description("A")]
        RA = 1,
        [Description("2")]
        R2,
        [Description("3")]
        R3,
        [Description("4")]
        R4,
        [Description("5")]
        R5,
        [Description("6")]
        R6,
        [Description("7")]
        R7,
        [Description("8")]
        R8,
        [Description("9")]
        R9,
        [Description("10")]
        RT,
        [Description("J")]
        RJ,
        [Description("Q")]
        RQ,
        [Description("K")]
        RK
    }
}