using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MVC.CRUD.Whitout.EF.Models;
using System;
using System.Data;

namespace MVC.CRUD.Whitout.EF.Controllers
{
    public class BookController : Controller
    {

        #region I did manually

        // Inject the configuration interface
        private readonly IConfiguration _configuration;
        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Book
        public IActionResult Index()
        {
            DataTable table = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SP_BookViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("Opc", 1);
                sqlDa.Fill(table);
            }

            return View(table);
        }

        // GET: Book/AddOrEdit/
        public IActionResult AddOrEdit(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            if (id > 0)
                bookViewModel = FetchBookById(id);

            return View(bookViewModel);
        }

        // POST: Book/AddOrEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookId,Title,Author,Price")] BookViewModel bookViewModel)
        {

            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("SP_UpdateDataInBooks", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("Opc", 1);
                    sqlCommand.Parameters.AddWithValue("BookId", bookViewModel.BookId);
                    sqlCommand.Parameters.AddWithValue("Title", bookViewModel.Title);
                    sqlCommand.Parameters.AddWithValue("Author", bookViewModel.Author);
                    sqlCommand.Parameters.AddWithValue("Price", bookViewModel.Price);
                    sqlCommand.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
        {

            BookViewModel bookViewModel = FetchBookById(id);
            return View(bookViewModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SP_UpdateDataInBooks", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("Opc", 2);
                sqlCommand.Parameters.AddWithValue("BookId", id);
                sqlCommand.ExecuteNonQuery();
            }

            return RedirectToAction(nameof(Index));
        }

        public BookViewModel FetchBookById(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable table = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SP_BookViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("Opc", 2);
                sqlDa.SelectCommand.Parameters.AddWithValue("BookId", id);
                sqlDa.Fill(table);

                if (table.Rows.Count == 1)
                {
                    bookViewModel.BookId = Convert.ToInt32(table.Rows[0]["BookId"].ToString());
                    bookViewModel.Title = table.Rows[0]["Title"].ToString();
                    bookViewModel.Author = table.Rows[0]["Author"].ToString();
                    bookViewModel.Price = Convert.ToInt32(table.Rows[0]["Price"].ToString());
                }
            }
            return bookViewModel;
        }

        #endregion




        #region Generate By Scaffolding
        /*
        private readonly MVCCRUDWhitoutEFContext _context;

        // Remove constructor with Injection Dependency "context"
        public BookController(MVCCRUDWhitoutEFContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookViewModel.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookViewModel = await _context.BookViewModel
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Author,Price")] BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookViewModel = await _context.BookViewModel.FindAsync(id);
            if (bookViewModel == null)
            {
                return NotFound();
            }
            return View(bookViewModel);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Author,Price")] BookViewModel bookViewModel)
        {
            if (id != bookViewModel.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookViewModelExists(bookViewModel.BookId))
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
            return View(bookViewModel);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookViewModel = await _context.BookViewModel
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookViewModel = await _context.BookViewModel.FindAsync(id);
            _context.BookViewModel.Remove(bookViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookViewModelExists(int id)
        {
            return _context.BookViewModel.Any(e => e.BookId == id);
        }
        */
        #endregion
    }
}
