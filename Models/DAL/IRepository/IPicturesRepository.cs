using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Repositories.Interfaces
{
    public interface IPicturesRepository
    {
        PictureDTO GetPictures(int PictureId);
        List<PictureDTO> GetAllPictures();
        List<PictureDTO> GetPagePictures(int offset, int range);
        bool Add(string json, string path);
        bool Update(PictureDTO pictureChanges);
        bool Delete(int PictureId);
    }
}
