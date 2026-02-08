namespace ITCompanionAI.Ingestion.Docs;





internal class FileHarvestPipeline
{


    public async Task RunAsync(string sourcePath)
    {
        var files = EnumerateFilePaths(sourcePath);

        foreach (var file in files)
        {
            var content = await File.ReadAllTextAsync(file);
            _ = new DocPage();
            SaveInSqlDatabase(file, content);
        }



        await Task.CompletedTask;
    }








    private void SaveInSqlDatabase(string file, string content)
    {
        throw new NotImplementedException();
    }








    /// <summary>
    /// </summary>
    /// <param name="rootPath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private IList<string> EnumerateFilePaths(string rootPath)
    {
        if (string.IsNullOrWhiteSpace(rootPath))
        {
            throw new ArgumentException("Root path cannot be null or empty.", nameof(rootPath));
        }

        if (!Directory.Exists(rootPath))
        {
            throw new DirectoryNotFoundException($"The directory '{rootPath}' does not exist.");
        }

        var filePaths = new List<string>();

        try
        {
            filePaths.AddRange(Directory.EnumerateFiles(rootPath, "*", SearchOption.AllDirectories));
        }
        catch (UnauthorizedAccessException ex)
        {
            // Log or handle the exception as needed
            throw new InvalidOperationException("Access to one or more directories was denied.", ex);
        }
        catch (IOException ex)
        {
            // Log or handle the exception as needed
            throw new InvalidOperationException("An I/O error occurred while enumerating files.", ex);
        }

        return filePaths;
    }
}