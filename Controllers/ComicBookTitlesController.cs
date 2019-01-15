using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace ComicBookRepository.Controllers
{
    public class ComicBookTitlesController : Controller
    {
        private readonly ComicBookRepositoryContext _context;

        public ComicBookTitlesController(ComicBookRepositoryContext context) => _context = context;

        // GET: ComicBookTitles
        public async Task<IActionResult> Index()
        {
            var titleList = await _context.ComicBookTitle.ToListAsync();
            return View(titleList.OrderBy(x => x.SortableTitle ?? x.Title));
        }


        // GET: ComicBookTitles/Create
        public IActionResult Create() => View();

        // POST: ComicBookTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,FirstIssue,LastIssue,NumIssues,NumSpIssues,LimitedSeries,SortableTitle")] ComicBookTitle comicBookTitle)
        {
            if (!ModelState.IsValid) return View(comicBookTitle);
            _context.Add(comicBookTitle);
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
            return View(comicBookTitle);
        }

        // POST: ComicBookTitles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,FirstIssue,LastIssue,NumIssues,NumSpIssues,LimitedSeries, SortableTitle")] ComicBookTitle comicBookTitle)
        {
            if (id != comicBookTitle.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(comicBookTitle);
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

            return RedirectToAction(nameof(Index), new {id});
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

            return View(comicBookTitle);
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

        private bool ComicBookTitleExists(long id) => _context.ComicBookTitle.Any(e => e.Id == id);
    }
}
