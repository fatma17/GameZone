
namespace GameZone.Service
{
    public interface IGamesService
    {
        IEnumerable<Game> GetAllGames();
        Game? GetById(int id);
        Task Create(CreatGameViewModel CreatViewModel);
        Task<Game?> Update(EditGameViewModel EditViewModel);
        bool Delete (int id);
        
    }
}
