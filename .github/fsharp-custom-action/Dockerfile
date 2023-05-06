# ========================================================
# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /proj

COPY src/ ./

# Restore + build + publish the project
RUN dotnet publish -c Release -o out

# ========================================================
# Runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0

WORKDIR /proj

COPY --from=build-env /proj/out .

ENTRYPOINT ["/proj/main"]
