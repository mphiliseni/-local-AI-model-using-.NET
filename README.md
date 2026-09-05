# LocalAi

LocalAi is a tiny, approachable .NET console app that demonstrates running a local AI model. It's meant to be simple, easy to explore, and a friendly starting point for experimenting with local inference.

## Quick Start

- Build:

	```bash
	dotnet build
	```

- Run from the `LocalAi` folder:

	```bash
	cd LocalAi
	dotnet run
	```

## Demo
<img width="1026" height="341" alt="Screenshot 2026-08-29 at 15 51 34" src="https://github.com/user-attachments/assets/ad7e42c7-2d67-44c6-a148-0a2d2da5b6af" />

## Web Version

This repository now includes a small web application (Razor Pages) under `Web/LocalAiChat` which provides a simple chat UI backed by the local AI model.

- Run the web app:

```bash
cd Web/LocalAiChat
dotnet run
```

- Open the app at: `http://localhost:5056` (default development URL, may vary)

You can add a screenshot for the web UI to the `images/` folder and reference it below. By convention place the screenshot at `images/chat-screenshot.png`.

![Chat screenshot](images/chat-screenshot.png)


## Contributing

Feel free to open issues or pull requests with improvements, questions, or ideas.

## Notes

This repository is a small learning project. Check the code to understand how the app is built and run.
