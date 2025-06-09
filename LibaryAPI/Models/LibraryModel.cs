namespace LibaryAPI.Models
{
    public class LibraryModel
    {
        public string Name { get; set; } = string.Empty;

        public string Autor { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public int id { get; set; }
        public int Pages { get; set; }
    }
}
