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
        // try
        // {
            var tempId = Guid.NewGuid().ToString();
            var inputPath = Path.Combine(Path.GetTempPath(), $"{tempId}.input");
            var outputPath = Path.Combine(Path.GetTempPath(), $"{tempId}.pcm");

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

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Important: read these to avoid deadlocks and ensure FFmpeg flushes all its output
                var stdOutTask = process.StandardOutput.ReadToEndAsync();
                var stdErrTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(stdOutTask, stdErrTask); // Wait for both to finish

                await process.WaitForExitAsync(); // Make sure the process ends fully

                if (process.ExitCode != 0)
                {
                    throw new Exception($"FFmpeg failed (Exit Code {process.ExitCode}): {stdErrTask.Result}");
                }
            }

            if (!File.Exists(outputPath))
            {
                throw new Exception("Audio conversion failed. PCM file not created.");
            }

            // Open the PCM file and send it for transcription
            SpeechToTextResponse result;

            try
            {
                await using (var audioStream = new FileStream(outputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    result = await ProcessRawAudioStream(audioStream);
                }
            }
            finally
            {
                if (File.Exists(inputPath)) File.Delete(inputPath);
                if (File.Exists(outputPath)) File.Delete(outputPath);
            }

            return result;
        // }
        // catch (Exception ex)
        // {
        //     throw new Exception($"File transcription error: {ex.Message}");
        // }
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