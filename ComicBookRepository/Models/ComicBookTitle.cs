using System;
using System.Collections.Generic;

namespace ComicBookRepository
{
    public partial class ComicBookTitle
    {
        public ComicBookTitle()
        {
            ComicBookDetails = new HashSet<ComicBookDetails>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public long FirstIssue { get; set; }
        public long LastIssue { get; set; }
        public long NumIssues { get; set; }
        public long NumSpIssues { get; set; }
        public string LimitedSeries { get; set; }

        public virtual ICollection<ComicBookDetails> ComicBookDetails { get; set; }
    }
}
