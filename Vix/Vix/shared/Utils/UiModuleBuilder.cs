using Tizen.NUI.BaseComponents;
using TizenDotNet1.shared.Dtos;

namespace TizenDotNet1.shared.Utils;
public static class UiModuleBuilder
{
    public static View Build(Node node, string moduleType, string name = "", string section = "")
    {
        return moduleType switch
        {
            "HERO_CAROUSEL" => UiHeroCarouselBuilder.BuildHeroCarousel(node, section),
            "VIDEO_CAROUSEL" => UiVideoCarouselBuilder.BuildVideoCarousel(node, name),
            _ => new View()
        };
    }
}
