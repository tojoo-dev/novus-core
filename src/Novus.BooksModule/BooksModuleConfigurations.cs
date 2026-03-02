using System.Data;
using System.Diagnostics.CodeAnalysis;
using Novus.BooksModule.Features.DeleteBook;
using Novus.BooksModule.Features.GetBook;
using Novus.BooksModule.Features.GetBooks;
using Novus.BooksModule.Features.UpsertBook;
using Novus.BooksModule.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Novus.BooksModule;

[ExcludeFromCodeCoverage]
public static class BooksModuleConfigurations
{
    public static IServiceCollection AddBooksModule(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddEndpointsApiExplorer()
            .AddValidation()
            .AddSingleton<IDbConnection>(sp => new SqliteConnection(configuration.GetConnectionString("SqliteDb")))
            .AddSingleton<DbInitializer>()
            ;

    public static IEndpointRouteBuilder MapBooksModule(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapGroup("/api/books")
            .WithTags("Books")
            .AddEndpointFilter<AuthFilter>()
            .MapGetBooksEndpoint()
            .MapGetBookEndpoint()
            .MapUpsertBookEndpoint()
            .MapDeleteBookEndpoint()
            ;

    public static IHealthChecksBuilder AddBooksModule(this IHealthChecksBuilder builder, IConfiguration configuration)
        => builder.AddSqlite(configuration.GetConnectionString("SqliteDb"), tags: ["ready"]);

    public static IApplicationBuilder InitBooksModule(this IApplicationBuilder app)
    {
        var initializer = app.ApplicationServices.GetRequiredService<DbInitializer>();
        initializer.Init();

        return app;
    }
}
