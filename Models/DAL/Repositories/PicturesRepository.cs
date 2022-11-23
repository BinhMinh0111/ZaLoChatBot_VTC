using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Repositories.Interfaces;

namespace ZaloOA_v2.Repositories
{
    public class PicturesRepository : IPicturesRepository
    {
        public bool Add(string json, string path)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int PictureId)
        {
            throw new NotImplementedException();
        }

        public List<Picture> GetAllPictures()
        {
            throw new NotImplementedException();
        }

        public List<Picture> GetPagePictures(int offset, int range)
        {
            throw new NotImplementedException();
        }

        public Picture GetPictures(int PictureId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Picture pictureChanges)
        {
            throw new NotImplementedException();
        }
    }
}
