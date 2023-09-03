using Mapster;

namespace LanguageStudyAPI.Mappers
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            //TypeAdapterConfig<Student, StudentDTO>
            //    .NewConfig()
            //    .Map(dest => dest.Name, src => src.FullName);

            //TypeAdapterConfig.GlobalSettings.NewConfig<Student, StudentDTO>()
            //.Map(dest => dest.Name, src => src.FullName);
            // Add more configurations as needed
        }
    }
}
