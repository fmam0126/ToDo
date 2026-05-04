# Todo

This project is a simple Todo API built to practice and demonstrate Test Driven Development (TDD) using .NET 10.

eatures
- RESTful API for managing todo items
- Add, retrieve, and list todos
- Comprehensive test suite using xUnit and ASP.NET Core testing tools

## Getting Started

### Prerequisites
- .NET 10 SDK

### Running the Application
1. Clone the repository:
   ```sh
   git clone https://github.com/fmam0126/ToDo.git
   cd ToDo
   ```
2. Build and run the API:
   ```sh
   dotnet run --project Todo
   ```

### Running the Tests
To run the test suite:
```sh
dotnet test
```

## Project Structure
- `Todo.Api/` - Main API project
- `Todo.Tests/` - Test project containing all TDD test cases

## Learning TDD
This project encourages the following TDD workflow:
1. Write a failing test for a new feature or bug.
2. Write the minimal code to make the test pass.
3. Refactor the code, keeping all tests green.


## License
This project is for educational purposes.