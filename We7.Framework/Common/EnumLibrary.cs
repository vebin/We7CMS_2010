using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7
{
    [Serializable]
    public static class EnumLibrary
    {
        public static int[] Position = { 2, 0, 0, 0, 0, 0, 2, 0, 4, 2, 2, 0, 2, 4, 6, 8, 0, 2, 4, 0, 4, 6, 2, 0, 2 };

        public const int PlaceLength = 2;

        public enum Business : int
        { 
            ArticleType = 6
        }
    }
}
