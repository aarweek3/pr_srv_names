using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class ArticleConstants
    {
        public const int SlugMaxLength = 150;
        public const int ArticleTypeMaxLength = 50;

        public const int MaxViewsCount = int.MaxValue;

        public const int MaxTagsPerArticle = 15;
        public const int MaxRelatedSoftwarePerArticle = 10;
        public const int MaxCommentsPerArticle = 1000;

        public static class ArticleTypes
        {
            public const string Review = "review";
            public const string Guide = "guide";
            public const string News = "news";
            public const string Comparison = "comparison";
            public const string Tutorial = "tutorial";
            public const string HowTo = "howto";
        }
    }

}
