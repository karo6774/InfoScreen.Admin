using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace InfoScreen.Admin.Logic
{
    public class DatabaseAdminRepository : IAdminRepository
    {
        private const string GetAdminQuery = "SELECT * from Admins WHERE Id=@Id";
        private const string FindAdminQuery = "SELECT * from Admins WHERE Username=@Username";

        private const string CreateAdminQuery =
            "INSERT INTO Admins (Username, PasswordHash, PasswordSalt) VALUES (@Username, @PasswordHash, @PasswordSalt)";
        
        private const string UpdateAdminQuery =
            "UPDATE Admins SET PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt WHERE Username=@Username";

        private const string DeleteAdminQuery =
            "DELETE Admins WHERE Username=@Username";

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

        public async Task<bool> UpdateAdmin(DAL.Entity.Admin admin)
        {
            await Database.Query(UpdateAdminQuery, parameters: new Dictionary<string, object>
            {
                {"@Username", admin.Username},
                {"@PasswordSalt", admin.PasswordSalt},
                {"@PasswordHash", admin.PasswordHash}
            });
            return true;
        }

        public async Task<bool> DeleteAdmin(string username)
        {
            await Database.Query(DeleteAdminQuery, parameters: new Dictionary<string, object>
            {
                {"@Username", username}
            });
            return true;
        }

        private static DAL.Entity.Admin ParseAdmin(DataRow row)
        {
            var salt = new byte[Hash.SaltSize];
            Array.Copy((byte[]) row["PasswordSalt"], salt, Hash.SaltSize);

            var hash = new byte[Hash.HashSize];
            Array.Copy((byte[]) row["PasswordHash"], hash, Hash.HashSize);

            return new DAL.Entity.Admin(
                id: (int) row["Id"],
                username: (string) row["Username"],
                passwordSalt: salt,
                passwordHash: hash
            );
        }
    }
}