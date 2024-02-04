
namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly IDevicesService _devicesService;
        private readonly ICategoriesService _CategoriesService;
        private readonly IGamesService _gamesService;

        public GamesController(IDevicesService devicesService
            , ICategoriesService CategoriesService , IGamesService gamesService)
        {
            _devicesService = devicesService;
            _CategoriesService = CategoriesService;
            _gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAllGames();
            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);

            if(game is null)
                return NotFound();

            return View(game);
        }

        [HttpGet]
        public IActionResult Creat()
        {
            CreatGameViewModel GameViewModel = new()
            {
                Categories = _CategoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList()
            };
            return View(GameViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creat(CreatGameViewModel GameViewModel)
        {
            if(!ModelState.IsValid)
            {
                GameViewModel.Categories = _CategoriesService.GetSelectList();
                GameViewModel.Devices = _devicesService.GetSelectList();
                return View(GameViewModel);
            }
            await _gamesService.Create(GameViewModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit (int id)
        {
            var game = _gamesService.GetById(id);

            if (game is null)
                return NotFound();

            EditGameViewModel EditGameViewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _CategoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList(),
                CurrentCover = game.Cover
            };
            return View(EditGameViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameViewModel EditViewModel)
        {
            if (!ModelState.IsValid)
            {
                EditViewModel.Categories = _CategoriesService.GetSelectList();
                EditViewModel.Devices = _devicesService.GetSelectList();
                return View(EditViewModel);
            }

            var game = await _gamesService.Update(EditViewModel);

            if (game is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gamesService.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }

    }
}
