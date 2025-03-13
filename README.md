# Private Chat Application with SignalR, DynamoDB Local, and Redis Pub/Sub

This is a real-time private chat application using **SignalR** for communication, **Amazon DynamoDB Local** for offline message storage, and **Redis Pub/Sub** for real-time message delivery. Users can chat privately by entering a username and selecting a recipient, and the messages are exchanged in real-time. The backend is implemented using **ASP.NET Core** and **C#**.

## Features:

- **Username Authentication**: Users enter their username before starting the chat.
- **Recipient Selection**: After entering the username, users can choose a recipient to send private messages.
- **Real-time Messaging**: Utilizes SignalR for real-time messaging.
- **Offline Messages**: When a recipient is offline, messages are saved in DynamoDB Local for later delivery.
- **Message Display**: Differentiates between messages sent by the user (sender) and messages received (receiver).
- **Message History**: Retrieve offline messages when a user comes online.

## Technologies Used:

- **Backend**: ASP.NET Core, SignalR, Amazon DynamoDB Local, Redis (optional)
- **Frontend**: HTML, CSS, JavaScript
- **Real-time Communication**: SignalR for real-time updates between users
- **Offline Message Storage**: DynamoDB Local for storing offline messages when users are not connected

## Backend Setup (C# - ASP.NET Core)

### Prerequisites:

- .NET 6 or higher
- Visual Studio or Visual Studio Code (for development)
- **Docker** (for containerization)
- **DynamoDB Local** (for local development)
- **Redis** (for scaling)

## Steps to Set Up the Backend:

### 1. **Install Docker and Run Docker Compose**:

To start the necessary services (DynamoDB Local, Redis, and the backend) using Docker, follow these steps:

- Install Docker on your machine:

  - **For Windows and Mac**: [Install Docker Desktop](https://www.docker.com/products/docker-desktop/)
  - **For Linux**: [Install Docker Engine](https://docs.docker.com/engine/install/)

- Verify that Docker is running by running:

```bash
docker --version
```

- Once Docker is installed, run Docker Compose to start the services:

```bash
docker-compose up --build
```

This will start DynamoDB Local, Redis, and the backend on your local machine.

### 2. **Add Required Packages**:

If you're running the backend locally (not in Docker), make sure to install the necessary dependencies for the application:

- Navigate to the backend project directory and restore the packages from the `.csproj` file:

```bash
dotnet restore
```

Alternatively, if the dependencies are not added yet, you can manually add them:

```bash
dotnet add package Microsoft.AspNetCore.SignalR
dotnet add package Amazon.DynamoDBv2
dotnet add package StackExchange.Redis
dotnet add package AWSSDK.Extensions.NETCore.Setup
```

### 3. **Build the Application**:

```bash
dotnet build
```

### 4. **Run the Application**:

- After adding the required packages or restoring from the `.csproj` file, you can now run the application using the following command:

```bash
dotnet run
```

- The backend should be running locally at `http://localhost:5000`.

### 5. **Open the Application in Two Browser Tabs**:

- Open two tabs in Google Chrome (or any other browser).
- In both tabs, navigate to `http://localhost:5000/index.html`.
- In the first tab, enter a username and select a recipient to begin chatting.
- In the second tab, do the same, and you should be able to send and receive messages in real-time.

### 6. **Stop the Application and Docker Services**:

- When you are finished, stop the application by pressing Ctrl + C in the terminal where dotnet run is running.

- Also, bring down the Docker containers to stop DynamoDB Local and Redis by running:

```bash
docker-compose down
```
