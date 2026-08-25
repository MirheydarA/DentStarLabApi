using System.Text.Json.Serialization;

namespace DentStarLab.Api.Extensions;

public static class OptionExtensions
{
    public static void InitMvc(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            // options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
    }
    
    public static void ApplyCors(this WebApplication app)
    {
		app.UseCors(policy =>
		{
			policy.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
	}
}
