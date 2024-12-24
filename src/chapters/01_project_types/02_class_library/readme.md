### Class Libraries

#### Introduction
A Class Library is a project that contains reusable code, which can be shared across multiple applications. It is used to encapsulate logic that can be reused in different projects, promoting code reuse and modularity.

#### Use Cases
Class Libraries are commonly used for:
- **Shared Logic**: Encapsulating business logic, data access, or utility functions that can be used across multiple applications.
- **Modularity**: Breaking down a large application into smaller, manageable, and reusable components.
- **Third-Party Libraries**: Creating libraries that can be distributed and used by other developers.

#### Creating a Class Library

1. **Open the terminal in Visual Studio Code.**
2. **Navigate to the directory where you want to create the project**
3. **Create a new class library using the `dotnet` CLI:**

```sh
dotnet new classlib --name class_lib
```

This command creates a new directory named `class_lib` with the necessary files for a class library.

4. **Navigate to the project directory:**

```sh
cd class_lib
```

5. Optional: **Open the project in Visual Studio Code:**

```sh
code .
```

#### Understanding the Project Structure
- `Class1.cs`: This is a default class file created with the project. You can add your reusable code here.
- `class_lib.csproj`: This is the project file that contains information about the project and its dependencies.

#### Using the Class Library in Other Projects
To use the class library in other projects, you need to add a reference to it:
1. **Navigate to the project directory where you want to add the reference:**

```sh
cd somewhere/into/another/project
```

2. **Add a reference to the class library:**
```sh
dotnet add reference wherever/that/is/class_lib.csproj
```