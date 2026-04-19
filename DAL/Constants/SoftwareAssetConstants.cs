using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class SoftwareAssetConstants
    {
        public const int AssetTypeMaxLength = 40;
        public const int UrlMaxLength = 2048;
        public const int TitleMaxLength = 200;
        public const int DescriptionMaxLength = 500;

        public const short MinSortOrder = 0;
        public const short MaxSortOrder = 9999;

        public const int MaxScreenshotsPerSoftware = 20;
        public const int MaxLogosPerSoftware = 5;
        public const int MaxBannersPerSoftware = 3;

        public static class AssetTypes
        {
            public const string Screenshot = "screenshot";
            public const string Logo = "logo";
            public const string Banner = "banner";
            public const string Icon = "icon";
            public const string Video = "video";
        }
    }

}
