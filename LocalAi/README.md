# Local AI with .NET

This project demonstrates how to integrate a local AI model into .NET applications using Ollama and the Microsoft.Extensions.AI abstractions.

The solution includes two sample apps:

- A console application that chats with a local model from the terminal
- A web application that exposes the same AI capability through a browser UI and a streaming API

The main goal is to show how a local LLM can be used in .NET without depending on external cloud AI services.

## Project purpose

The project is designed to help developers:

- run AI models locally on their machine
- connect .NET applications to those models using Ollama
- prototype chat experiences in both console and web apps
- learn how to build lightweight AI-powered apps with local inference

This is useful for offline development, privacy-sensitive scenarios, low-cost experimentation, and local AI workflows.

## Architecture overview

### 1. Console app

Location: `local Ai/`

This app runs a simple chat loop in the terminal:

- prompts the user for input
- sends the message to the local model
- streams the response back to the console
- keeps a short in-memory chat history

It uses:

- `Microsoft.Extensions.AI`
- `OllamaSharp`
- `OllamaApiClient(new Uri("http://localhost:11434"), "phi3:latest")`

### 2. Web app

Location: `Web /LocalAiChat/`

This app adds a browser-based interface and API layer:

- Razor Pages UI
- controller-based chat API
- streaming response support using SSE
- chat service wrapping the same local Ollama model

It uses the same local model endpoint and allows users to interact with the AI through a simple web chat interface.

## Local AI model setup

This project expects Ollama to be installed and running locally.

### Install Ollama

Follow the official instructions for your OS:

- https://ollama.com/download

### Pull a model

After installation, run:

```bash
ollama pull phi3:latest
```

Then ensure Ollama is running on:

```text
http://localhost:11434
```

## Run the console app

From the project root:

```bash
dotnet run --project "local Ai/local Ai.csproj"
```

The app will prompt you for text and stream the AI response in the console.

## Run the web app

From the project root:

```bash
dotnet run --project "Web /LocalAiChat/LocalAiChat.csproj"
```

Then open the local URL shown in the terminal, usually:

```text
https://localhost:PORT
```

or if configured for HTTP:

```text
http://localhost:PORT
```

## How the code connects to the local model

The integration point is the Ollama client setup.

Example pattern used in the project:

```csharp
var chatClient = new OllamaApiClient(
    new Uri("http://localhost:11434"),
    "phi3:latest");
```

This allows the .NET app to send prompts to a local model and stream responses back as text.

## Tech stack

- .NET 9
- ASP.NET Core
- Razor Pages
- Microsoft.Extensions.AI
- OllamaSharp
- Local LLM inference through Ollama

## Notes

- This project is intended for local experimentation and learning.
- It does not require cloud AI credentials.
- The model runs on your local machine through Ollama.
- The web app demonstrates a practical integration pattern for chat-based interfaces.

## Future improvements

Possible enhancements include:

- adding conversation memory controls
- supporting multiple local models
- adding authentication or user sessions
- improving UI styling and responsiveness
- adding document upload and RAG use cases

## Summary

This repository shows how to integrate a local AI model into .NET in both a terminal-based and web-based experience. It is a practical sample for developers who want to build AI-enabled applications without sending prompts to external cloud services.
