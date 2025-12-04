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
            // include entire end day
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
        // Prefill generated reference so user sees it (read-only in the form)
        model.Reference = _service.GenerateReference();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Approvisionnement model)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.Produits == null) model.Produits = new List<Produit>();
        // generate reference server-side
        if (string.IsNullOrWhiteSpace(model.Reference))
        {
            model.Reference = _service.GenerateReference();
        }
        if (model.Date == default) model.Date = DateTime.Today;
        _service.Add(model);
        // After creating, redirect to the Home dashboard
        return RedirectToAction("Index", "Home");
    }
}
