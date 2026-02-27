# Steam Profile Artwork Creator

A WPF Application for automating the process of creating steam profile artwork (artwork to be used as a showcase on your profile that combines your background with a overlay of a image/character)

---

## 📦 Requirements

- Windows OS
- .NET 6 or later
- Visual Studio 2022 or later (for building from source)

---

## ✏️ Usage

1. Click **Select** to choose whether you want to pick a **Background** or **Overlay**.
2. Browse and select your image file (.png, .jpg, .gif).
3. Preview images in the app.
4. Click **Crop** to generate two cropped sections for your Steam profile.
5. Cropped images are saved in the chosen directory as:
   - main_panel.png / main_panel.gif
   - right_panel.png / right_panel.gif

## 📥 Example Product

![example-ezgif com-resize](https://github.com/user-attachments/assets/62e053dd-8efd-4b79-ae0f-e9c2e29c3920)

---

## 🖼 Supported Formats

- Background: .png, .jpg, .gif  
- Overlay: .png, .jpg, .gif  
- Output: Matches the input format (GIF for animated, PNG/JPG for static)

---

## 🛠 Dependencies

- FFMpegCore – For image/video processing  
- WpfAnimatedGif – For GIF previews in WPF   
