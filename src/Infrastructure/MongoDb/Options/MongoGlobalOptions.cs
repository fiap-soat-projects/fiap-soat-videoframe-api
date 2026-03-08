using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.MongoDb.Options;

[ExcludeFromCodeCoverage]
internal static class MongoGlobalOptions
{
    const string GLOBAL_CONVENTIONS_NAME = "Global Conventions";

    private static readonly DateTimeSerializer _dateTimeSerializer = new(DateTimeKind.Local);

    private static readonly ConventionPack _conventions =
    [
        new CamelCaseElementNameConvention(),
        new EnumRepresentationConvention(BsonType.String),
        new IgnoreExtraElementsConvention(true),
        new IgnoreIfNullConvention(true)
    ];

    public static void Init()
    {
        ConventionRegistry.Register(
            GLOBAL_CONVENTIONS_NAME,
            _conventions,
            type => true);

        BsonSerializer.RegisterSerializer(
            typeof(DateTime),
            _dateTimeSerializer);
    }
}
