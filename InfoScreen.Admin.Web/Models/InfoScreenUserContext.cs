namespace InfoScreen.Admin.Web.Models
{
    public class InfoScreenUserContext
    {
        public int AdminId { get; }

        public InfoScreenUserContext(int adminId)
        {
            AdminId = adminId;
        }
    }
}