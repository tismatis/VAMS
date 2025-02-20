# VAMS

VAMS (Virtual Assembly Machine Sharp) is a virtual machine system designed to execute a custom intermediate language. This project contains the core components for parsing, compiling, and executing the intermediate language.

## Features

- Custom intermediate language support
- Parser for the intermediate language
- Virtual Machine (VM) for executing the parsed code
- Support for asynchronous functions
- Extensible symbol system

## Supported Symbols

The following table lists the currently supported symbols in the VAMS project:
| Symbol           | Description                                                                 | Arguments                        |
|------------------|-----------------------------------------------------------------------------|----------------------------------|
| `ADD`            | Adds two values of the specified type.                                      | `Type of numeric` |
| `CALL_EXTERNAL`  | Calls an external method from a specified type.                             | `Path`, `Name`, `Should Push in stack (Optionnal)`, `List of Type of arguments (Optionnal)`             |
| `CALL_INTERNAL`  | Calls an internal method from the VM.                                       | `Path`, `Name`, `Should Push in stack (Optionnal)`             |
| `COME_DOWN`      | Switches the position of the top stack element with the specified index.    | `Index`                          |
| `CURTIME`        | Pushes the current Unix time in milliseconds onto the stack.                | None                             |
| `DECREMENT`      | Decrements the top value of the stack by 1.                                 | `Type of numeric`                             |
| `DIV`            | Divides two values of the specified type.                                   | `Type of numeric`               |
| `EQUALS`         | Compares the top two values of the stack for equality.                      | `Type for compare`                             |
| `INCREMENT`      | Increments the top value of the stack by 1.                                 | `Type of numeric`                             |
| `INSERT`         | Inserts a specified value onto the stack. (ChangeType)                                   | `Type`, `Value in Raw`                          |
| `INSERT_PARSE`   | Parses and inserts a specified value onto the stack. (Parse)                       | `Type`, `Value`                          |
| `JUMP`           | Jumps to a specified address in the function.                               | `Address`                        |
| `JUMP_IF_EQUALS` | Jumps to a specified address if the top two values of the stack are equal.  | `Address`, `Type`                        |
| `JUMP_IF_FALSE`  | Jumps to a specified address if the top value of the stack is false.        | `Address`                        |
| `JUMP_IF_NOT_EQUALS` | Jumps to a specified address if the top two values of the stack are not equal. | `Address`, `Type`                  |
| `JUMP_IF_TRUE`   | Jumps to a specified address if the top value of the stack is true.         | `Address`                        |
| `JUMP_INDIRECT`  | Jumps to the address specified by the top value of the stack.               | None                             |
| `MAIN_THREAD_TASK` | Enqueues a task to be executed on the main thread.                        | `Address of the main thread`                           |
| `MUL`            | Multiplies two values of the specified type.                                | `Type of numeric`               |
| `NOT`            | Negates the top value of the stack.                                         | None                             |
| `PEEK_INFO`      | Outputs the type and value of the top stack element.                        | None                             |
| `POP`            | Pops the top value from the stack.                                          | None                             |
| `POW`            | Raises a value to the power of another value.                               | `Type of numeric`               |
| `PRINT`          | Outputs the top value of the stack.                                         | None                             |
| `RETURN`         | Returns from the current function.                                          | None                             |
| `SUB`            | Subtracts two values of the specified type.                                 | `Type of numeric`               |
| `SWITCH`         | Switches the top two values of the stack.                                   | None                             |
| `TASK`           | Creates and starts a new task.                                              | `Address of the task`                           |
| `TOSTRING`       | Converts the top value of the stack to a string.                            | None                             |
| `WAIT_ALL_TASK`  | Waits for all tasks to complete.                                            | None                             |
| `WAIT_TASK`      | Waits for the top task on the stack to complete.                            | None                             |

## Example of `.vams` file

You can write your own program very easily:
1. Create your `program.vams`

2. Create your own class using `define class <Name>`, end it with an `define class_end`.
    * The `VAMS` language use these "balises" for split in parts the code, that permit to easily parse it quickly.

3. Create your method using `define method <Name>`, end it with an `define method_end`.
    * Dont forget to use `TAB`, this language is indentation sensitive
    * If you want wait an Task, mark your method as `async` like this: `define method async <Name>`.
    * You should see this
    ```vams
    define class Program
        define method Main
        define method_end
    define class_end
    ```
    * You can start adding `Symbols` to your methods to do your code doing his thing, you can check the symbol table.

4. For this example, we will create an simple "Hello World" app. We will start first to add to the stack* the `String` value and after we will print the value.
    * `VAMS` use a stack for store values, that permit to each symbol to do their action very quickly.
    * Use the `INSERT` symbol like this: `INSERT <TYPE> <VALUE>`, so in this situation, you will need to write `INSERT System.String "Hello World"`. You should note that because `VAMS` is running in a c# based runtime, he use c# types to work.
    * You can now use the Symbol `PRINT` that will take the top value in the Stack and print it.
    ```vams
    define class Program
        define method Main
            INSERT System.String "Hello World"
            PRINT
        define method_end
    define class_end
    ```

5. You should see now your first own Program but for test it, you need to move your program inside the same folder than an runner like `VAMSRunner` and run it to see the result!

## Getting Started

To get started with the VAMS project, follow these steps:

1. Clone the repository.
2. Open the solution file `VAMS.sln` in Visual Studio or your preferred IDE.
3. Build the solution to restore the necessary packages and compile the projects.
4. Run the `VAMSRunner` project to execute a sample program.