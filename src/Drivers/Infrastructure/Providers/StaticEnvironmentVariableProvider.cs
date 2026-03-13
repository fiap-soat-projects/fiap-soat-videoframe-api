namespace Infrastructure.Providers;

public static class StaticEnvironmentVariableProvider
{
    private const string MONGODB_CONNECTION_STRING = "VideoframeMongoDbConnectionString";
    private const string KAFKA_CONNECTION_STRING = "KafkaConnectionString";
    private const string KAFKA_PRODUCE_TOPIC_NAME = "KafkaProduceTopicName";
    private const string S3_BUCKET_BASE_URL = "S3BucketBaseUrl";
    private const string S3_BUCKET_USER = "S3BucketUser";
    private const string S3_BUCKET_PASSWORD = "S3BucketPassword";
    private const string S3_BUCKET_NAME = "S3BucketName";
    private const string COGNITO_REGION = "AWS_REGION";
    private const string COGNITO_USER_POOL_ID = "AWS_USER_POOL_ID";

    internal static readonly string VideoframeMongoDbConnectionString;
    internal static readonly string KafkaConnectionString;
    internal static readonly string KafkaProduceTopicName;
    internal static readonly string S3BucketBaseUrl;
    internal static readonly string S3BucketUser;
    internal static readonly string S3BucketPassword;
    internal static readonly string S3BucketName;
    public static readonly string CognitoRegion;
    public static readonly string CognitoUserPoolId;

    static StaticEnvironmentVariableProvider()
    {
        VideoframeMongoDbConnectionString = GetRequiredEnvironmentVariable(MONGODB_CONNECTION_STRING);
        KafkaConnectionString = GetRequiredEnvironmentVariable(KAFKA_CONNECTION_STRING);
        KafkaProduceTopicName = GetRequiredEnvironmentVariable(KAFKA_PRODUCE_TOPIC_NAME);
        S3BucketBaseUrl = GetRequiredEnvironmentVariable(S3_BUCKET_BASE_URL);
        S3BucketUser = GetRequiredEnvironmentVariable(S3_BUCKET_USER);
        S3BucketPassword = GetRequiredEnvironmentVariable(S3_BUCKET_PASSWORD);
        CognitoRegion = GetRequiredEnvironmentVariable(COGNITO_REGION);
        CognitoUserPoolId = GetRequiredEnvironmentVariable(COGNITO_USER_POOL_ID);
        S3BucketName = GetRequiredEnvironmentVariable(S3_BUCKET_NAME);
    }

    internal static void Init() { }

    private static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Environment variable '{variableName}' is not set.");
        }

        return value;
    }
}