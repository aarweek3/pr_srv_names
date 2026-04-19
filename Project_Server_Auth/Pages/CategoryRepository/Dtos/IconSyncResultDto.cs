using System.Collections.Generic;

namespace Project_Server_Auth.Pages.CategoryRepository.Dtos
{
    public class IconSyncResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int CategoriesProcessed { get; set; }
        public int IconsProcessed { get; set; }
        public List<string> Logs { get; set; } = new List<string>();
    }
}
