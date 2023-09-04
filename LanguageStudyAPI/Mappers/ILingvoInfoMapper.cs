using LanguageStudyAPI.Models;

namespace LingvoInfoAPI.Mappers
{
    public interface ILingvoInfoMapper<T>
    {
        LingvoInfo MapToLingvoInfo(T obj);
    }
}
