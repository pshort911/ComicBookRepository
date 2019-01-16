using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicBookRepository.Controllers
{
    public class ComicBookDetailsController : Controller
    {
        #region Private Fields

        private readonly ComicBookRepositoryContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private string _userId;
        private IQueryable<ComicBookDetails> _comicBookDetails;

        #endregion Private Fields

        #region Public Constructors

        public ComicBookDetailsController(ComicBookRepositoryContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #endregion Public Constructors

        #region Public Methods

        // GET: ComicBookDetails/Create
        public IActionResult Create() => View();

        // POST: ComicBookDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OwnerId,Title,SpecialIssue,IssueNum,IssueName,Grade,Rating,Description,Own,Want")]
            ComicBookDetails comicBookDetails)
        {
            if (!ModelState.IsValid) return View(comicBookDetails);
            comicBookDetails.OwnerId = _userId;
            _context.Add(comicBookDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { comicBookDetails.Title.Id });

        }

        // GET: ComicBookDetails/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var comicBookDetails = await _context.ComicBookDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comicBookDetails == null) return NotFound();

            return View(comicBookDetails);
        }

        // POST: ComicBookDetails/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var comicBookDetails = await _context.ComicBookDetails.FindAsync(id);
            _context.ComicBookDetails.Remove(comicBookDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { comicBookDetails.Title.Id });
        }

        // GET: ComicBookDetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var comicBookDetails = await _context.ComicBookDetails.FindAsync(id);
            if (comicBookDetails == null) return NotFound();
            return View(comicBookDetails);
        }

        // POST: ComicBookDetails/Edit/5 To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,OwnerId,Title,SpecialIssue,IssueNum,IssueName,Grade,Rating,Description,Own,Want")]
            ComicBookDetails comicBookDetails)
        {
            if (id != comicBookDetails.Id) return NotFound();

            if (!ModelState.IsValid) return View(comicBookDetails);

            try
            {
                comicBookDetails.OwnerId = _userId;
                _context.Update(comicBookDetails);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicBookDetailsExists(comicBookDetails.Id)) return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index), new { comicBookDetails.Title.Id });
        }

        //GET: ComicBookDetails/asdf
        public async Task<ActionResult> Index(long? id)
        {
            await UpdateUserId();

            if (_userId == null) return NotFound();

            var comicBookDetails = id == null ? 
                _context.ComicBookDetails.Where(m => m.OwnerId == _userId) : 
                _context.ComicBookDetails.Where(m => m.OwnerId == _userId && m.TitleId == id).Include(t => t.Title);
            return View(comicBookDetails);
        }

        //GET: ComicBookDetails/OwnList
        public async Task<ActionResult> OwnList(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            await UpdateUserId();

            if (_userId == null) return NotFound();
            _comicBookDetails = from s in _context.ComicBookDetails where s.OwnerId == _userId where s.Own select s;

            var sortingFiltering = GetSortingAndFilteringData(sortOrder, currentFilter, searchString, page);

            ViewData["CurrentSort"] = sortingFiltering.CurrentSort;
            ViewData["NameSortParm"] = sortingFiltering.NameSortParm;
            page = sortingFiltering.CurrentPage;
            ViewData["CurrentFilter"] = sortingFiltering.CurrentFilter;

            var pageSize = 100;
            return View(await PaginatedList<ComicBookDetails>.CreateAsync(_comicBookDetails.AsNoTracking(), page ?? 1, pageSize));
        }

        //GET: ComicBookDetails/WantList
        public async Task<ActionResult> WantList(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            await UpdateUserId();

            if (_userId == null) return NotFound();

            _comicBookDetails = from s in _context.ComicBookDetails where s.OwnerId == _userId where s.Want select s;

            var sortingFiltering = GetSortingAndFilteringData(sortOrder, currentFilter, searchString, page);

            ViewData["CurrentSort"] = sortingFiltering.CurrentSort;
            ViewData["NameSortParm"] = sortingFiltering.NameSortParm;
            page = sortingFiltering.CurrentPage;
            ViewData["CurrentFilter"] = sortingFiltering.CurrentFilter;
            
            var pageSize = 100;
            return View(await PaginatedList<ComicBookDetails>.CreateAsync(_comicBookDetails.AsNoTracking(), page ?? 1, pageSize));
        }

        #endregion Public Methods

        #region Private Methods

        private bool ComicBookDetailsExists(long id) => _context.ComicBookDetails.Any(e => e.Id == id);

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private async Task UpdateUserId()
        {
            if (_userId == null)
            {
                var user = await GetCurrentUserAsync();
                _userId = user?.Id;
            }
        }

        private SortingAndFiltering GetSortingAndFilteringData(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var sortingFiltering = new SortingAndFiltering
            {
                CurrentSort = sortOrder,
                NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "",
                CurrentFilter = searchString,
                CurrentPage = page
            };
            if (!string.IsNullOrEmpty(searchString))
            {
                _comicBookDetails = _comicBookDetails.Where(s => s.Title.Title.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    _comicBookDetails = _comicBookDetails.OrderByDescending(s => s.Title.SortableTitle ?? s.Title.Title);
                    break;
                default:
                    _comicBookDetails = _comicBookDetails.OrderBy(s => s.Title.SortableTitle ?? s.Title.Title);
                    break;
            }

            return sortingFiltering;
        }

        #endregion Private Methods
    }
}