using HttpConsumeExamples.DTOs;

namespace HttpConsumeExamples.Interfaces
{
    public interface ISalesCarWebWithoutRefitRepository
    {
        Task<List<AnunciosResponse>> ReturnAnuncios(int skip, int take);
    }
}