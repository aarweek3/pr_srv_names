using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class SoftwareStatisticsConstants
    {
        public const long MaxViewsCount = long.MaxValue;
        public const long MaxDownloadCount = long.MaxValue;

        public const double MinAverageRating = 0.0;
        public const double MaxAverageRating = 5.0;

        public const int MaxReviewCount = int.MaxValue;
        public const int MaxRatingOnlyCount = int.MaxValue;
        public const int MaxBookmarkCount = int.MaxValue;
        public const int MaxShareCount = int.MaxValue;
    }

}
