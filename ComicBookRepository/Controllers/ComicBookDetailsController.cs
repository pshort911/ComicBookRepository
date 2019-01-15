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
        public async Task<IActionResult> Index(long? id)
        {
            var user = await GetCurrentUserAsync();
            _userId = user?.Id;
            if (_userId == null) return NotFound();

            var comicBookDetails = id == null ? 
                _context.ComicBookDetails.Where(m => m.OwnerId == _userId) : 
                _context.ComicBookDetails.Where(m => m.OwnerId == _userId && m.TitleId == id).Include(t => t.Title);
            return View(comicBookDetails);
        }

        //GET: ComicBookDetails/OwnList
        public async Task<IActionResult> OwnList()
        {
            var user = await GetCurrentUserAsync();
            _userId = user?.Id;
            if (_userId == null) return NotFound();

            var comicBookDetails = _context.ComicBookDetails.Where(m => m.OwnerId == _userId && m.Own).Include(t => t.Title);
            return View(comicBookDetails);
        }

        //GET: ComicBookDetails/WantList
        public async Task<IActionResult> WantList()
        {
            var user = await GetCurrentUserAsync();
            _userId = user?.Id;
            if (_userId == null) return NotFound();

            var comicBookDetails = _context.ComicBookDetails.Where(m => m.OwnerId == _userId && m.Want).Include(t => t.Title);
            return View(comicBookDetails);
        }

        #endregion Public Methods

        #region Private Methods

        private bool ComicBookDetailsExists(long id) => _context.ComicBookDetails.Any(e => e.Id == id);

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        #endregion Private Methods
    }
}