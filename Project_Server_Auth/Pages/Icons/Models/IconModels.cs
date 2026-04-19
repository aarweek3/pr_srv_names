namespace pr_srv_names.Pages.Icons.Models
{
    public class IconMetadata
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? SvgContent { get; set; } // Optional: included when batch loading
    }

    public class IconCategoryPage
    {
        public string Category { get; set; } = string.Empty;
        public List<IconMetadata> Icons { get; set; } = new();
    }

    public class UpdateIconRequest
    {
        public string IconType { get; set; } = string.Empty;
        public string SvgContent { get; set; } = string.Empty;
        public bool ToBackend { get; set; } = true;
        public bool ToFrontend { get; set; } = true;
    }

    public class MoveIconRequest
    {
        public string IconType { get; set; } = string.Empty;
        public int TargetCategoryId { get; set; }
    }

    public class SaveIconToDiskRequest
    {
        public string FileName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string SvgContent { get; set; } = string.Empty;
    }
}
