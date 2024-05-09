# See https://github.com/nunit/docfx-action/blob/master/Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy

# Setting the path up to allow .NET tools
ENV PATH "$PATH:/root/.dotnet/tools"

# Install the docfx tool
RUN dotnet tool install --global docfx --version 2.76.0

# HACK: This effectively negates a git security patch that requires file ownership to match.
# Doing this because it does not appear that there is a standard way to address this in our container setup.
# A follow-up will likely be to take in a parameter and set the safe directory when that parameter is passed in.
RUN git config --system --add safe.directory '*'

ENTRYPOINT [ "docfx" ]
