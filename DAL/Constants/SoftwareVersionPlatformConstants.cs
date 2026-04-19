using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class SoftwareVersionPlatformConstants
    {
        public const int ArchitectureMaxLength = 20;
        public const int SupportStatusMaxLength = 50;

        public const int MaxFilesPerPlatform = 10;

        public static class Architectures
        {
            public const string X64 = "x64";
            public const string X86 = "x86";
            public const string Arm64 = "arm64";
            public const string Universal = "universal";
        }

        public static class SupportStatuses
        {
            public const string Supported = "supported";
            public const string Deprecated = "deprecated";
            public const string Planned = "planned";
            public const string Beta = "beta";
        }
    }

}
