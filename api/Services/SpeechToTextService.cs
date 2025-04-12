using Vosk;
using System.Text.Json;
using Newtonsoft.Json;
using api.DTO;
using System.Diagnostics;

public interface ISpeechToTextService
{
    Task<SpeechToTextResponse> ProcessRawAudioStream(Stream audioStream);
    Task<SpeechToTextResponse> ProcessAudioFile(IFormFile file);
}

public class SpeechToTextService : ISpeechToTextService
{
    private readonly Model _model;

    public SpeechToTextService()
    {
        _model = new Model("models/vosk-model-small-en-us-0.15");
    }

    public async Task<SpeechToTextResponse> ProcessRawAudioStream(Stream audioStream)
    {
        try
        {
            var modelPath = "Dependencies/vosk-model-small-en-us-0.15";
            if (!Directory.Exists(modelPath))
            {
                throw new Exception($"Model path does not exist at: {modelPath}");
            }

            using var recognizer = new VoskRecognizer(new Model(modelPath), 16000.0f);
            recognizer.SetMaxAlternatives(0);
            recognizer.SetWords(true);

            var buffer = new byte[8192]; // Buffer size for larger chunks
            string finalResult = "";

            int bytesRead;
            while ((bytesRead = await audioStream.ReadAsync(buffer)) > 0)
            {
                if (recognizer.AcceptWaveform(buffer, bytesRead))
                {
                    var result = recognizer.Result();
                    // Append partial results
                    var resultJson = JsonConvert.DeserializeObject<dynamic>(result);
                    if (resultJson?.text != null)
                    {
                        finalResult += resultJson.text + " "; // Combine the recognized words
                    }
                }
            }

            // Append the final result after the stream ends
            var finalJson = recognizer.FinalResult();
            var finalResultJson = JsonConvert.DeserializeObject<dynamic>(finalJson);
            if (finalResultJson?.text != null)
            {
                finalResult += finalResultJson.text;
            }

            return new SpeechToTextResponse()
            {
                Text = finalResult.Trim()
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }

    public async Task<SpeechToTextResponse> ProcessAudioFile(IFormFile file)
    {
        try
        {
            var inputPath = Path.GetTempFileName(); // Temp file for uploaded audio
            var outputPath = Path.ChangeExtension(inputPath, ".pcm"); // Temp file for PCM conversion

            // Save the uploaded file to a temporary location
            using (var fileStream = new FileStream(inputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await file.CopyToAsync(fileStream); // Copy file to the stream
            }

            // Force garbage collection to release any locks
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Ensure that the file is not locked before proceeding
            if (IsFileLocked(inputPath))
            {
                throw new Exception("The file is locked by another process.");
            }

            // Run FFmpeg to convert the file to raw PCM
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{inputPath}\" -ac 1 -ar 16000 -f s16le -acodec pcm_s16le \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                process!.WaitForExit(); // Wait until FFmpeg finishes processing
            }

            if (!File.Exists(outputPath))
            {
                throw new Exception("Audio conversion failed. PCM file not created.");
            }

            // Open the PCM file and send it for transcription
            await using var audioStream = new FileStream(outputPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var result = await ProcessRawAudioStream(audioStream);

            // Clean up: Delete the temp files after processing
            File.Delete(inputPath);
            File.Delete(outputPath);

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"File transcription error: {ex.Message}");
        }
    }

    private bool IsFileLocked(string filePath)
    {
        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                // Attempt to open the file exclusively. If it succeeds, the file is not locked.
                return false;
            }
        }
        catch (IOException)
        {
            // If an IOException is thrown, it means the file is locked.
            return true;
        }
    }
}