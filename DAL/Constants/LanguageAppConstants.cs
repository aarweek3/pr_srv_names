using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class LanguageAppConstants
    {
        public const int CodeMaxLength = 10;
        public const int ShortCodeMaxLength = 5;
        public const int TitleMaxLength = 100;
        public const int NativeTitleMaxLength = 100;
        public const int DirectionMaxLength = 3;
        public const int IconKeyMaxLength = 20;

        public const int MinSortOrder = 0;
        public const int MaxSortOrder = 9999;

        public const int MaxLanguagesCount = 100;

        public static class Directions
        {
            public const string LeftToRight = "ltr";
            public const string RightToLeft = "rtl";
        }

        public static class CommonCodes
        {
            public const string Russian = "ru-RU";
            public const string English = "en-US";
            public const string German = "de-DE";
            public const string French = "fr-FR";
            public const string Spanish = "es-ES";
        }
    }
}


