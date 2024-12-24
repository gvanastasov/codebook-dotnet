### Console Applications

#### Introduction
A Console Application is a program that runs in a command-line interface or terminal. It is the simplest type of .NET application and is often used for utility programs or for learning purposes.

#### Use Cases
Console Applications are commonly used for:
- **Utility Programs**: Small tools that perform specific tasks, such as file manipulation, data processing, or system monitoring.
- **Prototyping**: Quickly testing and prototyping new features or algorithms.
- **Learning and Teaching**: Ideal for beginners to learn programming concepts without the complexity of a graphical user interface.
- **Automation Scripts**: Automating repetitive tasks or integrating with other systems through command-line scripts.

#### Creating a Console Application

1. **Open the terminal in Visual Studio Code.**
2. **Navigate to the directory where you want to create the project**
3. **Create a new console application using the `dotnet` CLI:**
    
```sh
dotnet new console -n console_app
```

This command creates a new directory named `console_app` with the necessary files for a console application.

4. **Navigate to the project directory:**

```sh
cd console_app
```

5. Optional: **Open the project in Visual Studio Code:**
```sh
code .
```

6. **Run the project**
```sh
dotnet run
```

#### Understanding the Template Project Structure
- `Program.cs`: This is the main file where the application starts executing. It contains the `Main` method, which is the entry point of the application (optionally top-level function is used, omitting the need of specifing Main function).
- `console_app.csproj`: This is the project file that contains information about the project and its dependencies.
