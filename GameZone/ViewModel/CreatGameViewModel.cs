using GameZone.Attributes;

namespace GameZone.ViewModel
{
    public class CreatGameViewModel : GameViewModel
    {
        
        [AllowedExtensions(FileSettings.AllowedExtensions)
            ,MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;


    }
}
