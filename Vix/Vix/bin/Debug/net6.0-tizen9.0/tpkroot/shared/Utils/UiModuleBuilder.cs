using Tizen.NUI.BaseComponents;
using static TizenDotNet1.shared.Dtos.BuilderDto;

namespace TizenDotNet1.shared.Utils;
public static class UiModuleBuilder
{
    public static View Build(UiModule module)
    {
        return module.ModuleType switch
        {
            "HERO_CAROUSEL" => UiHeroCarouselBuilder.BuildHeroCarousel(module),
            "VIDEO_CAROUSEL" => UiVideoCarouselBuilder.BuildVideoCarousel(module),
            _ => new View()
        };
    }
}
