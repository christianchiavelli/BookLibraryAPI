namespace BookLibraryAPI.Parameters
{
    public class SearchBooksParameters
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Author { get; set; } = "";
        public string Genre { get; set; } = "";
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public bool? IsBorrowed { get; set; } = null;
        public decimal? MinPrice { get; set; } = null;
        public decimal? MaxPrice { get; set; } = null;
        public double? MinRating { get; set; } = null;
        public double? MaxRating { get; set; } = null;
        public string SortBy { get; set; } = "Title";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
