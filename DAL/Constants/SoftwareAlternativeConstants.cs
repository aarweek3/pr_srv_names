using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class SoftwareAlternativeConstants
    {
        public const int RelationTypeMaxLength = 32;
        public const int CommentMaxLength = 500;

        public const short MinSortOrder = 0;
        public const short MaxSortOrder = 9999;

        public const int MaxAlternativesPerSoftware = 20;

        public static class RelationTypes
        {
            public const string Alternative = "alternative";
            public const string Similar = "similar";
            public const string Replacement = "replacement";
            public const string Successor = "successor";
        }
    }

}
