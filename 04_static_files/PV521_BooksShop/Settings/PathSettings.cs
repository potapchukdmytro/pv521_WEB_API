namespace PV521_BooksShop.Settings
{
    public class PathSettings
    {
        public static string Storage => "Storage";
        public static string Images => Path.Combine(Storage, "Images");
        public static string Books => Path.Combine(Images, "books");
    }
}
