
namespace GameZone.Service
{
    public class DevicesService : IDevicesService
    {
        private readonly ApplicationDbContext _DbContext;

        public DevicesService(ApplicationDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _DbContext.Devices
                 .Select(d => new SelectListItem
                 {
                     Value = d.Id.ToString(),
                     Text = d.Name
                 })
                 .OrderBy(d => d.Text)
                 .AsNoTracking()
                 .ToList();      
        }
    }
}
