


using Microsoft.EntityFrameworkCore;

namespace GameZone.Service
{
    public class GamesService :IGamesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public GamesService(ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagePath}";
        }

        public IEnumerable<Game> GetAllGames()
        {
            return _context.Games
                .Include(g=>g.Category)
                .Include(g=>g.Devices)
                .ThenInclude(d=>d.Device)
                .AsNoTracking()
                .ToList();
        }

        public Game? GetById(int id)
        {
            return _context.Games
                 .Include(g => g.Category)
                 .Include(g => g.Devices)
                 .ThenInclude(d => d.Device)
                 .AsNoTracking()
                 .SingleOrDefault(g=>g.Id==id);
        }

        public async Task Create(CreatGameViewModel CreatViewModel) 
        {
            var coverName = await SaveCover(CreatViewModel.Cover);

            Game game = new()
            {
                Name = CreatViewModel.Name,
                Description = CreatViewModel.Description,
                Cover = coverName,
                CategoryId = CreatViewModel.CategoryId,
                Devices = CreatViewModel.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };

            _context.Add(game);
            _context.SaveChanges();
        }
    
        public async Task<Game?> Update(EditGameViewModel EditViewModel)
        {
            //var game = _context.Games..Find(EditViewModel.Id);

            var game = _context.Games
                .Include(g => g.Devices)
                .SingleOrDefault(g => g.Id == EditViewModel.Id); 

            if (game == null)
                return null;

            var hasNewCover = EditViewModel.Cover != null;
            var oldcover = game.Cover;

            game.Name = EditViewModel.Name;
            game.Description = EditViewModel.Description;
            game.CategoryId = EditViewModel.CategoryId;
            game.Devices = EditViewModel.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();
        
            if(hasNewCover)
            {
                game.Cover = await SaveCover(EditViewModel.Cover!);
            } 
            var result = _context.SaveChanges();
            if (result > 0)
            {
                if (hasNewCover)
                {
                    //delete old image
                    var cover = Path.Combine(_imagesPath, oldcover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                //delete new image
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = _context.Games.Find(id);

            if(game == null)
                return isDeleted;

            _context.Remove(game);

            var effectRows = _context.SaveChanges();
            if(effectRows > 0)
            {
                isDeleted = true;

                var cover =Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }

        private async Task<String> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;
        }

        
    }

}
