namespace GesAppro.Models;

public class Produit
{
    public string NomProduit { get; set; } = string.Empty;
    public int Quantite { get; set; }
    public decimal PrixUnitaire { get; set; }
    public decimal Montant => Quantite * PrixUnitaire;
}
