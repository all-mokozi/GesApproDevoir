using System.ComponentModel.DataAnnotations;

namespace GesAppro.Models;

public class Approvisionnement
{
    public int Id { get; set; }
    [Display(Name = "Référence")]
    public string Reference { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Fournisseur { get; set; } = string.Empty;
    public List<Produit> Produits { get; set; } = new List<Produit>();
    public string Statut { get; set; } = "Reçu";
    public decimal MontantTotal => Produits.Sum(p => p.Montant);
}
