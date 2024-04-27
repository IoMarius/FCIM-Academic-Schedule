using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProiect.Models.Enums
{
    public class LessonLength
    {
        private uint Length;

        public LessonLength()
        {
            //default length of a class. One table cell
            //Note: used as a parameter for "rowspan" in html tables.
            Length = 1;
        }

        public LessonLength(uint _length)
        {  
            Length = _length; 
        }

        public uint GetLength() { return Length; }
        public void SetLength(uint _length)
        {
            Length= _length;
        }
    }
}