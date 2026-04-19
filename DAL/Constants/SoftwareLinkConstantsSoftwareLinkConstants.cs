using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class SoftwareLinkConstants
    {
        public const int UrlMaxLength = 2048;
        public const int LinkTypeMaxLength = 40;
        public const int TitleMaxLength = 200;
        public const int DescriptionMaxLength = 500;

        public const short MinSortOrder = 0;
        public const short MaxSortOrder = 9999;

        public const int MaxLinksPerSoftware = 15;

        public static class LinkTypes
        {
            public const string Website = "website";
            public const string Documentation = "documentation";
            public const string GitHub = "github";
            public const string GitLab = "gitlab";
            public const string Support = "support";
            public const string Changelog = "changelog";
            public const string Forum = "forum";
            public const string Wiki = "wiki";
        }
    }

}
