using HttpConsumeExamples.Interfaces;
using Refit;

namespace HttpConsumeExamples.Clients
{
    public static class AddRefitClientExtensions
    {
        public static void AddRefitClients(this IServiceCollection services)
        {
            //Using Refit
            services.AddRefitClient<ISalesCarWebRepository>().ConfigureHttpClient(c => {
                c.BaseAddress = new Uri("http://localhost:8000/api");
            });

            // Using HTTP Client
            services.AddHttpClient<ISalesCarWebWithoutRefitRepository, SalesCarWebWithoutRefitRepository>(c => {
                c.BaseAddress = new Uri("http://localhost:8000/api");
                c.Timeout = new TimeSpan(0, 0, 10);
            });
        }
    }
}