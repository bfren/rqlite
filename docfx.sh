#!/bin/sh

# clean directory tree
find . -name 'bin' -type d -exec rm -rf {} \;
find . -name 'obj' -type d -exec rm -rf {} \;

# build Client package
dotnet build src/Rqlite.Client/Rqlite.Client.csproj \
    --configuration Debug

# build docfx image
docker buildx build \
    --load \
    -f docfx.Dockerfile \
    -t docfx \
    .

# run docfx to generate api documentation
docker run \
    -v ${PWD}:/repo \
    docfx \
    /repo/docs/docfx.json
