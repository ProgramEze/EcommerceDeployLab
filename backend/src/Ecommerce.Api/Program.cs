namespace Ecommerce.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHealthChecks();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapHealthChecks("/health");

        app.MapGet(
            "/",
            () =>
                Results.Ok(
                    new
                    {
                        application = "EcommerceDeployLab API",
                        status = "Running",
                        environment = app.Environment.EnvironmentName,
                    }
                )
        );

        app.Run();
    }
}
