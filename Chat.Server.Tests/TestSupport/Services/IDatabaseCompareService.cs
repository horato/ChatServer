namespace Chat.Server.Tests.TestSupport.Services
{
    public interface IDatabaseCompareService
    {
        string CompareDatabasesAndGenerateChangescripts(string sourceDatabaseName, string targetDatabaseName);
    }
}