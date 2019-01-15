using System.ComponentModel;

namespace ComicBookRepository
{
    public sealed class ComicBookDetails
    {
        [DisplayName("ID")]
        public long Id { get; set; }
        [DisplayName("Owner ID")]
        public string OwnerId { get; set; }
        [DisplayName("Special Issue?")]
        public bool SpecialIssue { get; set; }
        [DisplayName("Issue Number")]
        public long IssueNum { get; set; }
        [DisplayName("Issue Name")]
        public string IssueName { get; set; }
        [DisplayName("Grade")]
        public string Grade { get; set; }
        [DisplayName("Rating")]
        public string Rating { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Own?")]
        public bool Own { get; set; }
        [DisplayName("Want?")]
        public bool Want { get; set; }
        [DisplayName("Title ID")]
        public long? TitleId { get; set; }
        public ComicBookTitle Title { get; set; }
    }
}
