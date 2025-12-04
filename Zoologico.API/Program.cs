using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Zoologico.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ZoologicoAPIContext>(options =>
                                 //options.UseSqlServer(builder.Configuration.GetConnectionString("ZoologicoAPIContext.sqlServer") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found.")))
                                 //Usa PostgreSQL:
                                 options.UseNpgsql(builder.Configuration.GetConnectionString("ZoologicoAPIContext.postgresql") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext.postgresql' not found.")));




            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Configure JSON options
            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(
                    options =>
                    options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            /* lo mejor es quitar las llaves que tambien rodean a los usos de swagger , app.UseSwagger y app.UseSwaggerUI para un 
               codigo mas limpio y es una buena practica tener swagger en todos los entornos */

            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}


            //nos quedaria asi , sin las llaves y sin el if
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
