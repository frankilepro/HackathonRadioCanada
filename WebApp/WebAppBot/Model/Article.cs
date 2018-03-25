using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppBot.Model
{
    [BsonIgnoreExtraElements]
    public class Article
    {
        [BsonId]
        public ObjectId MongoId { get; set; }


        [BsonElement("vector")]
        public float[] Vector { get; set; }

        [BsonIgnore]
        [BsonElement("selfLink")]
        public Link SelfLink { get; set; }

        [BsonIgnore]
        [BsonElement("canonicalWebLink")]
        public Link CanonicalWebLink { get; set; }

        [BsonIgnore]
        [BsonElement("contentType")]
        public ContentType ContentType { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }

        [BsonIgnore]
        [BsonElement("publishedFirstTimeAt")]
        public System.DateTimeOffset PublishedFirstTimeAt { get; set; }

        [BsonIgnore]
        [BsonElement("publishedLastTimeAt")]
        public System.DateTimeOffset PublishedLastTimeAt { get; set; }

        [BsonIgnore]
        [BsonElement("subSector")]
        public SubSector SubSector { get; set; }

        [BsonIgnore]
        [BsonElement("themeTag")]
        public ContentType ThemeTag { get; set; }

        [BsonIgnore]
        [BsonElement("subThemeTags")]
        public ContentType[] SubThemeTags { get; set; }

        [BsonIgnore]
        [BsonElement("regionTags")]
        public ContentType[] RegionTags { get; set; }

        [BsonIgnore]
        [BsonElement("isOfRegionalInterestOnly")]
        public bool IsOfRegionalInterestOnly { get; set; }

        [BsonIgnore]
        [BsonElement("subjectTags")]
        public object[] SubjectTags { get; set; }

        [BsonIgnore]
        [BsonElement("customTags")]
        public object[] CustomTags { get; set; }

        [BsonIgnore]
        [BsonElement("signatures")]
        public object[] Signatures { get; set; }

        [BsonIgnore]
        [BsonElement("signaturesV2")]
        public object[] SignaturesV2 { get; set; }

        [BsonIgnore]
        [BsonElement("pressAgencies")]
        public PressAgency[] PressAgencies { get; set; }

        [BsonIgnore]
        [BsonElement("isDispatch")]
        public bool IsDispatch { get; set; }

        [BsonIgnore]
        [BsonElement("areCommentsEnabled")]
        public bool AreCommentsEnabled { get; set; }

        [BsonIgnore]
        [BsonElement("isWitnessInvitationEnabled")]
        public bool IsWitnessInvitationEnabled { get; set; }

        [BsonIgnore]
        [BsonElement("summaryMultimediaItem")]
        public ShareableSummaryMultimediaContent SummaryMultimediaItem { get; set; }

        [BsonIgnore]
        [BsonElement("summaryMultimediaContentForDetail")]
        public ShareableSummaryMultimediaContent SummaryMultimediaContentForDetail { get; set; }

        [BsonIgnore]
        [BsonElement("summaryMultimediaContent")]
        public ShareableSummaryMultimediaContent SummaryMultimediaContent { get; set; }

        [BsonIgnore]
        [BsonElement("body")]
        public Body Body { get; set; }

        [BsonIgnore]
        [BsonElement("relatedContents")]
        public object[] RelatedContents { get; set; }

        [BsonIgnore]
        [BsonElement("previousCanonicalWebLinks")]
        public object[] PreviousCanonicalWebLinks { get; set; }

        [BsonIgnore]
        [BsonElement("geoTags")]
        public object[] GeoTags { get; set; }

        [BsonIgnore]
        [BsonElement("primaryClassificationTag")]
        public PressAgency PrimaryClassificationTag { get; set; }

        [BsonIgnore]
        [BsonElement("shareableTitle")]
        public string ShareableTitle { get; set; }

        [BsonIgnore]
        [BsonElement("shareableSummary")]
        public string ShareableSummary { get; set; }

        [BsonIgnore]
        [BsonElement("shareableSummaryMultimediaContent")]
        public ShareableSummaryMultimediaContent ShareableSummaryMultimediaContent { get; set; }

        [BsonIgnore]
        [BsonElement("keyPhrases")]
        public string[] KeyPhrases { get; set; }
    }

    public class Body
    {
        [BsonElement("html")]
        public string Html { get; set; }

        [BsonElement("attachments")]
        public Attachment[] Attachments { get; set; }
    }

    public class Attachment
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("anchor")]
        public Anchor Anchor { get; set; }

        [BsonElement("htmlSnippet")]
        public HtmlSnippet HtmlSnippet { get; set; }
    }

    public class Anchor
    {
        [BsonElement("fragmentId")]
        public string FragmentId { get; set; }
    }

    public class HtmlSnippet
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

    public class Link
    {
        [BsonElement("href")]
        public string Href { get; set; }
    }

    public class ContentType
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }

    public class PressAgency
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("codeName")]
        public string CodeName { get; set; }
    }

    public class ShareableSummaryMultimediaContent
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

    public class SummaryImage
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

    public class ConcreteImage
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

    public class SubSector
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("region")]
        public object Region { get; set; }
    }
}