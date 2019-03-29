using System.Collections.Generic;

namespace ComicBookRepository.Data {
    public sealed class ComicBookTitle
    {
        public ComicBookTitle() => ComicBookDetails = new HashSet<ComicBookDetails>();

        public long Id { get; set; }

        public string Title { get; set; }

        public long FirstIssue { get; set; }

        public long LastIssue { get; set; }

        public long NumIssues { get; set; }

        public long NumSpIssues { get; set; }

        public bool LimitedSeries { get; set; }

        public string SortableTitle { get; set; }

        public ICollection<ComicBookDetails> ComicBookDetails { get; set; }
    }
}
