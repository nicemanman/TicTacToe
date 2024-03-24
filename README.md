```markdown
# TicTacToe Project

This is the TicTacToe project, an implementation using ASP.NET Core for educational and development purposes. Follow the instructions below to set up the project in your development environment with or without Docker.

## Getting Started

These instructions will get your copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- Git
- .NET Core SDK
- PostgreSQL
- Docker (optional, for containerized environments)

### Installation without Docker

1. **Clone the Repository**

   ```bash
   git clone https://github.com/nicemanman/TicTacToe
   cd TicTacToe
   ```

2. **Set Up the Database**

   Install PostgreSQL and create a database with the following credentials:

   - Host: localhost
   - Port: 5432
   - Username: postgres
   - Password: postgres

   Ensure that PostgreSQL is running on your system.

3. **Start the Backend Server**

   Navigate to the Server directory:

   ```bash
   cd Server
   dotnet build
   dotnet run
   ```

   The backend server will be available at `localhost:5000`.

4. **Start the User Interface**

   Open a new terminal and navigate to the UserInterface directory:

   ```bash
   cd UserInterface
   dotnet build
   dotnet run
   ```

   The User Interface will be accessible at `localhost:5001`.

### Installation with Docker

1. **Start Development Environment**

   To start the environment with open server ports for debugging:

   ```bash
   docker-compose -f docker-compose.Development.yml up
   ```

   This will start both backend and frontend services with the backend server accessible for debugging.

2. **Start Production Environment**

   To deploy the environment without exposing the backend port:

   ```bash
   docker-compose -f docker-compose.Production.yml up
   ```

   The User Interface will still be accessible at `localhost:5001`.

### Postman Collection

For API testing, you can import the following Postman collection into your Postman application:

```json
{
	"info": {
		"_postman_id": "ffe300bd-58fc-47a1-b4a7-d533aaf9c677",
		"name": "TicTacToe",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
		"_exporter_id": "11353334"
	},
	"item": [
		{
			"name": "Start new game",
			"request": {
				"method": "POST",
				"header": [],
				"url": {
					"raw": "http://localhost:5000/api/game",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5000",
					"path": [
						"api",
						"game"
					]
				}
			},
			"response": []
		},
		{
			"name": "Get game state",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "http://localhost:5000/api/game",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5000",
					"path": [
						"api",
						"game"
					]
				}
			},
			"response": []
		},
		{
			"name": "Make a move",
			"request": {
				"method": "PATCH",
				"header": [],
				"url": {
					"raw": "http://localhost:5000/api/game?row=1&column=2",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5000",
					"path": [
						"api",
						"game"
					],
					"query": [
						{
							"key": "row",
							"value": "1"
						},
						{
							"key": "column",
							"value": "2"
						}
					]
				}
			},
			"response": []
		}
	]
}
```
