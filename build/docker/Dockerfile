FROM mcr.microsoft.com/dotnet/aspnet:9.0.0-bookworm-slim
EXPOSE 5000
WORKDIR /app

# Copy the published output to the /app directory in the container
COPY publish /app

# Make sure the entry point executable is executable
RUN chmod +x ./Order.Presentation.Service

# Set the entrypoint to the executable for the self-contained app
ENTRYPOINT ["./Order.Presentation.Service"]