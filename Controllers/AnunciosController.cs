using HttpConsumeExamples.DTOs;
using HttpConsumeExamples.DTOs.Request;
using HttpConsumeExamples.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;

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
            List<AnunciosResponse> anuncios = new();
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    var result = await _asyncPolicy.ExecuteAsync(() =>
                        _salesCarWebRepository.ReturnAnuncios(query.Skip, query.Take));
                    Console.WriteLine("Sucesso na consulta");
                    anuncios = result;


                    // var anuncios = await _salesCarWebWithoutRefitRepository.ReturnAnuncios(query.Skip, query.Take);
                    return anuncios;
                }
                catch (BrokenCircuitException)
                {
                    Console.WriteLine("Circuit breaker is open. Requests are blocked.");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error to consume: {nameof(_salesCarWebRepository.ReturnAnuncios)}");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
            return anuncios;
        }
    }
}