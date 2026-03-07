namespace Infrastructure.Providers;

public static class StaticEnvironmentVariableProvider
{
    private const string MONGODB_CONNECTION_STRING = "MongoDbConnectionString";
    private const string KAFKA_CONNECTION_STRING = "KafkaConnectionString";
    private const string S3_BUCKET_BASE_URL = "S3BucketBaseUrl";
    private const string S3_BUCKET_USER = "S3BucketUser";
    private const string S3_BUCKET_PASSWORD = "S3BucketPassword";
    private const string COGNITO_REGION = "CognitoRegion";
    private const string COGNITO_USER_POOL_ID = "CognitoUserPoolId";
    private const string COGNITO_CLIENT_ID = "CognitoUserClientId";

    internal static readonly string MongoDbConnectionString;
    internal static readonly string KafkaConnectionString;
    internal static readonly string S3BucketBaseUrl;
    internal static readonly string S3BucketUser;
    internal static readonly string S3BucketPassword;
    public static readonly string CognitoRegion;
    public static readonly string CognitoUserPoolId;
    public static readonly string CognitoClientId;

    static StaticEnvironmentVariableProvider()
    {
        MongoDbConnectionString = GetRequiredEnvironmentVariable(MONGODB_CONNECTION_STRING);
        KafkaConnectionString = GetRequiredEnvironmentVariable(KAFKA_CONNECTION_STRING);
        S3BucketBaseUrl = GetRequiredEnvironmentVariable(S3_BUCKET_BASE_URL);
        S3BucketUser = GetRequiredEnvironmentVariable(S3_BUCKET_USER);
        S3BucketPassword = GetRequiredEnvironmentVariable(S3_BUCKET_PASSWORD);
        CognitoRegion = GetRequiredEnvironmentVariable(COGNITO_REGION);
        CognitoUserPoolId = GetRequiredEnvironmentVariable(COGNITO_USER_POOL_ID);
        CognitoClientId = GetRequiredEnvironmentVariable(COGNITO_CLIENT_ID);
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