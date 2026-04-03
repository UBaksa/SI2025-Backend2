using carGooBackend.Data;
using carGooBackend.Models;
using HotChocolate;
using HotChocolate.Data;

namespace carGooBackend.GraphQL
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Ponuda> GetPonude([Service] CarGooDataContext context)
        {
            return context.Ponude;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Preduzece> GetPreduzeca([Service] CarGooDataContext context)
        {
            return context.Preduzeca;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<PonudaVozila> GetPonudaVozila([Service] CarGooDataContext context)
        {
            return context.PonudaVozila;
        }
    }
}