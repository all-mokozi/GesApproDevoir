using GesAppro.Models;

namespace GesAppro.Services;

public interface IApprovisionnementService
{
    IEnumerable<Approvisionnement> GetAll();
    Approvisionnement? GetById(int id);
    void Add(Approvisionnement a);
    string GenerateReference();
    IEnumerable<Approvisionnement> GetRecent(int count = 5);
    decimal TotalMontant();
    (string Fournisseur, decimal Montant) PrincipalFournisseur();
}
