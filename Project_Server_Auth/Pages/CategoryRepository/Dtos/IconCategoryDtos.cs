namespace Project_Server_Auth.Pages.CategoryRepository.Dtos
{
    public class IconCategoryDto
    {
        public int Id { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsSystem { get; set; }
        public string? MenuIcon { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int IconCount { get; set; }
    }

    public class IconCategoryCreateDto
    {
        public string FolderName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? MenuIcon { get; set; }
        public bool IsSystem { get; set; }
    }

    public class IconCategoryUpdateDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? MenuIcon { get; set; }
    }
}
