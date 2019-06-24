using System.Threading.Tasks;

namespace InfoScreen.Admin.Logic
{
    public interface IAdminRepository
    {
        Task<DAL.Entity.Admin> GetAdmin(int id);
        
        Task<DAL.Entity.Admin> FindByUsername(string username);

        Task<bool> CreateAdmin(DAL.Entity.Admin admin);

        Task<bool> UpdateAdmin(DAL.Entity.Admin admin);
    }
}