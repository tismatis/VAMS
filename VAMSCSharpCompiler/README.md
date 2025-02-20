# VAMSCSharpCompiler

VAMSCSharpCompiler is a compiler for the VAMS (Virtual Assembly Machine Sharp) project. It translates C# code into the custom intermediate language used by VAMS, enabling the execution of C# code on the VAMS virtual machine.

## Features

- Translates C# code to VAMS intermediate language
- Supports a subset of C# features
- Integrates with the VAMS virtual machine
- Extensible for additional C# features

**You should note that not every feature are supported using this compiler because of limitations from `VAMS` and because he is not finished.**

Like cast aren't supported, call "internal" method (compiled in `VAMS`) cannot currently take arguments, instantiation isn't implemented, we can't get returns value from methods calleds, arrays, fields, ext.
Missing features will be added in the future, you can contribute if you want too!

## Getting Started

To get started with the VAMSCSharpCompiler project, follow these steps:

1. Clone the repository.
2. Open the solution file `VAMSCSharpCompiler.sln` in Visual Studio or your preferred IDE.
3. Build the solution to restore the necessary packages and compile the projects.
4. Run the `VAMSCSharpCompiler` project to compile a c# class file into an `.vams` file inside a folder, `Compiled\`.
5. You can test your compiled script `.vams` using the `VAMSRunner`