namespace Test_Taste_Console_Application.Constants
{
    public static class ExceptionMessage
    {
        public const string ApiKeyMissing =
            "No API key found. Set the environment variable SOLAR_SYSTEM_API_KEY or create appsettings.local.json.";

        public const string ApiKeyRejected =
            "The API rejected the API key (HTTP 401). Check that SOLAR_SYSTEM_API_KEY holds a valid key.";

        public const string FileOutputOperationFailed = "The file output operation failed with the following error: ";

        public const string ScreenOutputOperationFailed =
            "The screen output operation failed with the following error: ";
    }
}
