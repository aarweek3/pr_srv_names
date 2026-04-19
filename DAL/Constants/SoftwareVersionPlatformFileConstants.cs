using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public static class SoftwareVersionPlatformFileConstants
    {
        public const int FileTypeMaxLength = 40;
        public const int FileFormatMaxLength = 20;
        public const int UrlMaxLength = 2048;
        public const int Sha256MaxLength = 64;

        public const long MaxFileSizeBytes = 10L * 1024 * 1024 * 1024; // 10GB

        public static class FileTypes
        {
            public const string Installer = "installer";
            public const string Portable = "portable";
            public const string Source = "source";
            public const string Update = "update";
        }

        public static class FileFormats
        {
            public const string Exe = "exe";
            public const string Msi = "msi";
            public const string Dmg = "dmg";
            public const string Pkg = "pkg";
            public const string Deb = "deb";
            public const string Rpm = "rpm";
            public const string AppImage = "appimage";
            public const string Zip = "zip";
            public const string Tar = "tar";
            public const string TarGz = "tar.gz";
        }
    }

}
