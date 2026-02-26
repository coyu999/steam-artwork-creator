# Steam Profile Image Cropper

A WPF Application for automating the process of creating steam profile artwork (artwork to be used as a showcase on your profile that combines your background with a overlay of a image/character)

---

## 📦 Requirements

- Windows OS
- .NET 6 or later
- FFmpeg installed or included in the application folder
- Visual Studio 2022 or later (for building from source)

---

## 🚀 Installation

1. **Clone the repository**
   git clone https://github.com/coyu999/steam-artwork-creator.git
   cd steam-artwork-creator

2. **Open in Visual Studio**
   - Open steamprofile.slnx
   - Restore NuGet packages (FFMpegCore, WpfAnimatedGif)

3. **Build and run**
   - Press F5 to run the application

---

## 🎨 Usage

1. Click **Select** to choose whether you want to pick a **Background** or **Overlay**.
2. Browse and select your image file (.png, .jpg, .gif).
3. Preview images in the app.
4. Click **Crop** to generate two cropped sections for your Steam profile.
5. Cropped images are saved in the application directory as:
   - crop1.png / crop1.gif
   - crop2.png / crop2.gif

---

## 🖼 Supported Formats

- Background: .png, .jpg, .gif  
- Overlay: .png, .jpg, .gif  
- Output: Matches the input format (GIF for animated, PNG/JPG for static)

---

## 🛠 Dependencies

- FFMpegCore – For image/video processing  
- WpfAnimatedGif – For GIF previews in WPF   
