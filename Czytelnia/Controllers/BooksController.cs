using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Czytelnia.Data;
using Czytelnia.Models;

namespace Czytelnia.Controllers
{
    public class BooksController : Controller
    {
        private readonly CzytelniaContext _context;

        public BooksController(CzytelniaContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewData["CurrentSort"]=sortOrder;
            ViewData["Tytuly"] = String.IsNullOrEmpty(sortOrder) ? "tytuly_malejaco" : "";
            ViewData["Autorzy"] = sortOrder == "autorzy_malejaco" ? "autorzy_rosnaco" : "autorzy_malejaco";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var books = from b in _context.Books.Include(a=>a.Autor)
                           select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Autor.Nazwisko.Contains(searchString)
                                       || b.Tytul.Contains(searchString)
                                       || b.Autor.Imie.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "tytuly_malejaco":
                    books = books.OrderByDescending(b => b.Tytul);
                    break;
                case "autorzy_rosnaco":
                    books = books.OrderBy(b => b.Autor.Nazwisko);
                    break;
                case "autorzy_malejaco":
                    books = books.OrderByDescending(b => b.Autor.Nazwisko);
                    break;
                default:
                    books = books.OrderBy(b => b.Tytul);
                    break;
            }
            int pageSize = 10;
            //return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), page ?? 1, pageSize));
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(),page ?? 1, pageSize));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(a=>a.Autor)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            PopulateAuthorDropdownList();
            PopulateGatunekDropDownList();
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,Tytul,AuthorID,Gatunek")] Book book)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
            "Try again, and if the problem persists " +
            "see your system administrator.");
            }
            PopulateAuthorDropdownList(book.Autor);
            PopulateGatunekDropDownList(book.Gatunek);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.SingleOrDefaultAsync(m => m.BookID == id);

            if (book == null)
            {
                return NotFound();
            }
            PopulateAuthorDropdownList(book.Autor);
            PopulateGatunekDropDownList(book.Gatunek);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bookToUpdate = await _context.Books
                .SingleOrDefaultAsync(c => c.BookID == id);
            
                if (await TryUpdateModelAsync<Book>(bookToUpdate,
                    "",
                    c => c.Tytul, c => c.AuthorID, c => c.Gatunek))
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException /* ex */)
                    {
                        //Log the error (uncomment ex variable name and write a log.)
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                    return RedirectToAction(nameof(Index));
                
                 }
            PopulateAuthorDropdownList(bookToUpdate.Autor);
            PopulateGatunekDropDownList(bookToUpdate.Gatunek);
            return View(bookToUpdate);
        }

        private void PopulateAuthorDropdownList(object selectedAuthor = null)
        {
            var departmentsQuery = from d in _context.Authors
                                   orderby d.Nazwisko
                                   select d;
            ViewBag.Autor = new SelectList(departmentsQuery.AsNoTracking(), "AuthorID", "Nazwisko", selectedAuthor);
        }
        private void PopulateGatunekDropDownList(object selectedGatunek = null)
        {
            var gatunki = from Gatunek d in Enum.GetValues(typeof(Gatunek))
                             select new { ID = (int)d, Name = d.ToString() };
            ViewBag.Gatunek = new SelectList(gatunki, "ID", "Name", selectedGatunek);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(a => a.Autor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
