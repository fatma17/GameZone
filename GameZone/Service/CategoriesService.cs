

namespace GameZone.Service
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _DbContext;

        public CategoriesService(ApplicationDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _DbContext.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .OrderBy(c => c.Text)
                .AsNoTracking()
                .ToList();
        }
    }
}
