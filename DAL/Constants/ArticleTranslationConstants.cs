using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class ArticleTranslationConstants
    {
        public const int TitleMaxLength = 250;
        public const int ContentMaxLength = 50000;
        public const int ShortSummaryMaxLength = 500;
        public const int CoverImageUrlMaxLength = 2048;

        public const int MinContentLength = 100;
        public const int RecommendedContentLength = 1000;
    }

}
