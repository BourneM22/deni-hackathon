using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.Services
{
    public interface ISearchingService
    {
        Double ComputeCosineSimilarity(string text1, string text2);
    }

    public class SearchingService : ISearchingService
    {
        // Example stemming method (you could use a more robust library such as Snowball or PorterStemmer)
        private static readonly HashSet<string> StopWords = new HashSet<string>
        {
            // English stop words
            "the", "is", "in", "and", "to", "a", "an", "of", "for", "on", "with", "at", "by", "this", "that", "which", "who", "whom", "it", "its", "i", "you", "we", "he", "she", "they", "them", "are", "were", "was", "been", "being", "have", "has", "had", "having", "do", "does", "did", "doing", "but", "not", "can", "could", "will", "would", "should", "may", "might", "must", "shall", "please", "here", "there",

            // Indonesian stop words
            "yang", "di", "dan", "ke", "dengan", "untuk", "adalah", "pada", "ini", "itu", "sebuah", "mereka", "kami", "saya", "aku", "kamu", "dia", "ini", "tersebut", "oleh", "seperti", "pada", "atas", "sudah", "belum", "untuk", "dari", "apakah", "jika", "lebih", "dapat", "akan", "mungkin", "ada", "tidak", "bukan", "bisa", "saja", "melalui", "kami", "diri", "sendiri", "semua", "beberapa", "walaupun", "saat", "kerja", "jadi", "penuh", "selain", "segera", "tapi", "saya", "ingin", "tengah", "terus", "langsung", "harus", "meskipun", "dan", "ketika", "saat", "mungkin", "tentang", "hanya"
        };

        public Double ComputeCosineSimilarity(string text1, string text2)
        {
            // Tokenize the input strings into words
            var tokens1 = Tokenize(text1);
            var tokens2 = Tokenize(text2);

            // Create a union of unique tokens from both texts
            var allTokens = tokens1.Concat(tokens2).Distinct().ToList();

            // Create term frequency vectors for both texts
            var vector1 = CreateVector(allTokens, tokens1);
            var vector2 = CreateVector(allTokens, tokens2);

            // Use MathNet's LinearAlgebra library to calculate the cosine similarity
            var cosSim = CosineSimilarityMathNet(vector1, vector2);

            return cosSim;
        }

        // Tokenization: Split the string into words (simple whitespace splitting)
        private List<string> Tokenize(string text)
        {
            var normalizedText = text.ToLower();
            var tokens = normalizedText.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Where(token => !StopWords.Contains(token)); // Remove stop words
            return tokens.Select(token => Stemming(token)).ToList();
        }

        // Simple stemming (for demonstration, replace with a proper stemmer if needed)
        private string Stemming(string token)
        {
            // Example basic stemming: remove common suffixes
            if (token.EndsWith("ing")) return token.Substring(0, token.Length - 3);
            if (token.EndsWith("ed")) return token.Substring(0, token.Length - 2);
            return token;
        }

        // Levenshtein Distance to compare fuzzy matches
        private bool AreTokensSimilar(string token1, string token2, int threshold = 2)
        {
            var distance = LevenshteinDistance(token1, token2);
            return distance <= threshold; // Tokens are similar if the distance is small enough
        }

        // Calculate Levenshtein Distance
        private int LevenshteinDistance(string a, string b)
        {
            int[,] matrix = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; matrix[i, 0] = i++) ;
            for (int j = 0; j <= b.Length; matrix[0, j] = j++) ;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[a.Length, b.Length];
        }

        // Method to create a vector from the term frequency of words
        private Vector<double> CreateVector(List<string> allTokens, List<string> tokens)
        {
            var termFrequency = allTokens.Select(token => (double)
                tokens.Count(t => AreTokensSimilar(t, token)) // Compare tokens with fuzzy matching
            ).ToArray();

            return Vector<double>.Build.Dense(termFrequency);
        }

        // Calculate Cosine Similarity using MathNet.Numerics
        private Double CosineSimilarityMathNet(Vector<double> vector1, Vector<double> vector2)
        {
            var dotProduct = vector1.DotProduct(vector2);
            var magnitude1 = vector1.L2Norm();
            var magnitude2 = vector2.L2Norm();

            // Cosine similarity formula
            return dotProduct / (magnitude1 * magnitude2);
        }
    }
}
