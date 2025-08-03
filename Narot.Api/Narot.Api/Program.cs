
using Narot.Application.Interfaces;
using Narot.Application.Services;
using Narot.Infrastructure.Repositories;
using Narot.Infrastructure.Services;
using DotNetEnv;

namespace Narot.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load(".env-secrets");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<INarotRepository>(new FSNarotRepository(Env.GetString("FS-PID"), Env.GetString("FS-CRED")));
            builder.Services.AddSingleton<IGeminiService>(new GeminiService(Env.GetString("GGAPI-KEY"), Env.GetString("GGAPI-EP")));
            builder.Services.AddScoped<INarotService, NarotServices>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
