using System.Collections.Generic;
using System.ComponentModel;

namespace ComicBookRepository
{
    public sealed class ComicBookTitleDTO
    {
        public ComicBookTitleDTO() => ComicBookDetails = new HashSet<ComicBookDetailsDTO>();
        [DisplayName("ID")]
        public long Id { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("First Issue #")]
        public long FirstIssue { get; set; }
        [DisplayName("Last Issue #")]
        public long LastIssue { get; set; }
        [DisplayName("# of Issues")]
        public long NumIssues { get; set; }
        [DisplayName("# of Special Issues")]
        public long NumSpIssues { get; set; }
        [DisplayName("Limited Series?")]
        public bool LimitedSeries { get; set; }
        [DisplayName("Sortable Title (if any)")]
        public string SortableTitle { get; set; }

        public ICollection<ComicBookDetailsDTO> ComicBookDetails { get; set; }
    }
}
