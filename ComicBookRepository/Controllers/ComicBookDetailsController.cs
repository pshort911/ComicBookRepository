using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ComicBookRepository.Core;
using ComicBookRepository.Data;
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
        private IQueryable<ComicBookDetailsDTO> _comicBookDetails;
        private readonly IMapper _mapper;
        private string _userId;

        #endregion Private Fields

        #region Public Constructors

        public ComicBookDetailsController(ComicBookRepositoryContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        #endregion Public Constructors

        #region Public Methods

        // GET: ComicBookDetails/Create
        public IActionResult Create() => View();

        // POST: ComicBookDetails/Create To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OwnerId,BookTitle,SpecialIssue,IssueNum,IssueName,Grade,Rating,Description,Own,Want")]
            ComicBookDetailsDTO comicBookDetails)
        {
            if (!ModelState.IsValid) return View(comicBookDetails);
            await GetUser();
            comicBookDetails.OwnerId = _userId;
            _context.Add(comicBookDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {comicBookDetails.TitleId});
        }

        // GET: ComicBookDetails/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var comicBookDetails = await _context.ComicBookDetails.FirstOrDefaultAsync(m => m.Id == id);
            if (comicBookDetails == null) return NotFound();
            var comicBookDetailsDto = _mapper.Map<ComicBookDetailsDTO>(comicBookDetails);
            return View(comicBookDetailsDto);
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
            return RedirectToAction(nameof(Index), new {comicBookDetails.Title.Id});
        }

        // GET: ComicBookDetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var comicBookDetails = await _context.ComicBookDetails.FindAsync(id);
            if (comicBookDetails == null) return NotFound();
            var comicBookDetailsDto = _mapper.Map<ComicBookDetailsDTO>(comicBookDetails);
            return View(comicBookDetailsDto);
        }

        // POST: ComicBookDetails/Edit/5 To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,OwnerId,BookTitle,SpecialIssue,IssueNum,IssueName,Grade,Rating,Description,Own,Want,TitleId")]
            ComicBookDetailsDTO comicBookDetailsDto)
        {
            if (id != comicBookDetailsDto.Id) return NotFound();

            if (!ModelState.IsValid) return View(comicBookDetailsDto);

            var comicBookDetails = _mapper.Map<ComicBookDetails>(comicBookDetailsDto);
            await GetUser();
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

            return RedirectToAction(nameof(Index), new { comicBookDetails.TitleId});
        }

        //GET: ComicBookDetails/asdf
        public async Task<ActionResult> Index(long? id)
        {
            await GetUser();
            if (_userId == null) return NotFound();

            var comicBookDetails = id == null ? _context.ComicBookDetails.Where(m => m.OwnerId == _userId) : _context.ComicBookDetails.Where(m => m.OwnerId == _userId && m.TitleId == id).Include(t => t.Title);
            var comicBookDetailsDto = comicBookDetails.ProjectTo<ComicBookDetailsDTO>(_mapper.ConfigurationProvider);
            return View(comicBookDetailsDto);
        }

        //GET: ComicBookDetails/OwnList
        public async Task<ActionResult> OwnList(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            await GetUser();
            if (_userId == null) return NotFound();

            var comicBookDetails = from s in _context.ComicBookDetails where s.OwnerId == _userId where s.Own select s;

            var sortingFiltering = GetSortingAndFilteringData(comicBookDetails, sortOrder, currentFilter, searchString, page);

            ViewData["CurrentSort"] = sortingFiltering.CurrentSort;
            ViewData["NameSortParm"] = sortingFiltering.NameSortParm;
            page = sortingFiltering.CurrentPage;
            ViewData["CurrentFilter"] = sortingFiltering.CurrentFilter;

            var pageSize = 100;
            return View(await PaginatedList<ComicBookDetailsDTO>.CreateAsync(_comicBookDetails.AsNoTracking(), page ?? 1, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOwnList([FromBody] ComicBookDetailsDTO comicBookDetails) => UpdateDetails(false, comicBookDetails.Id, comicBookDetails.Own);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateWantList([FromBody] ComicBookDetailsDTO comicBookDetails) => UpdateDetails(true, comicBookDetails.Id, comicBookDetails.Want);

        //GET: ComicBookDetails/WantList
        public async Task<ActionResult> WantList(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            await GetUser();
            if (_userId == null) return NotFound();

             var comicBookDetails = from s in _context.ComicBookDetails where s.OwnerId == _userId where s.Want select s;

            var sortingFiltering = GetSortingAndFilteringData(comicBookDetails, sortOrder, currentFilter, searchString, page);

            ViewData["CurrentSort"] = sortingFiltering.CurrentSort;
            ViewData["NameSortParm"] = sortingFiltering.NameSortParm;
            page = sortingFiltering.CurrentPage;
            ViewData["CurrentFilter"] = sortingFiltering.CurrentFilter;

            var pageSize = 100;
            return View(await PaginatedList<ComicBookDetailsDTO>.CreateAsync(_comicBookDetails.AsNoTracking(), page ?? 1, pageSize));
        }

        #endregion Public Methods

        #region Private Methods

        private bool ComicBookDetailsExists(long id)
        {
            return _context.ComicBookDetails.Any(e => e.Id == id);
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private SortingAndFiltering GetSortingAndFilteringData(
            IQueryable<ComicBookDetails> comicBookDetails,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            var sortingFiltering = new SortingAndFiltering
            {
                CurrentSort = sortOrder,
                NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "",
                CurrentFilter = searchString,
                CurrentPage = page
            };
            if (!string.IsNullOrEmpty(searchString)) comicBookDetails = comicBookDetails.Where(s => s.Title.Title.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
            switch (sortOrder)
            {
                case "name_desc":
                    comicBookDetails = comicBookDetails.OrderByDescending(s => s.Title.SortableTitle ?? s.Title.Title);
                    break;

                default:
                    comicBookDetails = comicBookDetails.OrderBy(s => s.Title.SortableTitle ?? s.Title.Title);
                    break;
            }

            _comicBookDetails = comicBookDetails.ProjectTo<ComicBookDetailsDTO>(_mapper.ConfigurationProvider);
            return sortingFiltering;
        }

        private IActionResult UpdateDetails(bool wantList, long id, bool detail)
        {
            try
            {
                var currentComicBookDetails = _context.ComicBookDetails.FindAsync(id);
                if (wantList)
                    currentComicBookDetails.Result.Want = detail;
                else
                    currentComicBookDetails.Result.Own = detail;

                _context.Update(currentComicBookDetails.Result);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicBookDetailsExists(id)) return NotFound();

                throw;
            }

            return new EmptyResult();
        }

        private async Task GetUser()
        {
            if (_userId != null) return;
            var user = await GetCurrentUserAsync();
            _userId = user?.Id;
        }

        #endregion Private Methods
    }
}