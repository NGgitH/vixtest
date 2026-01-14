using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TizenDotNet1.shared.Dtos;
/*
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
    }*/

// Data myDeserializedClass = JsonConvert.DeserializeObject<Data>(myJsonResponse);



    public class ClickTrackingJson
    {
        public string ui_carousel_slug { get; set; }
        public string ui_content_id { get; set; }
        public string ui_content_title { get; set; }
        public string ui_content_type { get; set; }
        public string ui_content_group { get; set; }
    }

    public class Contents
    {
        public int totalCount { get; set; }
        public List<Edge> edges { get; set; }
        public PageInfo pageInfo { get; set; }
    }

    public class Data
    {
        public UiPage uiPage { get; set; }
    }

    public class Edge
    {
        public string cursor { get; set; }
        public Node node { get; set; }
    }

    public class Extensions
    {
        public int queryComplexity { get; set; }
        public string traceId { get; set; }
    }

    public class GenresV2
    {
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class Image
    {
        public string filePath { get; set; }
        public string imageRole { get; set; }
        public string link { get; set; }
        public string mediaType { get; set; }
    }

    public class ImageAsset
    {
        public string filePath { get; set; }
        public string imageRole { get; set; }
        public string link { get; set; }
        public string mediaType { get; set; }
    }

    public class Node
    {
        public TrackingMetadataJson trackingMetadataJson { get; set; }
        public string id { get; set; }
        public string moduleType { get; set; }
        public string trackingId { get; set; }
        public string title { get; set; }
        public string treatment { get; set; }
        public LandscapeFillImage landscapeFillImage { get; set; }
        public object portraitFillImage { get; set; }
        public object sponsorMetadata { get; set; }
        public Contents contents { get; set; }
        public Image image { get; set; }
        public LogoImage logoImage { get; set; }
        public Video video { get; set; }
        public ClickTrackingJson clickTrackingJson { get; set; }
    }

    public class LandscapeFillImage
    {
        public string link { get; set; }
    }

    public class LogoImage
    {
        public string link { get; set; }
    }

    public class PageAnalyticsMetadata
    {
    }

    public class PageAvailability
    {
        public bool isBlocked { get; set; }
        public object reason { get; set; }
    }

    public class PageContentAvailability
    {
        public bool isBlocked { get; set; }
        public object reason { get; set; }
    }

    public class PageInfo
    {
        public bool hasPreviousPage { get; set; }
        public bool hasNextPage { get; set; }
        public string startCursor { get; set; }
        public string endCursor { get; set; }
    }

    public class Rating
    {
        public string ratingValue { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public Extensions extensions { get; set; }
    }

    public class TrackingMetadataJson
    {
        public string ui_module_id { get; set; }
        public string ui_module_title { get; set; }
        public string ui_navigation_section { get; set; }
        public bool ui_is_recommendation { get; set; }
        public string ui_object_type { get; set; }
        public string ui_carousel_slug { get; set; }
        public string ui_recommendation_engine { get; set; }
    }

    public class UiModules
    {
        public int totalCount { get; set; }
        public List<Edge> edges { get; set; }
        public PageInfo pageInfo { get; set; }
    }

    public class UiPage
    {
        public string urlPath { get; set; }
        public string pageName { get; set; }
        public PageAnalyticsMetadata pageAnalyticsMetadata { get; set; }
        public PageAvailability pageAvailability { get; set; }
        public PageContentAvailability pageContentAvailability { get; set; }
        public UiModules uiModules { get; set; }
    }

    public class Video
    {
        public string id { get; set; }
        public int copyrightYear { get; set; }
        public DateTime dateReleased { get; set; }
        public string description { get; set; }
        public List<GenresV2> genresV2 { get; set; }
        public object headline { get; set; }
        public List<string> keywords { get; set; }
        public string title { get; set; }
        public List<string> badges { get; set; }
        public string contentVertical { get; set; }
        public List<Rating> ratings { get; set; }
        public List<ImageAsset> imageAssets { get; set; }
        public string videoType { get; set; }
        public VideoTypeData videoTypeData { get; set; }
        public VodAvailability vodAvailability { get; set; }
    }

    public class VideoTypeData
    {
        public string seriesSubType { get; set; }
        public int seasonsCount { get; set; }
        public int episodesCount { get; set; }
    }

    public class VodAvailability
    {
        public bool isBlocked { get; set; }
        public object reason { get; set; }
    }



    /*

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
}*/
