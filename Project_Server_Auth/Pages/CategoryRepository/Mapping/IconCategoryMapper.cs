using DAL.Models.GeneralModels;
using Project_Server_Auth.Pages.CategoryRepository.Dtos;

namespace Project_Server_Auth.Pages.CategoryRepository.Mapping
{
    public static class IconCategoryMapper
    {
        public static IconCategoryDto ToDto(this IconCategory entity)
        {
            if (entity == null) return null!;

            return new IconCategoryDto
            {
                Id = entity.Id,
                FolderName = entity.FolderName,
                DisplayName = entity.Name, // Name maps to DisplayName
                IsSystem = entity.IsSystem,
                MenuIcon = entity.MenuIcon,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public static IconCategory ToEntity(this IconCategoryCreateDto dto)
        {
            if (dto == null) return null!;

            return new IconCategory
            {
                Name = dto.DisplayName,
                FolderName = dto.FolderName,
                IsSystem = dto.IsSystem,
                MenuIcon = dto.MenuIcon ?? "av_folder"
            };
        }

        public static void UpdateEntity(this IconCategoryUpdateDto dto, IconCategory entity)
        {
            if (dto == null || entity == null) return;

            entity.Name = dto.DisplayName;
            entity.MenuIcon = dto.MenuIcon ?? entity.MenuIcon;
        }
    }
}
