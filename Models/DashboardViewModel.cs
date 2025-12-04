namespace GesAppro.Models;

public class DashboardViewModel
{
    public decimal TotalApprovisionnements { get; set; }
    public int NombreApprovisionnements { get; set; }
    public string FournisseurPrincipal { get; set; } = string.Empty;
    public decimal FournisseurPrincipalMontant { get; set; }
    public PaginatedList<Approvisionnement> RecentApprovisionnements { get; set; } = new PaginatedList<Approvisionnement>(new List<Approvisionnement>(), 0, 1, 5);
}
