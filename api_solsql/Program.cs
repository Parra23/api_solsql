
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;

namespace api_solsql
{
    public class Program
    {
           public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Add the database 
            builder.Services.AddDbContext<ContextDB>(options =>
            options.UseMySql(
            builder.Configuration.GetConnectionString("Connection_mysql"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Connection_mysql"))));


            // MOVER CORS AQUÍ (ANTES de builder.Build())
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            // Habilitar el caché de respuestas
            builder.Services.AddResponseCaching();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","API v1");
                c.RoutePrefix = "swagger";  // Ruta para acceder a Swagger
            });
            // Si no necesitas redirección HTTPS, no habilites UseHttpsRedirection.
            // Si estuvieras en producción y quisieras redirigir automáticamente a HTTPS, descomenta la línea:
            //app.UseHttpsRedirection();
            
            // Habilitar el middleware de caché de respuestas
            app.UseResponseCaching();

            // Usa CORS
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
