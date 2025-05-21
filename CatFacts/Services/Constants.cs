namespace CatFacts.Services;

public static class Constants
{
    public const string DataBaseFileName = "CatFacts.db3";

    public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

    public static string DataBasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DataBaseFileName);
}
