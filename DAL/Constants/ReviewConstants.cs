using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class ReviewConstants
    {
        public const int ContentMaxLength = 5000;

        public const short MinScore = 1;
        public const short MaxScore = 5;

        public const int MinContentLengthForFullReview = 10;
        public const int MaxReviewsPerUserPerSoftware = 1;

        public const int MaxHelpfulCount = int.MaxValue;
        public const int MaxUnhelpfulCount = int.MaxValue;
    }

}
