﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Shared.Helpers
{
    public static class ColorIndex
    {
        private static List<System.Drawing.Color> _colors = new List<System.Drawing.Color>
        {
            System.Drawing.Color.FromArgb(0,0,0),
            System.Drawing.Color.FromArgb(255,0,0),
            System.Drawing.Color.FromArgb(255,255,0),
            System.Drawing.Color.FromArgb(0,255,0),
            System.Drawing.Color.FromArgb(0,255,255),
            System.Drawing.Color.FromArgb(0,0,255),
            System.Drawing.Color.FromArgb(255,0,255),
            System.Drawing.Color.FromArgb(255,255,255),
            System.Drawing.Color.FromArgb(65,65,65),
            System.Drawing.Color.FromArgb(128,128,128),
            System.Drawing.Color.FromArgb(255,0,0),
            System.Drawing.Color.FromArgb(255,170,170),
            System.Drawing.Color.FromArgb(189,0,0),
            System.Drawing.Color.FromArgb(189,126,126),
            System.Drawing.Color.FromArgb(129,0,0),
            System.Drawing.Color.FromArgb(129,86,86),
            System.Drawing.Color.FromArgb(104,0,0),
            System.Drawing.Color.FromArgb(104,69,69),
            System.Drawing.Color.FromArgb(79,0,0),
            System.Drawing.Color.FromArgb(79,53,53),
            System.Drawing.Color.FromArgb(255,63,0),
            System.Drawing.Color.FromArgb(255,191,170),
            System.Drawing.Color.FromArgb(189,46,0),
            System.Drawing.Color.FromArgb(189,141,126),
            System.Drawing.Color.FromArgb(129,31,0),
            System.Drawing.Color.FromArgb(129,96,86),
            System.Drawing.Color.FromArgb(104,25,0),
            System.Drawing.Color.FromArgb(104,78,69),
            System.Drawing.Color.FromArgb(79,19,0),
            System.Drawing.Color.FromArgb(79,59,53),
            System.Drawing.Color.FromArgb(255,127,0),
            System.Drawing.Color.FromArgb(255,212,170),
            System.Drawing.Color.FromArgb(189,94,0),
            System.Drawing.Color.FromArgb(189,157,126),
            System.Drawing.Color.FromArgb(129,64,0),
            System.Drawing.Color.FromArgb(129,107,86),
            System.Drawing.Color.FromArgb(104,52,0),
            System.Drawing.Color.FromArgb(104,86,69),
            System.Drawing.Color.FromArgb(79,39,0),
            System.Drawing.Color.FromArgb(79,66,53),
            System.Drawing.Color.FromArgb(255,191,0),
            System.Drawing.Color.FromArgb(255,234,170),
            System.Drawing.Color.FromArgb(189,141,0),
            System.Drawing.Color.FromArgb(189,173,126),
            System.Drawing.Color.FromArgb(129,96,0),
            System.Drawing.Color.FromArgb(129,118,86),
            System.Drawing.Color.FromArgb(104,78,0),
            System.Drawing.Color.FromArgb(104,95,69),
            System.Drawing.Color.FromArgb(79,59,0),
            System.Drawing.Color.FromArgb(79,73,53),
            System.Drawing.Color.FromArgb(255,255,0),
            System.Drawing.Color.FromArgb(255,255,170),
            System.Drawing.Color.FromArgb(189,189,0),
            System.Drawing.Color.FromArgb(189,189,126),
            System.Drawing.Color.FromArgb(129,129,0),
            System.Drawing.Color.FromArgb(129,129,86),
            System.Drawing.Color.FromArgb(104,104,0),
            System.Drawing.Color.FromArgb(104,104,69),
            System.Drawing.Color.FromArgb(79,79,0),
            System.Drawing.Color.FromArgb(79,79,53),
            System.Drawing.Color.FromArgb(191,255,0),
            System.Drawing.Color.FromArgb(234,255,170),
            System.Drawing.Color.FromArgb(141,189,0),
            System.Drawing.Color.FromArgb(173,189,126),
            System.Drawing.Color.FromArgb(96,129,0),
            System.Drawing.Color.FromArgb(118,129,86),
            System.Drawing.Color.FromArgb(78,104,0),
            System.Drawing.Color.FromArgb(95,104,69),
            System.Drawing.Color.FromArgb(59,79,0),
            System.Drawing.Color.FromArgb(73,79,53),
            System.Drawing.Color.FromArgb(127,255,0),
            System.Drawing.Color.FromArgb(212,255,170),
            System.Drawing.Color.FromArgb(94,189,0),
            System.Drawing.Color.FromArgb(157,189,126),
            System.Drawing.Color.FromArgb(64,129,0),
            System.Drawing.Color.FromArgb(107,129,86),
            System.Drawing.Color.FromArgb(52,104,0),
            System.Drawing.Color.FromArgb(86,104,69),
            System.Drawing.Color.FromArgb(39,79,0),
            System.Drawing.Color.FromArgb(66,79,53),
            System.Drawing.Color.FromArgb(63,255,0),
            System.Drawing.Color.FromArgb(191,255,170),
            System.Drawing.Color.FromArgb(46,189,0),
            System.Drawing.Color.FromArgb(141,189,126),
            System.Drawing.Color.FromArgb(31,129,0),
            System.Drawing.Color.FromArgb(96,129,86),
            System.Drawing.Color.FromArgb(25,104,0),
            System.Drawing.Color.FromArgb(78,104,69),
            System.Drawing.Color.FromArgb(19,79,0),
            System.Drawing.Color.FromArgb(59,79,53),
            System.Drawing.Color.FromArgb(0,255,0),
            System.Drawing.Color.FromArgb(170,255,170),
            System.Drawing.Color.FromArgb(0,189,0),
            System.Drawing.Color.FromArgb(126,189,126),
            System.Drawing.Color.FromArgb(0,129,0),
            System.Drawing.Color.FromArgb(86,129,86),
            System.Drawing.Color.FromArgb(0,104,0),
            System.Drawing.Color.FromArgb(69,104,69),
            System.Drawing.Color.FromArgb(0,79,0),
            System.Drawing.Color.FromArgb(53,79,53),
            System.Drawing.Color.FromArgb(0,255,63),
            System.Drawing.Color.FromArgb(170,255,191),
            System.Drawing.Color.FromArgb(0,189,46),
            System.Drawing.Color.FromArgb(126,189,141),
            System.Drawing.Color.FromArgb(0,129,31),
            System.Drawing.Color.FromArgb(86,129,96),
            System.Drawing.Color.FromArgb(0,104,25),
            System.Drawing.Color.FromArgb(69,104,78),
            System.Drawing.Color.FromArgb(0,79,19),
            System.Drawing.Color.FromArgb(53,79,59),
            System.Drawing.Color.FromArgb(0,255,127),
            System.Drawing.Color.FromArgb(170,255,212),
            System.Drawing.Color.FromArgb(0,189,94),
            System.Drawing.Color.FromArgb(126,189,157),
            System.Drawing.Color.FromArgb(0,129,64),
            System.Drawing.Color.FromArgb(86,129,107),
            System.Drawing.Color.FromArgb(0,104,52),
            System.Drawing.Color.FromArgb(69,104,86),
            System.Drawing.Color.FromArgb(0,79,39),
            System.Drawing.Color.FromArgb(53,79,66),
            System.Drawing.Color.FromArgb(0,255,191),
            System.Drawing.Color.FromArgb(170,255,234),
            System.Drawing.Color.FromArgb(0,189,141),
            System.Drawing.Color.FromArgb(126,189,173),
            System.Drawing.Color.FromArgb(0,129,96),
            System.Drawing.Color.FromArgb(86,129,118),
            System.Drawing.Color.FromArgb(0,104,78),
            System.Drawing.Color.FromArgb(69,104,95),
            System.Drawing.Color.FromArgb(0,79,59),
            System.Drawing.Color.FromArgb(53,79,73),
            System.Drawing.Color.FromArgb(0,255,255),
            System.Drawing.Color.FromArgb(170,255,255),
            System.Drawing.Color.FromArgb(0,189,189),
            System.Drawing.Color.FromArgb(126,189,189),
            System.Drawing.Color.FromArgb(0,129,129),
            System.Drawing.Color.FromArgb(86,129,129),
            System.Drawing.Color.FromArgb(0,104,104),
            System.Drawing.Color.FromArgb(69,104,104),
            System.Drawing.Color.FromArgb(0,79,79),
            System.Drawing.Color.FromArgb(53,79,79),
            System.Drawing.Color.FromArgb(0,191,255),
            System.Drawing.Color.FromArgb(170,234,255),
            System.Drawing.Color.FromArgb(0,141,189),
            System.Drawing.Color.FromArgb(126,173,189),
            System.Drawing.Color.FromArgb(0,96,129),
            System.Drawing.Color.FromArgb(86,118,129),
            System.Drawing.Color.FromArgb(0,78,104),
            System.Drawing.Color.FromArgb(69,95,104),
            System.Drawing.Color.FromArgb(0,59,79),
            System.Drawing.Color.FromArgb(53,73,79),
            System.Drawing.Color.FromArgb(0,127,255),
            System.Drawing.Color.FromArgb(170,212,255),
            System.Drawing.Color.FromArgb(0,94,189),
            System.Drawing.Color.FromArgb(126,157,189),
            System.Drawing.Color.FromArgb(0,64,129),
            System.Drawing.Color.FromArgb(86,107,129),
            System.Drawing.Color.FromArgb(0,52,104),
            System.Drawing.Color.FromArgb(69,86,104),
            System.Drawing.Color.FromArgb(0,39,79),
            System.Drawing.Color.FromArgb(53,66,79),
            System.Drawing.Color.FromArgb(0,63,255),
            System.Drawing.Color.FromArgb(170,191,255),
            System.Drawing.Color.FromArgb(0,46,189),
            System.Drawing.Color.FromArgb(126,141,189),
            System.Drawing.Color.FromArgb(0,31,129),
            System.Drawing.Color.FromArgb(86,96,129),
            System.Drawing.Color.FromArgb(0,25,104),
            System.Drawing.Color.FromArgb(69,78,104),
            System.Drawing.Color.FromArgb(0,19,79),
            System.Drawing.Color.FromArgb(53,59,79),
            System.Drawing.Color.FromArgb(0,0,255),
            System.Drawing.Color.FromArgb(170,170,255),
            System.Drawing.Color.FromArgb(0,0,189),
            System.Drawing.Color.FromArgb(126,126,189),
            System.Drawing.Color.FromArgb(0,0,129),
            System.Drawing.Color.FromArgb(86,86,129),
            System.Drawing.Color.FromArgb(0,0,104),
            System.Drawing.Color.FromArgb(69,69,104),
            System.Drawing.Color.FromArgb(0,0,79),
            System.Drawing.Color.FromArgb(53,53,79),
            System.Drawing.Color.FromArgb(63,0,255),
            System.Drawing.Color.FromArgb(191,170,255),
            System.Drawing.Color.FromArgb(46,0,189),
            System.Drawing.Color.FromArgb(141,126,189),
            System.Drawing.Color.FromArgb(31,0,129),
            System.Drawing.Color.FromArgb(96,86,129),
            System.Drawing.Color.FromArgb(25,0,104),
            System.Drawing.Color.FromArgb(78,69,104),
            System.Drawing.Color.FromArgb(19,0,79),
            System.Drawing.Color.FromArgb(59,53,79),
            System.Drawing.Color.FromArgb(127,0,255),
            System.Drawing.Color.FromArgb(212,170,255),
            System.Drawing.Color.FromArgb(94,0,189),
            System.Drawing.Color.FromArgb(157,126,189),
            System.Drawing.Color.FromArgb(64,0,129),
            System.Drawing.Color.FromArgb(107,86,129),
            System.Drawing.Color.FromArgb(52,0,104),
            System.Drawing.Color.FromArgb(86,69,104),
            System.Drawing.Color.FromArgb(39,0,79),
            System.Drawing.Color.FromArgb(66,53,79),
            System.Drawing.Color.FromArgb(191,0,255),
            System.Drawing.Color.FromArgb(234,170,255),
            System.Drawing.Color.FromArgb(141,0,189),
            System.Drawing.Color.FromArgb(173,126,189),
            System.Drawing.Color.FromArgb(96,0,129),
            System.Drawing.Color.FromArgb(118,86,129),
            System.Drawing.Color.FromArgb(78,0,104),
            System.Drawing.Color.FromArgb(95,69,104),
            System.Drawing.Color.FromArgb(59,0,79),
            System.Drawing.Color.FromArgb(73,53,79),
            System.Drawing.Color.FromArgb(255,0,255),
            System.Drawing.Color.FromArgb(255,170,255),
            System.Drawing.Color.FromArgb(189,0,189),
            System.Drawing.Color.FromArgb(189,126,189),
            System.Drawing.Color.FromArgb(129,0,129),
            System.Drawing.Color.FromArgb(129,86,129),
            System.Drawing.Color.FromArgb(104,0,104),
            System.Drawing.Color.FromArgb(104,69,104),
            System.Drawing.Color.FromArgb(79,0,79),
            System.Drawing.Color.FromArgb(79,53,79),
            System.Drawing.Color.FromArgb(255,0,191),
            System.Drawing.Color.FromArgb(255,170,234),
            System.Drawing.Color.FromArgb(189,0,141),
            System.Drawing.Color.FromArgb(189,126,173),
            System.Drawing.Color.FromArgb(129,0,96),
            System.Drawing.Color.FromArgb(129,86,118),
            System.Drawing.Color.FromArgb(104,0,78),
            System.Drawing.Color.FromArgb(104,69,95),
            System.Drawing.Color.FromArgb(79,0,59),
            System.Drawing.Color.FromArgb(79,53,73),
            System.Drawing.Color.FromArgb(255,0,127),
            System.Drawing.Color.FromArgb(255,170,212),
            System.Drawing.Color.FromArgb(189,0,94),
            System.Drawing.Color.FromArgb(189,126,157),
            System.Drawing.Color.FromArgb(129,0,64),
            System.Drawing.Color.FromArgb(129,86,107),
            System.Drawing.Color.FromArgb(104,0,52),
            System.Drawing.Color.FromArgb(104,69,86),
            System.Drawing.Color.FromArgb(79,0,39),
            System.Drawing.Color.FromArgb(79,53,66),
            System.Drawing.Color.FromArgb(255,0,63),
            System.Drawing.Color.FromArgb(255,170,191),
            System.Drawing.Color.FromArgb(189,0,46),
            System.Drawing.Color.FromArgb(189,126,141),
            System.Drawing.Color.FromArgb(129,0,31),
            System.Drawing.Color.FromArgb(129,86,96),
            System.Drawing.Color.FromArgb(104,0,25),
            System.Drawing.Color.FromArgb(104,69,78),
            System.Drawing.Color.FromArgb(79,0,19),
            System.Drawing.Color.FromArgb(79,53,59),
            System.Drawing.Color.FromArgb(51,51,51),
            System.Drawing.Color.FromArgb(80,80,80),
            System.Drawing.Color.FromArgb(105,105,105),
            System.Drawing.Color.FromArgb(130,130,130),
            System.Drawing.Color.FromArgb(190,190,190),
            System.Drawing.Color.FromArgb(255,255,255)
        };
        public static System.Drawing.Color GetColorByIndex(int index)
        {
            if (index < 0)
                index = 0;
            if(index > 255)
                index = 255;
            return _colors[index];
        }

        public static String GetColorHex(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}