# LocalDrop

Telefondan bilgisayara kabloyla dosya atmaktan veya kendime mail/WhatsApp mesajı atmaktan sıkıldığım için yaptığım çok basit bir dosya transfer aracı.

Mantığı çok basit: Bilgisayarda çalıştırıyorsunuz, size bir QR kod veriyor. Telefonla aynı Wi-Fi ağındayken o QR kodu okutuyorsunuz ve telefondan seçtiğiniz dosyalar direkt bilgisayardaki `Downloads` klasörüne iniyor.

## Kullanılan Araçlar
- C# & ASP.NET Core Minimal API
- QR kod üretimi için `QRCoder` kütüphanesi
- Arayüz için saf HTML/CSS/JS

## Nasıl Çalıştırılır?

Bilgisayarınızda .NET yüklü olması gerekiyor.

1. Proje klasöründe terminali açıp şu komutu çalıştırın:
   ```bash
   dotnet run
   ```
2. Konsolda çıkan linki (genelde `http://localhost:5000/dashboard.html`) tarayıcınızda açın.
3. Ekranda beliren QR kodu telefonunuzun kamerasına okutun.
4. Telefondan dosyalarınızı seçip gönderin. 

İnen dosyalar proje klasöründeki `Downloads` klasörüne kaydedilir.

---
Kendi işimi görsün diye yazdım, ufak tefek eksikleri olabilir ama işimi fazlasıyla görüyor. İsteyen alıp kullanabilir.
