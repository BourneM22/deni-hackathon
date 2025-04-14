using System.Speech.Synthesis;
using NAudio.Wave;
using NAudio.Lame;
using api.Models;

namespace api.Services
{
    public interface ITextToSpeechService
    {
        Audio TextToSpeech(String text);
        byte[] WavToMp3Converter(MemoryStream memoryStream);
    }

#pragma warning disable CA1416 // Validate platform compatibility
    public class TextToSpeechService : ITextToSpeechService
    {
        public Audio TextToSpeech(String text)
        {
            using SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            using MemoryStream memoryStream = new MemoryStream();

            synthesizer.SetOutputToWaveStream(memoryStream);

            synthesizer.Speak(text);
            memoryStream.Position = 0;

            byte[] mp3Bytes = WavToMp3Converter(memoryStream);

            return new Audio()
            {
                AudioBytes = mp3Bytes,
                FileName = Guid.NewGuid().ToString(),
                FileType = "audio/mpeg"
            };
        }
#pragma warning restore CA1416 // Validate platform compatibility

        public byte[] WavToMp3Converter(MemoryStream memoryStream)
        {
            using MemoryStream mp3Stream = new MemoryStream();
            using WaveFileReader wavReader = new WaveFileReader(memoryStream);
            using var writer = new LameMP3FileWriter(mp3Stream, wavReader.WaveFormat, LAMEPreset.ABR_64);

            wavReader.CopyTo(writer);
            writer.Flush();

            return mp3Stream.ToArray();
        }
    }
}