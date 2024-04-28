using static Security_App__Blazor.Pages.HashInvestigator;

namespace Security_App__Blazor.Data;

public interface IFileService
{
    Task<List<ScriptHash>> GetUnauthScripts();
    Task ApproveHash(string hash, string filePath);
}

public class FileService : IFileService
{

    private readonly string directoryPath = "API";

    public async Task<List<ScriptHash>> GetUnauthScripts()
    {
        var scriptList = new List<ScriptHash>();
        try
        {
            // Get all files in the directory that end with "_unauth_scripts.txt"
            var filePaths = Directory.GetFiles(directoryPath, "*_unauth_scripts.txt");
            foreach (var filePath in filePaths)
            {
                var subdomain = Path.GetFileNameWithoutExtension(filePath).Split('_')[0];
                var lines = await File.ReadAllLinesAsync(filePath);
                ScriptHash currentScript = null;
                // Iterate through each line in the file to parse hashes and contents
                foreach (var line in lines)
                {
                    if (line.StartsWith("Script hash:"))
                    {
                        if (currentScript != null)
                        {
                            scriptList.Add(currentScript); // Add the previous script to the list before starting a new one
                        }
                        currentScript = new ScriptHash
                        {
                            Hash = line.Replace("Script hash: ", "").Trim(),
                            Subdomain = subdomain,
                            FilePath = filePath
                        };
                    }
                    else if (currentScript != null)
                    {
                        // Add to the content of the current script
                        currentScript.Content += line + "\n";  // Ensure content accumulates with line breaks
                    }
                }
                if (currentScript != null)
                {
                    scriptList.Add(currentScript); // Add the last script to the list
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception details
            throw new InvalidOperationException("Failed to read unauthorized scripts.", ex);
        }
        return scriptList;
    }

    public async Task ApproveHash(string hash, string filePath)
    {
        try
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            string allowedFilePath = Path.Combine(directoryPath, Path.GetFileNameWithoutExtension(filePath).Split('_')[0] + ".txt");

            // Read the entire unauthorized script file
            var fileContent = await File.ReadAllLinesAsync(filePath);
            List<string> updatedContent = new List<string>();
            bool isCurrentScript = false;

            foreach (var line in fileContent)
            {
                if (line.Contains($"Script hash: {hash}"))
                {
                    // Found the hash, start skipping lines
                    isCurrentScript = true;
                    // Optionally, add the hash to the approved file immediately
                    await File.AppendAllTextAsync(allowedFilePath, hash + Environment.NewLine);
                    continue;
                }
                if (isCurrentScript)
                {
                    // Check if the next script hash starts or if it's the end of the content block
                    if (line.StartsWith("Script hash:"))
                    {
                        isCurrentScript = false;
                        updatedContent.Add(line); // This line is the start of the next script
                    }
                    // Otherwise, continue skipping the lines as part of the current script content
                }
                else
                {
                    // This line is not part of the script to remove
                    updatedContent.Add(line);
                }
            }

            // Write the updated content back to the unauthorized file
            await File.WriteAllLinesAsync(filePath, updatedContent);
        }
        catch (Exception ex)
        {
            // Log the exception details
            throw new InvalidOperationException("Failed to approve hash.", ex);
        }
    }

    public async Task<string> GetUrlFile(string fileName)
    {
        string fileContent = string.Empty;

        try
        {
            // Combine the directory path with the file name
            string fullFilePath = Path.Combine(directoryPath, fileName);

            // Check if the file exists
            if (File.Exists(fullFilePath))
            {
                // Read the entire file content
                fileContent = await File.ReadAllTextAsync(fullFilePath);
            }
            else
            {
                // File not found, handle accordingly
                Console.WriteLine($"File not found: {fullFilePath}");
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occurred
            Console.WriteLine($"Error reading URL file: {ex.Message}");
        }

        return fileContent;
    }
}



