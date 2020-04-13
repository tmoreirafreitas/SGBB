using Microsoft.Extensions.DependencyInjection;
using SGBB.Service;

namespace SGBB.Infra.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<GerenciadorBoletoBancario>();
        }
    }
}
