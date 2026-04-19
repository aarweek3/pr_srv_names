using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class CategoryConstants
    {
        public const int SlugMaxLength = 150;
        public const int PathMaxLength = 500;

        public const short MaxLevel = 5;
        public const int MaxChildrenCount = 50;
        public const int MinSortOrder = 0;
        public const int MaxSortOrder = 9999;

        public const char PathSeparator = '/';
        public const int MinPathSegments = 1;
        public const int MaxPathSegments = 5;
    }

}
