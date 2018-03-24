using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppBot.Model
{
    public partial class Article
    {
        [BsonElement("vector")]
        public List<float> Vector { get; set; }

        [BsonElement("selfLink")]
        public Link SelfLink { get; set; }

        [BsonElement("canonicalWebLink")]
        public Link CanonicalWebLink { get; set; }

        [BsonElement("contentType")]
        public ContentType ContentType { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }

        [BsonElement("publishedFirstTimeAt")]
        public System.DateTimeOffset PublishedFirstTimeAt { get; set; }

        [BsonElement("publishedLastTimeAt")]
        public System.DateTimeOffset PublishedLastTimeAt { get; set; }

        [BsonElement("subSector")]
        public SubSector SubSector { get; set; }

        [BsonElement("themeTag")]
        public ContentType ThemeTag { get; set; }

        [BsonElement("subThemeTags")]
        public ContentType[] SubThemeTags { get; set; }

        [BsonElement("regionTags")]
        public ContentType[] RegionTags { get; set; }

        [BsonElement("isOfRegionalInterestOnly")]
        public bool IsOfRegionalInterestOnly { get; set; }

        [BsonElement("subjectTags")]
        public object[] SubjectTags { get; set; }

        [BsonElement("customTags")]
        public object[] CustomTags { get; set; }

        [BsonElement("signatures")]
        public object[] Signatures { get; set; }

        [BsonElement("signaturesV2")]
        public object[] SignaturesV2 { get; set; }

        [BsonElement("pressAgencies")]
        public PressAgency[] PressAgencies { get; set; }

        [BsonElement("isDispatch")]
        public bool IsDispatch { get; set; }

        [BsonElement("areCommentsEnabled")]
        public bool AreCommentsEnabled { get; set; }

        [BsonElement("isWitnessInvitationEnabled")]
        public bool IsWitnessInvitationEnabled { get; set; }

        [BsonElement("summaryMultimediaItem")]
        public ShareableSummaryMultimediaContent SummaryMultimediaItem { get; set; }

        [BsonElement("summaryMultimediaContentForDetail")]
        public ShareableSummaryMultimediaContent SummaryMultimediaContentForDetail { get; set; }

        [BsonElement("summaryMultimediaContent")]
        public ShareableSummaryMultimediaContent SummaryMultimediaContent { get; set; }

        [BsonElement("body")]
        public Body Body { get; set; }

        [BsonElement("relatedContents")]
        public object[] RelatedContents { get; set; }

        [BsonElement("previousCanonicalWebLinks")]
        public object[] PreviousCanonicalWebLinks { get; set; }

        [BsonElement("geoTags")]
        public object[] GeoTags { get; set; }

        [BsonElement("primaryClassificationTag")]
        public PressAgency PrimaryClassificationTag { get; set; }

        [BsonElement("shareableTitle")]
        public string ShareableTitle { get; set; }

        [BsonElement("shareableSummary")]
        public string ShareableSummary { get; set; }

        [BsonElement("shareableSummaryMultimediaContent")]
        public ShareableSummaryMultimediaContent ShareableSummaryMultimediaContent { get; set; }

        [BsonElement("keyPhrases")]
        public string[] KeyPhrases { get; set; }
    }

    public partial class Body
    {
        [BsonElement("html")]
        public string Html { get; set; }

        [BsonElement("attachments")]
        public Attachment[] Attachments { get; set; }
    }

    public partial class Attachment
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("anchor")]
        public Anchor Anchor { get; set; }

        [BsonElement("htmlSnippet")]
        public HtmlSnippet HtmlSnippet { get; set; }
    }

    public partial class Anchor
    {
        [BsonElement("fragmentId")]
        public string FragmentId { get; set; }
    }

    public partial class HtmlSnippet
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("html")]
        public string Html { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }

    public partial class Link
    {
        [BsonElement("href")]
        public string Href { get; set; }
    }

    public partial class ContentType
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }

    public partial class PressAgency
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("codeName")]
        public string CodeName { get; set; }
    }

    public partial class ShareableSummaryMultimediaContent
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("selfLink")]
        public Link SelfLink { get; set; }

        [BsonElement("contentType")]
        public ContentType ContentType { get; set; }

        [BsonElement("futureId")]
        public string FutureId { get; set; }

        [BsonElement("contentSubtype")]
        public PressAgency ContentSubtype { get; set; }

        [BsonElement("primaryClassificationTag")]
        public PressAgency PrimaryClassificationTag { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("futureDuration")]
        public long FutureDuration { get; set; }

        [BsonElement("seekTime")]
        public long SeekTime { get; set; }

        [BsonElement("isExternalPlayAllowed")]
        public bool IsExternalPlayAllowed { get; set; }

        [BsonElement("downloadableFile")]
        public object DownloadableFile { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }

        [BsonElement("summaryImage")]
        public SummaryImage SummaryImage { get; set; }

        [BsonElement("broadcastedFirstTimeAt")]
        public System.DateTimeOffset BroadcastedFirstTimeAt { get; set; }

        [BsonElement("credits")]
        public string Credits { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("duration")]
        public System.DateTimeOffset Duration { get; set; }

        [BsonElement("contentTypeClassificationTag")]
        public ContentType ContentTypeClassificationTag { get; set; }
    }

    public partial class SummaryImage
    {
        [BsonElement("contentType")]
        public ContentType ContentType { get; set; }

        [BsonElement("alt")]
        public string Alt { get; set; }

        [BsonElement("legend")]
        public string Legend { get; set; }

        [BsonElement("imageCredits")]
        public string ImageCredits { get; set; }

        [BsonElement("pressAgencies")]
        public PressAgency[] PressAgencies { get; set; }

        [BsonElement("imageCollection")]
        public string ImageCollection { get; set; }

        [BsonElement("concreteImages")]
        public ConcreteImage[] ConcreteImages { get; set; }
    }

    public partial class ConcreteImage
    {
        [BsonElement("mediaLink")]
        public Link MediaLink { get; set; }

        [BsonElement("width")]
        public long Width { get; set; }

        [BsonElement("height")]
        public long Height { get; set; }

        [BsonElement("dimensionRatio")]
        public string DimensionRatio { get; set; }
    }

    public partial class SubSector
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("region")]
        public object Region { get; set; }
    }
}