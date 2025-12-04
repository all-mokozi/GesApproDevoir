using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GesAppro.Models;

namespace GesAppro.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly GesAppro.Services.IApprovisionnementService _approService;

    public HomeController(ILogger<HomeController> logger, GesAppro.Services.IApprovisionnementService approService)
    {
        _logger = logger;
        _approService = approService;
    }

    public IActionResult Index(DateTime? start = null, DateTime? end = null, int page = 1, int pageSize = 5)
    {
        var query = _approService.GetAll();

        if (start.HasValue)
        {
            query = query.Where(a => a.Date >= start.Value.Date);
        }
        if (end.HasValue)
        {
            var endOfDay = end.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(a => a.Date <= endOfDay);
        }

        var total = query.Count();
        var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var vm = new GesAppro.Models.DashboardViewModel
        {
            TotalApprovisionnements = _approService.TotalMontant(),
            NombreApprovisionnements = total,
            RecentApprovisionnements = new GesAppro.Models.PaginatedList<GesAppro.Models.Approvisionnement>(items, total, page, pageSize)
        };

        var principal = _approService.PrincipalFournisseur();
        vm.FournisseurPrincipal = principal.Fournisseur;
        vm.FournisseurPrincipalMontant = principal.Montant;

        ViewData["start"] = start?.ToString("yyyy-MM-dd") ?? string.Empty;
        ViewData["end"] = end?.ToString("yyyy-MM-dd") ?? string.Empty;

        return View(vm);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
