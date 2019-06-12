using InfoScreen.Admin.Logic;

namespace InfoScreen.Admin.DAL.Entity
{
    public class Admin
    {
        public int Id { get; }
        public string Username { get; set; }
        public byte[] PasswordSalt { get; private set; }
        public byte[] PasswordHash { get; private set; }

        public Admin()
        {
            Id = -1;
        }

        public Admin(int id, string username, byte[] passwordSalt, byte[] passwordHash)
        {
            Id = id;
            Username = username;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
        }

        public void SetPassword(string password)
        {
            var (hash, salt) = Hash.HashPassword(password);
            PasswordSalt = salt;
            PasswordHash = hash;
        }

        public bool MatchesPassword(string password)
        {
            return Hash.VerifyPassword(password, PasswordHash, PasswordSalt);
        }
    }
}