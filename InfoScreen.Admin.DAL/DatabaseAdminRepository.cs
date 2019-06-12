using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace InfoScreen.Admin.Logic
{
    public class DatabaseAdminRepository : IAdminRepository
    {
        private const string GetAdminQuery = "SELECT * from Admins WHERE Id=@Id";
        private const string FindAdminQuery = "SELECT TOP 1 * from Admins WHERE Username=@Username";

        private const string CreateAdminQuery =
            "INSERT INTO Admins (Username, PasswordHash, PasswordSalt) VALUES (@Username, @PasswordHash, @PasswordSalt)";

        public async Task<DAL.Entity.Admin> GetAdmin(int id)
        {
            var data = await Database.Query(GetAdminQuery, parameters: new Dictionary<string, object>
            {
                {"@Id", id}
            });
            var table = data.Tables["Table"];
            if (table.Rows.Count <= 0)
                return null;
            var row = table.Rows[0];
            return ParseAdmin(row);
        }

        public async Task<DAL.Entity.Admin> FindByUsername(string username)
        {
            var data = await Database.Query(FindAdminQuery, parameters: new Dictionary<string, object>
            {
                {"@Username", username}
            });
            var table = data.Tables["Table"];
            if (table.Rows.Count <= 0)
                return null;
            var row = table.Rows[0];
            return ParseAdmin(row);
        }

        public async Task<bool> CreateAdmin(DAL.Entity.Admin admin)
        {
            await Database.Query(CreateAdminQuery, parameters: new Dictionary<string, object>
            {
                {"@Username", admin.Username},
                {"@PasswordSalt", admin.PasswordSalt},
                {"@PasswordHash", admin.PasswordHash}
            });
            return true;
        }

        private static DAL.Entity.Admin ParseAdmin(DataRow row)
        {
            return new DAL.Entity.Admin(
                id: (int) row["Id"],
                username: (string) row["Username"],
                passwordSalt: (byte[]) row["PasswordSalt"],
                passwordHash: (byte[]) row["PasswordHash"]
            );
        }
    }
}