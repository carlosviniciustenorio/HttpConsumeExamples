using HttpConsumeExamples.DTOs;
using HttpConsumeExamples.DTOs.Request;
using HttpConsumeExamples.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Polly;

namespace HttpConsumeExamples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnunciosController : ControllerBase
    {
        private readonly ISalesCarWebRepository _salesCarWebRepository;
        private readonly ISalesCarWebWithoutRefitRepository _salesCarWebWithoutRefitRepository;
        private readonly AsyncPolicy _asyncPolicy;

        public AnunciosController(ISalesCarWebRepository salesCarWebRepository, AsyncPolicy asyncPolicy, ISalesCarWebWithoutRefitRepository salesCarWebWithoutRefitRepository)
        {
            _salesCarWebRepository = salesCarWebRepository;
            _asyncPolicy = asyncPolicy;
            _salesCarWebWithoutRefitRepository = salesCarWebWithoutRefitRepository;
        }

        [HttpGet]
        public async Task<List<AnunciosResponse>> GetAll([FromQuery] AnunciosQueryStringRequest query)
        {
            try
            {
                var anuncios = await _asyncPolicy.ExecuteAsync(() => _salesCarWebRepository.ReturnAnuncios(query.Skip, query.Take));
                // var anuncios = await _salesCarWebWithoutRefitRepository.ReturnAnuncios(query.Skip, query.Take);
                return anuncios;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");
                throw;
            }
        }
    }
}