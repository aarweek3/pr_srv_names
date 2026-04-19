using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class ArticleCommentConstants
    {
        public const int ContentMaxLength = 4000;

        public const short MinRating = 0;
        public const short MaxRating = 5;

        public const int MaxCommentDepth = 5;
        public const int MaxRepliesPerComment = 100;

        public const int MinContentLength = 3;
    }

}
