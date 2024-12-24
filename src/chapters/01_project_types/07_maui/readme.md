### .NET MAUI (Cross-platform)

#### Introduction
.NET MAUI (Multi-platform App UI) is a framework for building cross-platform applications with a single codebase. It allows you to create applications that run on Android, iOS, macOS, and Windows.

#### Use Cases
.NET MAUI is commonly used for:
- **Cross-Platform Applications**: Building applications that run on multiple platforms with a single codebase.
- **Mobile Applications**: Developing mobile apps for Android and iOS.
- **Desktop Applications**: Creating desktop apps for Windows and macOS.
- **Shared Codebase**: Sharing code and UI components across different platforms.

#### Installing .NET MAUI Workload

1. **Open the terminal in Visual Studio Code.**
2. **Install the .NET MAUI workload:**
```sh
dotnet workload install maui
```

3. **Verify the installation:**
```sh
dotnet new --list
```
You should see `.NET MAUI App` listed among the available templates.

#### Creating a .NET MAUI Application

1. **Open the terminal in Visual Studio Code.**
2. **Navigate to the directory where you want to create the project**
3. **Create a new .NET MAUI application using the `dotnet` CLI:**
```sh
dotnet new maui -n MyMauiApp
```
This command creates a new directory named `MyMauiApp` with the necessary files for a .NET MAUI application.

4. **Navigate to the project directory:**
```sh
cd MyMauiApp
```

5. Optional: **Open the project in Visual Studio Code:**
```sh
code .
```

#### Understanding the Project Structure
- `Platforms`: This directory contains platform-specific code for Android, iOS, macOS, and Windows.
- `Resources`: This directory contains shared resources such as images, fonts, and styles.
- `App.xaml`: This file contains the application-level XAML markup.
- `App.xaml.cs`: This file contains the application-level code.
- `MainPage.xaml`: This file contains the XAML markup for the main page of the application.
- `MainPage.xaml.cs`: This file contains the code-behind for the main page.
- `MyMauiApp.csproj`: This is the project file that contains information about the project and its dependencies.

#### Running the .NET MAUI Application
To run the application, use the following command in the terminal:
```sh
dotnet build
dotnet run -f net9.0-windows10.0.19041.0
```

> Replace net9.0-windows10.0.19041.0 with the appropriate target framework for your platform (e.g., net6.0-ios, net6.0-maccatalyst, net6.0-android)