using System.Diagnostics;

namespace ExcelGenerator.Api.Services;

public class PdfGeneratorService
{
    public string ConvertExcelToPdf(string excelPath)
    {
        var outputDir = Path.GetDirectoryName(excelPath)!;

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\LibreOffice\program\soffice.exe",
                Arguments = $"--headless --convert-to pdf \"{excelPath}\" --outdir \"{outputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        var pdfPath = Path.ChangeExtension(excelPath, ".pdf");

        if (!File.Exists(pdfPath))
            throw new Exception("Falha ao converter Excel para PDF.");

        return pdfPath;
    }
}
