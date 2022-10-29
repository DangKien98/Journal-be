# Back-End
## Run project bằng docker (linux)
Cài docker trên máy trước

trên terminal run
```
docker build -t journal-backend -f Journal-be/Dockerfile . 
```
run tiếp
```
docker run -d -p 31081:31081 journal-backend
```
## Run project bằng cmd
Cài SDK .Net 6 :
https://dotnet.microsoft.com/en-us/download/dotnet/6.0

mở cmd lên run
```
cd Journal-be
```

sau đó run
```
dotnet run
```
