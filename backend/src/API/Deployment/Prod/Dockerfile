FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Install the PostgreSQL client
RUN apt-get update \
    && apt-get install -y postgresql-client \
    && rm -rf /var/lib/apt/lists/*

# Set the working directory
WORKDIR /app

# Copy the application files
COPY app /app

# Expose the port
EXPOSE 5001

# Start the application
#ENTRYPOINT ["dotnet", "app/net6.0/API.dll"]
