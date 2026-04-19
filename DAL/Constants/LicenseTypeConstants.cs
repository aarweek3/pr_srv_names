using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class LicenseTypeConstants
    {
        public const int NameMaxLength = 100;
        public const int CodeMaxLength = 50;
        public const int DescriptionMaxLength = 500;

        public const int MinSortOrder = 0;
        public const int MaxSortOrder = 9999;

        public static class Codes
        {
            public const string Free = "free";
            public const string Paid = "paid";
            public const string Freemium = "freemium";
            public const string Trial = "trial";
            public const string OpenSource = "opensource";
            public const string Subscription = "subscription";
        }
    }

}
