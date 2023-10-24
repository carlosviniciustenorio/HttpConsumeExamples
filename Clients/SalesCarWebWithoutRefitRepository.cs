using HttpConsumeExamples.DTOs;
using HttpConsumeExamples.Interfaces;
using Polly;

namespace HttpConsumeExamples.Clients
{
    public class SalesCarWebWithoutRefitRepository : ISalesCarWebWithoutRefitRepository
    {
        private readonly HttpClient _client;
        private readonly AsyncPolicy _policy;

        public SalesCarWebWithoutRefitRepository(HttpClient client, AsyncPolicy policy)
        {
            _client = client;
            _policy = policy;
        }

        public async Task<List<AnunciosResponse>> ReturnAnuncios(int skip, int take)
        {
            var url = $"{_client.BaseAddress}/anuncios/getAll?skip={skip}&take={take}";
            return await _policy.ExecuteAsync(() => _client.GetFromJsonAsync<List<AnunciosResponse>>(url));
        } 
    }
}