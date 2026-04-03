using carGooBackend.Data;
using carGooBackend.Models;
using HotChocolate;

namespace carGooBackend.GraphQL
{
    public class Mutation
    {
        public async Task<Ponuda> CreatePonuda(
            [Service] CarGooDataContext context,
            string drzavaU,
            string drzavaI,
            string mestoU,
            string mestoI,
            DateTime utovar,
            DateTime istovar,
            double duzina,
            double tezina,
            string tipNadogradnje,
            string tipKamiona,
            string vrstaTereta,
            string zamenaPaleta,
            int cena,
            Guid idPreduzeca,
            string idKorisnika)
        {
            var ponuda = new Ponuda
            {
                PonudaId = Guid.NewGuid(),
                DrzavaU = drzavaU,
                DrzavaI = drzavaI,
                MestoU = mestoU,
                MestoI = mestoI,
                Utovar = DateTime.SpecifyKind(utovar, DateTimeKind.Utc),
                Istovar = DateTime.SpecifyKind(istovar, DateTimeKind.Utc),
                Duzina = duzina,
                Tezina = tezina,
                TipNadogradnje = tipNadogradnje,
                TipKamiona = tipKamiona,
                VrstaTereta = vrstaTereta,
                ZamenaPaleta = zamenaPaleta,
                Cena = cena,
                IdPreduzeca = idPreduzeca,
                IdKorisnika = idKorisnika,
                Vreme = DateTime.UtcNow
            };

            context.Ponude.Add(ponuda);
            await context.SaveChangesAsync();

            return ponuda;
        }

        public async Task<bool> DeletePonuda(
            [Service] CarGooDataContext context,
            Guid ponudaId)
        {
            var ponuda = await context.Ponude.FindAsync(ponudaId);
            if (ponuda == null) return false;

            context.Ponude.Remove(ponuda);
            await context.SaveChangesAsync();
            return true;
        }
    }
}