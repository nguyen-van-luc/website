namespace Doanweb.Utilities
{
    public class Function
    {
        public static int _AccountId = 0;
        public static string _UserName = string.Empty;
        public static string _Email = string.Empty;
        public static string _Message = string.Empty;
        public static string _MessageEmail = string.Empty;
        public static int? _RoleId { get; set; }
        public static string TitleSlugGenerrationAlias(string title)
        {

            return SlugGenerator.SlugGenerator.GenerateSlug(title);
        }
        public static bool IsLogin()
        {
            if (string.IsNullOrEmpty(Function._UserName) || Function._AccountId <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
