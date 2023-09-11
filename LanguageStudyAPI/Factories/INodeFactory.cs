using LanguageStudyAPI.Models.Lingvo;

namespace LingvoInfoAPI.Factories
{
    public interface INodeFactory
    {
        Node CreateNode(NodeType nodeType);
    }
}
