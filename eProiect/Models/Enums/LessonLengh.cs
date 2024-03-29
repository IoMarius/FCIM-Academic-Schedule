﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.Enums
{
    public class LessonLengh
    {

        private uint Length { get; set; }

        public LessonLengh()
        {
            //default length of a class. One table cell
            //Note: used as a parameter for "rowspan" in html tables.
            Length = 1;
        }

        public LessonLengh(uint _length)
        {
            if(Length>0 && Length<8)
                Length= _length;
            else
                Length = 1;
        }

        public uint GetLength() { return Length; }
        public void SetLength(uint _length)
        {
            if (Length > 0 && Length < 8)
                Length = _length;
            else
                Length = 1;
        }
    }
}