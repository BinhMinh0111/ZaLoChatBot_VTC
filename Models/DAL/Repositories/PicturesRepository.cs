using ZaloOA_v2.Models.DAL.IRepository;
using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Models.DAL.Repositories
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

        public List<PictureDTO> GetAllPictures()
        {
            throw new NotImplementedException();
        }

        public List<PictureDTO> GetPagePictures(int offset, int range)
        {
            throw new NotImplementedException();
        }

        public PictureDTO GetPictures(int PictureId)
        {
            throw new NotImplementedException();
        }

        public bool Update(PictureDTO pictureChanges)
        {
            throw new NotImplementedException();
        }
    }
}
