using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class UpdateFeedConstants
    {
        public const int TitleMaxLength = 200;
        public const int ChangesMaxLength = 4000;

        public const int MinSortOrder = 0;
        public const int MaxSortOrder = 9999;

        public const int MaxUpdatesPerVersion = 50;
    }

}
