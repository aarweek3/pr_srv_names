using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class ArticleTagConstants
    {
        public const short MinSortOrder = 0;
        public const short MaxSortOrder = 9999;

        public const int MaxTagsPerArticle = 15;
        public const int MaxPrimaryTagsPerArticle = 3;
    }

}
