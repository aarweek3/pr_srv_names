using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class PlatformConstants
    {
        public const int NameMaxLength = 100;
        public const int CodeMaxLength = 50;
        public const int FamilyMaxLength = 50;
        public const int DescriptionMaxLength = 500;

        public const int MinSortOrder = 0;
        public const int MaxSortOrder = 9999;

        public static class Codes
        {
            public const string Windows = "windows";
            public const string MacOS = "macos";
            public const string Linux = "linux";
            public const string Android = "android";
            public const string IOS = "ios";
            public const string Web = "web";
        }

        public static class Families
        {
            public const string Desktop = "desktop";
            public const string Mobile = "mobile";
            public const string Web = "web";
        }
    }

}
