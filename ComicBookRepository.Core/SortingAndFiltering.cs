namespace ComicBookRepository.Core
{
    public class SortingAndFiltering
    {
        public string CurrentSort { get; set; }
        public string NameSortParm { get; set; }
        public string CurrentFilter { get; set; }
        public int? CurrentPage { get; set; }
    }
}
