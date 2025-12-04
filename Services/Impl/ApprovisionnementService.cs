using GesAppro.Models;

namespace GesAppro.Services.Impl;

public class ApprovisionnementService : GesAppro.Services.IApprovisionnementService
{
    private readonly List<Approvisionnement> _data = new();
    private int _nextId = 1;

    public ApprovisionnementService()
    {
        var p1 = new Approvisionnement
        {
            Id = _nextId++,
            Reference = "APP-2023-001",
            Date = new DateTime(2023,4,15),
            Fournisseur = "Textiles Dakar SARL",
            Statut = "Reçu",
            Produits = new List<Produit>
            {
                new Produit { NomProduit = "Tissu A", Quantite = 10, PrixUnitaire = 75000 },
                new Produit { NomProduit = "Boutons", Quantite = 5, PrixUnitaire = 5000 }
            }
        };

        var p2 = new Approvisionnement
        {
            Id = _nextId++,
            Reference = "APP-2023-002",
            Date = new DateTime(2023,4,10),
            Fournisseur = "Mercerie Centrale",
            Statut = "Reçu",
            Produits = new List<Produit>
            {
                new Produit { NomProduit = "Fil", Quantite = 20, PrixUnitaire = 16000 },
                new Produit { NomProduit = "Aiguille", Quantite = 50, PrixUnitaire = 100 }
            }
        };

        var p3 = new Approvisionnement
        {
            Id = _nextId++,
            Reference = "APP-2023-003",
            Date = new DateTime(2023,4,5),
            Fournisseur = "Tissus Premium",
            Statut = "En attente",
            Produits = new List<Produit>
            {
                new Produit { NomProduit = "Tissu B", Quantite = 5, PrixUnitaire = 90000 }
            }
        };

        _data.AddRange(new[] { p1, p2, p3 });
        for (int i = 4; i <= 15; i++)
        {
            var a = new Approvisionnement
            {
                Id = _nextId++,
                Reference = $"APP-2023-{i:000}",
                Date = new DateTime(2023, 4, Math.Max(1, i)),
                Fournisseur = i % 3 == 0 ? "Textiles Dakar SARL" : (i % 3 == 1 ? "Mercerie Centrale" : "Tissus Premium"),
                Statut = i % 4 == 0 ? "En attente" : "Reçu",
                Produits = new List<Produit>
                {
                    new Produit { NomProduit = $"Article {i}", Quantite = i * 2, PrixUnitaire = 10000 + i * 500 }
                }
            };
            _data.Add(a);
        }
    }

    public IEnumerable<Approvisionnement> GetAll() => _data.OrderByDescending(a => a.Date);

    public Approvisionnement? GetById(int id) => _data.FirstOrDefault(a => a.Id == id);

    public void Add(Approvisionnement a)
    {
        a.Id = _nextId++;
        _data.Add(a);
    }

    public string GenerateReference()
    {
        return $"APP-2023-{_nextId:000}";
    }

    public IEnumerable<Approvisionnement> GetRecent(int count = 5) => GetAll().Take(count);

    public decimal TotalMontant() => _data.Sum(a => a.MontantTotal);

    public (string Fournisseur, decimal Montant) PrincipalFournisseur()
    {
        var pair = _data
            .SelectMany(a => a.Produits, (a, prod) => new { a.Fournisseur, Montant = prod.Montant })
            .GroupBy(x => x.Fournisseur)
            .Select(g => new { Fournisseur = g.Key, Montant = g.Sum(x => x.Montant) })
            .OrderByDescending(x => x.Montant)
            .FirstOrDefault();

        if (pair == null) return (string.Empty, 0m);
        return (pair.Fournisseur, pair.Montant);
    }
}
