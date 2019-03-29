using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ComicBookRepository.Core;
using ComicBookRepository.Data;

namespace ComicBookRepository.Controllers
{
    public class ComicBookTitlesController : Controller
    {
        #region Private Fields

        private readonly ComicBookRepositoryContext _context;
        private readonly IMapper _mapper;

        #endregion Private Fields

        #region Public Constructors

        public ComicBookTitlesController(ComicBookRepositoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion Public Constructors

        #region Public Methods

        // GET: ComicBookTitles/Create
        public IActionResult Create() => View();

        // POST: ComicBookTitles/Create To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,FirstIssue,LastIssue,NumIssues,NumSpIssues,LimitedSeries,SortableTitle")] ComicBookTitleDTO comicBookTitle)
        {
            if (!ModelState.IsValid)
            {
                return View(comicBookTitle);
            }

            _context.Add(comicBookTitle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ComicBookTitles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comicBookTitle = await _context.ComicBookTitle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comicBookTitle == null)
            {
                return NotFound();
            }

            var comicBookTitleDto = _mapper.Map<ComicBookTitleDTO>(comicBookTitle);
            return View(comicBookTitleDto);
        }

        // POST: ComicBookTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var comicBookTitle = await _context.ComicBookTitle.FindAsync(id);
            _context.ComicBookTitle.Remove(comicBookTitle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ComicBookTitles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comicBookTitle = await _context.ComicBookTitle.FindAsync(id);
            if (comicBookTitle == null)
            {
                return NotFound();
            }
            var comicBookTitleDto = _mapper.Map<ComicBookTitleDTO>(comicBookTitle);
            return View(comicBookTitleDto);
        }

        // POST: ComicBookTitles/Edit/5 To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,FirstIssue,LastIssue,NumIssues,NumSpIssues,LimitedSeries, SortableTitle")] ComicBookTitleDTO comicBookTitleDto)
        {
            if (id != comicBookTitleDto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(comicBookTitleDto);
            }

            try
            {
                _context.Update(_mapper.Map<ComicBookTitle>(comicBookTitleDto));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicBookTitleExists(comicBookTitleDto.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index), new { id });
        }

        // GET: ComicBookTitles
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            var titleList = from s in _context.ComicBookTitle select s;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                titleList = titleList.Where(s => s.Title.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    titleList = titleList.OrderByDescending(s => s.SortableTitle ?? s.Title);
                    break;

                default:
                    titleList = titleList.OrderBy(s => s.SortableTitle ?? s.Title);
                    break;
            }

            var pageSize = 100;
            return View(await PaginatedList<ComicBookTitleDTO>.CreateAsync(titleList.ProjectTo<ComicBookTitleDTO>(_mapper.ConfigurationProvider), page ?? 1, pageSize));
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Update(ComicBookTitleDTO comicBookTitle)
        {
            if (!ModelState.IsValid)
            {
                return new EmptyResult();
            }

            try
            {
                _context.Update(comicBookTitle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicBookTitleExists(comicBookTitle.Id))
                {
                    return NotFound();
                }

                throw;
            }
            return new EmptyResult();
        }

        #endregion Public Methods

        #region Private Methods

        private bool ComicBookTitleExists(long id) => _context.ComicBookTitle.Any(e => e.Id == id);

        #endregion Private Methods
    }
}