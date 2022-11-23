using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Repositories.Interfaces
{
    public interface IPicturesRepository
    {
        Picture GetPictures(int PictureId);
        List<Picture> GetAllPictures();
        List<Picture> GetPagePictures(int offset, int range);
        bool Add(string json, string path);
        bool Update(Picture pictureChanges);
        bool Delete(int PictureId);
    }
}
