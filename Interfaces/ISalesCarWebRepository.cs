using  System.Threading.Tasks;
using HttpConsumeExamples.DTOs;
using  Refit;

namespace HttpConsumeExamples.Interfaces
{
    public interface ISalesCarWebRepository
    {
        [Get("/anuncios")]
        Task<List<AnunciosResponse>> ReturnAnuncios(int skip, int take);
    }
}