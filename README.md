## BIO DATA TIM ##

Nama Tim: JayaJayaJaya
Anggota:
- Derren Malaka
- Bourne Dhiaze Jo
- Hans Christian Arinardi

=======================================================================

## DESKRIPSI PROJEK ##

DENI (DEaf CompaNIon) adalah aplikasi pendamping cerdas yang dirancang khusus untuk membantu penyandang tuna rungu berkomunikasi dan beraktivitas dengan lebih mudah dan mandiri. Dengan berbagai fitur berbasis kecerdasan buatan, DENI hadir sebagai teman digital yang menjembatani kesenjangan komunikasi antara pengguna dan lingkungan sekitarnya.

Proyek ini mencakup aplikasi Android yang dikembangkan menggunakan Flutter untuk front-end, yang berkomunikasi dengan Backend API berbasis .NET 9. Backend API ini terhubung dengan model AI yang dihosting menggunakan Flask untuk pemrosesan percakapan dan pertanyaan dan juga database MySQL yang sudah ada secara online. Aplikasi ini menggunakan JWT untuk autentikasi dan menyediakan layanan percakapan berbasis AI yang responsif.

=======================================================================

## YANG HARUS DIINSTALL ##

Sebelum memulai, pastikan Anda sudah menyiapkan beberapa hal berikut:
- .NET 9 SDK untuk menjalankan Backend API.
- MySQL untuk database yang digunakan oleh Backend API (API yang ada sudah terhubung ke DB online).
- Python 3.8+, Flask, Llama 3.2 (ollama LLM model).
- Flutter SDK untuk membangun aplikasi Android.

=======================================================================

## CARA RUNNING SECARA BERURUTAN ##

- Python 3.8+: buka folder "ai_pipeline" dan install semua requirements di "requirements.txt" kemudian jalankan file "main.py" nanti akan muncul API dari flask. Pastikan "ollama" sudah terinstall dengan model "Llama3.2" dan "mxbai-embed-large"

- .NET 9: buka folder "api" di cmd kemudian masukan command "dotnet restore" setelah itu "dotnet watch run" nanti akan muncul swagger UI untuk melakukan testing API. Pastikan "ffmpeg" sudah terinstall untuk running API .NET 9

- Flutter: gunakan command "flutter run" untuk detail ada di README.md di folder "app"

======================================================================
