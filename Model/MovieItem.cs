using System.ComponentModel.DataAnnotations;

namespace aureliadotnetcore.Model
{
    public class MovieItem
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
