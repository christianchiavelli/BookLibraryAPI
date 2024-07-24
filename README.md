# BookLibraryAPI

A RESTful API built with .NET 8 and Entity Framework Core to manage a book library system. This API includes functionalities for adding, retrieving, updating, and deleting book records. Swagger is used for API documentation.

## Table of Contents

- [About The Project](#about-the-project)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
- [Usage](#usage)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)
- [Acknowledgments](#acknowledgments)

## About The Project

This project is designed to manage a book library, allowing users to perform CRUD operations on book records.

## Getting Started
### Installation
1. Clone the repo:
   ```ssh
   git clone https://github.com/christianchiavelli/BookLibraryAPI.git
   ```
2. Navigate to the project directory:
   ```ssh
   cd BookLibraryAPI
   ```

3. Restore dependencies:
   ```ssh
   dotnet restore
   ```

4. Apply migrations to the database:
   ```ssh
   dotnet ef database update
   ```

5. Run the application:
   ```ssh
   dotnet run
   ```

### Prerequisites

- .NET SDK 8.0
- SQL Server (LocalDB or any SQL Server instance)

## Usage

To test the API, navigate to `https://localhost:44335/swagger/index.html` to access the Swagger UI.

## Roadmap

- Add user authentication
- Implement search functionality
- Add book borrowing feature
- Improve error handling

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

Christian Chiavelli - [christian.chiavelli@outlook.com](mailto:christian.chiavelli@outlook.com)

Project Link: [https://github.com/christianchiavelli/BookLibraryAPI](https://github.com/christianchiavelli/BookLibraryAPI)

## Acknowledgments

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Swagger](https://swagger.io/)
- [Visual Studio](https://visualstudio.microsoft.com/)