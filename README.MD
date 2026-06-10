# PUX

A small .NET application for directory snapshotting and diff calculations.

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

### POST /directory/diff

Send a directory path in the request body to schedule a diff calculation.

Example:

```bash
curl -X POST https://localhost:8080/directory/diff \
  -H "Content-Type: application/json" \
  -d '"/path/to/directory"' -i
```

The service returns the diff result once it is ready.

## Docker

```
docker run -d \
    --name api \
    --network=metlinskyi \
    -h api \
    -p 8081:8081 \
    -p 8082:8082 \
    metlinskyi/com:api-development
```
## Notes

- The main application project is located in `Src/pux.csproj`.
- The test project is located in `Tests/Tests.csproj`.
- Build artifacts are generated in the `bin/` and `obj/` folders.
