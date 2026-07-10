# PUX


[![How it works](https://github.com/metlinskyi/pux/raw/refs/heads/main/screenshot.png)](https://github.com/metlinskyi/pux/raw/refs/heads/main/demo.webm)


## Requirements

- .NET 10 SDK

## Run the application

```bash
dotnet run --project Src/pux.csproj
```

## Run tests

```bash
dotnet test Tests/Tests.csproj
```

## API endpoints

The application exposes a minimal API for directory diff operations.

### POST /directory/diff/async

Send a directory path in the request body to schedule a diff calculation.

Example:

```bash
curl -X POST http://localhost:8080/directory/diff/async \
  -H "Content-Type: application/json" \
  -d '{"Path":"/Web"}' -i
```

The service returns the diff result once it is ready.

## Docker

```bash
docker build --pull --rm -f "dockerfile" -t pux "."
```
```bash
docker run -d -p 8080:8080 -v ./Web/:/Web/:ro pux:latest 
```
## Notes

- The main application project is located in `Src/pux.csproj`.
- The test project is located in `Tests/Tests.csproj`.
- Build artifacts are generated in the `bin/` and `obj/` folders.
