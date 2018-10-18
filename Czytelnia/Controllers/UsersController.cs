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
    public class UsersController : Controller
    {
        private readonly CzytelniaContext _context;

        public UsersController(CzytelniaContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(b=>b.Books).AsNoTracking().ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(b=>b.Books)
                    .ThenInclude(b=>b.Autor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var Books = new List<Book>();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Imie,Nazwisko,Zapisany_od,Books")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id, string[] oddane)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u=>u.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserID == id); ;
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Imie,Nazwisko,Zapisany_od,Books")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        private void PopulateBookDropdownList(string searchString)
        {
            //string query = "SELECT * FROM Book WHERE UserID = null";
            var booksQuery = from d in _context.Books
                                   select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b => b.Tytul.Contains(searchString)).OrderBy(b=>b.Tytul);
            }
            
            ViewBag.Books = new List<Book>(booksQuery.Where(b=>b.UserID==null).Include(b=>b.Autor).AsNoTracking());
            
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
        public async Task<IActionResult> Wypozycz(int? id, string currentFilter,string searchString)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["CurrentFilter"] = searchString;
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            var user = await _context.Users
                .Include(u=>u.Books)
                    .ThenInclude(b =>b.Autor)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserID == id); ;
            if (user == null)
            {
                return NotFound();
            }
            PopulateBookDropdownList(searchString);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Wypozycz(int id, [Bind("UserID,Imie,Nazwisko,Zapisany_od,Books")] User user, int[] selectedBooks,string searchString,string currentFilter)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }
            ViewData["CurrentFilter"] = searchString;
            if (searchString==null)
            {
                searchString = currentFilter;
            }
            user = await _context.Users
                .Include(u => u.Books)
                    .ThenInclude(b=>b.Autor)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.UserID == id);

            if (ModelState.IsValid)
            {                
                UpdateUserBooks(user, selectedBooks);
                try
                {
                    _context.Update(user);                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                PopulateBookDropdownList(searchString);
                return RedirectToAction(nameof(Wypozycz));
            }
            PopulateBookDropdownList(searchString);
            return View(user);
        }
        private void UpdateUserBooks(User userToUpdate, int[] selectedBooks)
        {
            
            if(selectedBooks == null)
            {
                userToUpdate.Books = new List<Book>();
                return;
            }
            var selectedBooksHS = new HashSet<int>(selectedBooks);
            var currentBooks = new HashSet<int>
                (userToUpdate.Books.Select(b => b.BookID));
            foreach(var book in _context.Books.Include(a=>a.Autor).AsNoTracking())
            {
                if(selectedBooksHS.Contains(book.BookID))
                {
                    if(currentBooks.Contains(book.BookID))
                    {
                        Book removedBook = userToUpdate.Books.SingleOrDefault(b => b.BookID == book.BookID);
                        _context.Remove(removedBook);
                        _context.Books.Add(new Book { Tytul = book.Tytul, AuthorID = book.AuthorID, Autor = book.Autor, Gatunek = book.Gatunek });
                        //userToUpdate.Books.Remove(removedBook);
                        //_context.Update(removedBook);
                    }
                    else
                    {
                        userToUpdate.Books.Add(new Book { BookID = book.BookID, Tytul = book.Tytul, AuthorID = book.AuthorID, Autor = book.Autor, Gatunek = book.Gatunek });
                    }
                }
            }
        }
    }

   
}
