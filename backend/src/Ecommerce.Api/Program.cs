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

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "EcommerceDeployLab API v1");
                options.RoutePrefix = "swagger";
            });
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
            )
            .WithName("Root");

        app.Run();
    }
}
