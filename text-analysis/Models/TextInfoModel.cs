using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace text_analysis.Models
{
    public class TextInfo
    {
        public string TextId
        {
            get;
            set;
        }

        public int WordCount
        {
            get;
            set;
        }

        public int UniqueWords
        {
            get;
            set;
        }

        public int Complexity
        {
            get;
            set;
        }

        public string LinkToText {
            get;
            set;
        }
    }
}
