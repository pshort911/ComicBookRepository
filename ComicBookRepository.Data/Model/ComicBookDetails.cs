namespace ComicBookRepository.Data
{
    public sealed class ComicBookDetails
    {
        public long Id { get; set; }
        public string OwnerId { get; set; }
        public bool SpecialIssue { get; set; }
        public long IssueNum { get; set; }
        public string IssueName { get; set; }
        public string Grade { get; set; }
        public string Rating { get; set; }
        public string Description { get; set; }
        public bool Own { get; set; }
        public bool Want { get; set; }
        public long TitleId { get; set; }
        public ComicBookTitle Title { get; set; }
    }
}
