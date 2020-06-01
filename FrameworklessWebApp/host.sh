dotnet test

dotnet publish -c publish
 
docker build -t server .
docker run -it --rm -p 8090:8080 --name server-container server
