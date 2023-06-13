using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sccrumproject.Models;


public class ProductController : Controller
{
    private readonly ProductDbContext _dbContext; // Replace "YourDbContext" with your actual DbContext class name

    public ProductController(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: Product
    public async Task<IActionResult> Index()
    {
        var products = await _dbContext.products.ToListAsync();
        return View(products);
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            if (product.ImageFile != null && product.ImageFile.Length > 0)
            {
                // Generate a unique file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);

                // Save the image to a specific location on the server
                string imagePath = Path.Combine("C:\\Users\\syedb\\source\\repos\\sccrumproject\\sccrumproject\\Images\\", fileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                // Set the ImageUrl property of the product
                product.ImageUrl = $"https://yourdomain.com/images/{fileName}";
            }

            _dbContext.products.Add(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _dbContext.products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    // Generate a unique file name
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);

                    // Save the image to a specific location on the server
                    string imagePath = Path.Combine("C:\\Users\\syedb\\source\\repos\\sccrumproject\\sccrumproject\\Images\\", fileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }

                    // Set the ImageUrl property of the product
                    product.ImageUrl = $"https://yourdomain.com/images/{fileName}";
                }

                _dbContext.Update(product);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId))
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
        return View(product);
    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _dbContext.products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _dbContext.products.FindAsync(id);
        _dbContext.products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return _dbContext.products.Any(e => e.ProductId == id);
    }

    public IActionResult Chart()
    {
        var prices = new List<decimal[]>();

        // Add data for each price range
        prices.Add(GetProductsCountByPriceRange(0, 1000));
        prices.Add(GetProductsCountByPriceRange(1001, 2000));
        prices.Add(GetProductsCountByPriceRange(2001, 3000));
        prices.Add(GetProductsCountByPriceRange(3001, 5000));

        ViewBag.Prices = prices;

        return View();
    }

    private decimal[] GetProductsCountByPriceRange(decimal minPrice, decimal maxPrice)
    {
        var count = _dbContext.products.Count(p => p.Price >= minPrice && p.Price <= maxPrice);
        return new decimal[] { minPrice, maxPrice, count };
    }

}
