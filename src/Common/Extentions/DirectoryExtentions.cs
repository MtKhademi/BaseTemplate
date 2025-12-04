namespace Infrastructure.Extentions;

public static class DirectoryExtentions
{
    public static string CreateDirectoryIfNotExist(this string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }


    public static string DeleteFileIfExist(this string pathFile)
    {
        if (File.Exists(pathFile))
            File.Delete(pathFile);

        return pathFile;
    }
}
