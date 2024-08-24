
# Cachier

**Cachier** is a .NET-based application that leverages Redis for efficient caching and data management. It supports various Redis operations including strings, hashes, lists, sets, sorted sets, hyperloglogs, transactions, and pub/sub functionality.

## Features

- **String Operations**: Set, get, and delete string values.
- **Hash Operations**: Manage and retrieve hash fields.
- **List Operations**: Add, get, and remove list items.
- **Set Operations**: Add, get, and remove set members.
- **Sorted Set Operations**: Add, get, and remove items from sorted sets.
- **HyperLogLog Operations**: Add items and count unique items.
- **Transactions**: Execute multiple Redis commands in a transaction.
- **Pub/Sub**: Publish and subscribe to Redis channels.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Redis Server](https://redis.io/download)

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/nimanikoo/cachier.git
   cd cachier
   ```

2. **Install dependencies:**

   ```bash
   dotnet restore
   ```

3. **Configure your environment:**

   Update the `appsettings.json` file with your database and Redis connection strings:

   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=CashingApi_db;Trusted_Connection=True;TrustServerCertificate=True"
     },
     "RedisCacheUrl": "localhost"
   }
   ```

4. **Run the application:**

   ```bash
   dotnet run
   ```

## Usage

The application exposes several API endpoints to interact with the cache:

### String Operations

- **Set a String Value**

  ```http
  POST /cache/string
  Content-Type: application/json

  {
    "key": "myKey",
    "value": "myValue"
  }
  ```

- **Get a String Value**

  ```http
  GET /cache/string?key=myKey
  ```

- **Delete a String Value**

  ```http
  DELETE /cache/string?key=myKey
  ```

### Hash Operations

- **Set a Hash Field**

  ```http
  POST /cache/hash
  Content-Type: application/json

  {
    "key": "myHash",
    "field": "myField",
    "value": "myValue"
  }
  ```

- **Get a Hash Field**

  ```http
  GET /cache/hash?key=myHash&field=myField
  ```

- **Get All Hash Fields**

  ```http
  GET /cache/hash/all?key=myHash
  ```

- **Delete a Hash Field**

  ```http
  DELETE /cache/hash?key=myHash&field=myField
  ```

### List Operations

- **Add to List**

  ```http
  POST /cache/list
  Content-Type: application/json

  {
    "key": "myList",
    "value": "myValue"
  }
  ```

- **Get List Items**

  ```http
  GET /cache/list?key=myList
  ```

- **Remove from List**

  ```http
  DELETE /cache/list
  Content-Type: application/json

  {
    "key": "myList",
    "value": "myValue"
  }
  ```

### Set Operations

- **Add to Set**

  ```http
  POST /cache/set
  Content-Type: application/json

  {
    "key": "mySet",
    "value": "myValue"
  }
  ```

- **Get Set Members**

  ```http
  GET /cache/set?key=mySet
  ```

- **Remove from Set**

  ```http
  DELETE /cache/set
  Content-Type: application/json

  {
    "key": "mySet",
    "value": "myValue"
  }
  ```

### Sorted Set Operations

- **Add to Sorted Set**

  ```http
  POST /cache/sortedset
  Content-Type: application/json

  {
    "key": "mySortedSet",
    "value": "myValue",
    "score": 1
  }
  ```

- **Get Sorted Set Range**

  ```http
  GET /cache/sortedset?key=mySortedSet&start=0&end=10
  ```

- **Remove from Sorted Set**

  ```http
  DELETE /cache/sortedset
  Content-Type: application/json

  {
    "key": "mySortedSet",
    "value": "myValue"
  }
  ```

### HyperLogLog Operations

- **Add to HyperLogLog**

  ```http
  POST /cache/hyperloglog
  Content-Type: application/json

  {
    "key": "myHyperLogLog",
    "value": "myValue"
  }
  ```

- **Get HyperLogLog Count**

  ```http
  GET /cache/hyperloglog?key=myHyperLogLog
  ```

### Transactions

- **Execute a Redis Transaction**

  ```http
  POST /cache/transaction
  Content-Type: application/json

  {
    "commands": [
      {"type": "set", "key": "key1", "value": "value1"},
      {"type": "get", "key": "key1"}
    ]
  }
  ```

### Pub/Sub

- **Subscribe to a Channel**

  ```http
  POST /cache/subscribe
  Content-Type: application/json

  {
    "channel": "myChannel"
  }
  ```

- **Publish a Message**

  ```http
  POST /cache/publish
  Content-Type: application/json

  {
    "channel": "myChannel",
    "message": "Hello, world!"
  }
  ```

## Contributing

1. **Fork the repository**
2. **Create a new branch** (`git checkout -b feature/your-feature`)
3. **Commit your changes** (`git commit -am 'Add new feature'`)
4. **Push to the branch** (`git push origin feature/your-feature`)
5. **Create a Pull Request**

## License

This project is licensed under the [MIT License](LICENSE).

## Contact

For questions or suggestions, please open an issue on [GitHub](https://github.com/nimanikoo/cachier/issues) or email me at [nikoonazar.nima@gmail.com](mailto:nikoonazar.nima@gmail.com).
```

Feel free to further customize it as needed!
