using DAL.Models.GeneralModels;

namespace DAL.Repositories.Interfaces
{
    public interface IIconRepository : IRepository<Icon>
    {
        /// <summary>
        /// Retrieves the SVG content for a given icon name.
        /// </summary>
        /// <param name="name">The unique name of the icon (e.g., "av_save").</param>
        /// <returns>The SVG content string, or null if not found.</returns>
        Task<string?> GetContentByNameAsync(string name);

        /// <summary>
        /// Checks if an icon with the given name exists.
        /// </summary>
        Task<bool> ExistsByNameAsync(string name);
    }
}
