# VAMS

VAMS (Virtual Assembly Machine Sharp) is a virtual machine system designed to execute a custom intermediate language, the `.vams`. This repository contains each big projects for do it run.
It can be used as an Sandbox for modding in c# applications like in Unity or Godot.

## Projects in this Repository

This repository contains three main projects:

### VAMS

The core of the project, it contains each Symbols supported in the language, an parser and a VM for execute it inside C#.
- [The link to VAMS project](VAMS/README.md)

### VAMSCSharpCompiler

A compiler that translates C# code into the custom intermediate language that VAMS can execute. It uses Roslyn to parse C# code and generate the corresponding intermediate language instructions.
- [The link to VAMSCSharpCompiler project](VAMSCSharpCompiler/README.md)

### VAMSRunner

A runner project that executes the compiled intermediate language using the VAMS virtual machine. It serves as an example of how to use the VAMS library to run programs.
It's a good example of how you can use it inside Unity or Godot. You should note that a library will be added for support this by default.
- [The link to VAMSRunner project](VAMSRunner/README.md)

## Getting Started

Depending on what you want to do, you can navigate to each project's directory.

## Contributing

Contributions are welcome! If you have any ideas, suggestions, or bug reports, please open an issue or submit a pull request.

## License

This project is licensed under the GPL-3.0. See the [LICENSE](../LICENSE) file for details.