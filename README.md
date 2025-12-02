📌 Window Controller Tool for Unity (Runtime WinAPI Window Manager)

Unity için geliştirilmiş gelişmiş bir pencere kontrol aracı.
Editör üzerinden pencere ayarlarını yapılandırabilir ve bu ayarları hem Player Settings olarak hem de Windows Build Runtime’ında otomatik olarak uygulayabilirsin.

Bu araç, özellikle overlay oyunlar, mini masaüstü uygulamaları, şeffaf HUD sistemleri, desktop companion projeleri ve windowed utility uygulamaları için idealdir.

🚀 Özellikler
🔹 Unity Player Settings Entegrasyonu

Tool üzerinden düzenlenen tüm ayarlar otomatik olarak Unity’nin Player Settings paneline yazılır:

Run In Background

Fullscreen Mode

Default Width / Height

Resizable Window

Visible In Background

Allow Fullscreen Switch

Use Player Log

Force Single Instance

DXGI Flip Model

🔧 WinAPI Runtime Window Control

Build çalıştığında pencere gerçek Windows API seviyesinde düzenlenir:

Pencere Modları

Borderless Window

Always On Top

Disable Dragging

Freeze / Unfreeze Window

Click-Through Mode

Opacity (0–1)

Color Key Transparency (PNG dışı her yer görünmez olur)

Window Shadow / Frameless Mode

Pozisyon Presetleri (9 bölge)

Pencere aşağıdaki presetlere göre otomatik konumlandırılır:

TopLeft / TopCenter / TopRight

MiddleLeft / MiddleCenter / MiddleRight

BottomLeft / BottomCenter / BottomRight

🎨 Color Key Transparency Örneği

Bu özellik sayesinde uygulamanın arka planını tamamen şeffaf yapabilir, yalnızca PNG görsellerinin görünmesini sağlayabilirsiniz (stream overlay veya masaüstü pet projeleri için ideal).

🧩 ScriptableObject Yapılandırma

Tüm ayarlar WindowSettings.asset içine kaydedilir ve build sırasında okunur.

Bu sayede:

Editor → Kaydet

Build → Otomatik uygula

Sürüm kontrolü ile paylaşılabilir

📄 Kullanım

Tools → Window Controller penceresini aç

Player Settings ve WinAPI ayarlarını yap

“Apply to Player Settings” butonuna bas

Build al → Ayarlar otomatik uygulanır

📦 Dosya Yapısı
Assets/
 ├── Scripts/
 │    ├── Window/
 │    │    ├── WindowAPI.cs
 │    │    ├── WindowRuntime.cs
 │    │    ├── WindowSettings.cs
 │    │    └── WindowControllerEditor.cs
 │
 └── Resources/
      └── WindowSettings.asset

🖼️ Screenshot

<img width="589" height="323" alt="image" src="https://github.com/user-attachments/assets/8385fee1-823e-43c2-8c48-e7bf2f5f1ffc" />


<img width="628" height="345" alt="image" src="https://github.com/user-attachments/assets/1ce647bd-80f0-43e7-bf01-afba2f13c340" />


📌 Desteklenen Platformlar

✔ Windows (tam WinAPI kontrolü)
✖ macOS (sınırlı)
✖ Linux (WinAPI yok)

🛠 Gereksinimler

Unity 6

Windows Standalone Player

.NET 4.x API Compatibility

🔥 Neden Bu Aracı Geliştirdim?

Unity’nin yerleşik pencere kontrolleri çok sınırlı.
Bu araç, özellikle masaüstü overlay uygulamalar, custom window tools, transparent HUD’lar, desktop mini-oyunlar gibi projeler için Windows API seviyesinde tam kontrol sağlar.

📜 Lisans

MIT License – Her projede kullanılabilir.
