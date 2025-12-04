using Microsoft.AspNetCore.Mvc;
using GesAppro.Models;

namespace GesAppro.Controllers;

public class ApprovisionnementController : Controller
{
    private readonly GesAppro.Services.IApprovisionnementService _service;

    public ApprovisionnementController(GesAppro.Services.IApprovisionnementService service)
    {
        _service = service;
    }

    public IActionResult Index(DateTime? start = null, DateTime? end = null, int page = 1, int pageSize = 5)
    {
        var query = _service.GetAll();
        if (start.HasValue)
        {
            query = query.Where(a => a.Date >= start.Value);
        }
        if (end.HasValue)
        {
           
            var endOfDay = end.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(a => a.Date <= endOfDay);
        }

        var total = query.Count();
        var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var vm = new GesAppro.Models.PaginatedList<Approvisionnement>(items, total, page, pageSize);
        ViewData["start"] = start?.ToString("yyyy-MM-dd") ?? string.Empty;
        ViewData["end"] = end?.ToString("yyyy-MM-dd") ?? string.Empty;
        return View(vm);
    }

    public IActionResult Create()
    {
        var model = new Approvisionnement { Date = DateTime.Today };
       
        model.Reference = _service.GenerateReference();
        if (model.Produits == null || model.Produits.Count == 0)
        {
            model.Produits = new List<Produit> { new Produit() };
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Approvisionnement model, string? addProduct)
    {
        // If user clicked 'Ajouter un autre article', add an empty product and redisplay the form
        if (!string.IsNullOrEmpty(addProduct))
        {
            if (model.Produits == null) model.Produits = new List<Produit>();
            model.Produits.Add(new Produit());
            return View(model);
        }

        // If user clicked a remove button, the index will be in the Request.Form["remove"]
        var removeVal = Request.Form["remove"].FirstOrDefault();
        if (!string.IsNullOrEmpty(removeVal) && int.TryParse(removeVal, out var removeIdx))
        {
            if (model.Produits != null && removeIdx >= 0 && removeIdx < model.Produits.Count)
            {
                model.Produits.RemoveAt(removeIdx);
            }
            return View(model);
        }

        // Otherwise attempt to save
        if (!ModelState.IsValid) return View(model);
        if (model.Produits == null) model.Produits = new List<Produit>();

        if (string.IsNullOrWhiteSpace(model.Reference))
        {
            model.Reference = _service.GenerateReference();
        }
        if (model.Date == default) model.Date = DateTime.Today;
        _service.Add(model);

        return RedirectToAction("Index", "Home");
    }
}
