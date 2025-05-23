#!/bin/bash

# Install .NET SDK (adjust version as required)
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update && \
sudo apt-get install -y apt-transport-https && \
sudo apt-get update && \
sudo apt-get install -y dotnet-sdk-8.0

# Optional: verify installation
dotnet --version

# Restore dependencies
dotnet restore

# Optional: Run a quick test pass
dotnet test --no-build --verbosity minimal
