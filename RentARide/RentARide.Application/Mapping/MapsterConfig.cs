using Mapster;

namespace FlowApp.Application.Mappings;

/// <summary>
/// Mapster configuration for entity to DTO mappings
/// Automatically discovers and registers all mapping configurations from MappingConfigurations folder
/// </summary>
public static class MapsterConfig
{
    /// <summary>
    /// Configure all mappings for the application
    /// Automatically discovers all IRegister implementations in the MappingConfigurations namespace
    /// </summary>
    public static void ConfigureMappings()
    {
        // Automatically discover and register all mapping configurations
        // Similar to how Entity Framework discovers IEntityTypeConfiguration
        TypeAdapterConfig.GlobalSettings.Scan(typeof(MapsterConfig).Assembly);
    }
}
