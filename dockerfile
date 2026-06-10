FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

RUN apk add --no-cache \
    clang \
    build-base \
    zlib-dev 


WORKDIR /src

# Copy project file and restore dependencies
COPY Src/pux.csproj .
RUN dotnet restore

# Copy remaining source code
COPY Src/ .


# Publish the application with AOT compilation
RUN dotnet publish pux.csproj -c Release -o /app/publish \
    --runtime linux-musl-x64 \
    --self-contained true 

# Runtime stage
FROM alpine:latest
WORKDIR /app

# Install dependencies required for .NET AOT binaries
RUN apk add --no-cache \
    libstdc++ \
    libgcc \
    libintl \
    icu-libs

# Copy the published application from build stage
COPY --from=build /app/publish .

# Create a non-root user to run the application
RUN addgroup -g 1000 appuser && \
    adduser -D -u 1000 -G appuser appuser && \
    chown -R appuser:appuser /app

USER appuser

ARG  environment=Production
# Set environment variables
#ENV ASPNETCORE_URLS=http://+:8081
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_ENVIRONMENT=$environment 

# Run the application
ENTRYPOINT ["./pux"]