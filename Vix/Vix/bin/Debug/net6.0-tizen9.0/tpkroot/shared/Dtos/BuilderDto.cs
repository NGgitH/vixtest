using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TizenDotNet1.shared.Dtos;
public class BuilderDto
{

    // Root
    public class UiResponse
    {
        [JsonPropertyName("data")]
        public UiData Data { get; set; }

        [JsonPropertyName("extensions")]
        public Extensions Extensions { get; set; }
    }

    // ---------------- DATA ----------------
    public class UiData
    {
        [JsonPropertyName("uiPage")]
        public UiPage UiPage { get; set; }
    }

    public class UiPage
    {
        [JsonPropertyName("pageName")]
        public string PageName { get; set; }

        [JsonPropertyName("urlPath")]
        public string UrlPath { get; set; }

        [JsonPropertyName("uiModules")]
        public UiModules UiModules { get; set; }
    }

    // ---------------- MODULES ----------------
    public class UiModules
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("edges")]
        public List<UiModuleEdge> Edges { get; set; }
    }

    public class UiModuleEdge
    {
        [JsonPropertyName("__typename")]
        public string Typename { get; set; }

        [JsonPropertyName("cursor")]
        public string Cursor { get; set; }

        [JsonPropertyName("node")]
        public UiModule Node { get; set; }
    }

    public class UiModule
    {
        [JsonPropertyName("trackingMetadataJson")]
        public TrackingMetadata TrackingMetadataJson { get; set; }

        [JsonPropertyName("moduleType")]
        public string ModuleType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("scrollingTimeSeconds")]
        public int? ScrollingTimeSeconds { get; set; }

        [JsonPropertyName("contents")]
        public UiModuleContents Contents { get; set; }
    }

    // ---------------- TRACKING ----------------
    public class TrackingMetadata
    {
        [JsonPropertyName("ui_module_id")]
        public string UiModuleId { get; set; }

        [JsonPropertyName("ui_module_title")]
        public string UiModuleTitle { get; set; }

        [JsonPropertyName("ui_navigation_section")]
        public string UiNavigationSection { get; set; }

        [JsonPropertyName("ui_is_recommendation")]
        public bool UiIsRecommendation { get; set; }

        [JsonPropertyName("ui_object_type")]
        public string UiObjectType { get; set; }

        [JsonPropertyName("ui_carousel_slug")]
        public string UiCarouselSlug { get; set; }
    }

    // ---------------- CONTENTS ----------------
    public class UiModuleContents
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("edges")]
        public List<UiContentEdge> Edges { get; set; }
    }

    public class UiContentEdge
    {
        [JsonPropertyName("cursor")]
        public string Cursor { get; set; }

        [JsonPropertyName("node")]
        public UiContentNode Node { get; set; }
    }

    public class UiContentNode
    {
        [JsonPropertyName("textTitle")]
        public string TextTitle { get; set; }

        [JsonPropertyName("heroTargetContentType")]
        public string HeroTargetContentType { get; set; }

        [JsonPropertyName("heroTarget")]
        public object HeroTarget { get; set; }

        [JsonPropertyName("heroImageUrl")]
        public string HeroImageUrl { get; set; }

        [JsonPropertyName("heroTitle")]
        public string HeroTitle { get; set; }
    }

    // ---------------- EXTENSIONS ----------------
    public class Extensions
    {
        [JsonPropertyName("queryComplexity")]
        public int QueryComplexity { get; set; }

        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }
    }
}
