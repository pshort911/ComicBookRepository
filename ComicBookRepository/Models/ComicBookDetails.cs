using System;
using System.Collections.Generic;

namespace ComicBookRepository
{
    public partial class ComicBookDetails
    {
        public long Id { get; set; }
        public string OwnerId { get; set; }
        public string SpecialIssue { get; set; }
        public long IssueNum { get; set; }
        public string IssueName { get; set; }
        public string Grade { get; set; }
        public string Rating { get; set; }
        public string Description { get; set; }
        public string Own { get; set; }
        public string Want { get; set; }
        public long? TitleId { get; set; }

        public virtual ComicBookTitle Title { get; set; }
    }
}
