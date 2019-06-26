# SmallRedis

Redis is an in-memory NoSQL data store that supports operations or “commands” on data structures
such as sets, lists and hashes. The objective this application is to implement a service that supports a subset of the
Redis command set. A “mini redis”.

## Requirements for execution
 * .Net Core 2.1 (https://dotnet.microsoft.com/download/dotnet-core/2.1)

## Commands implemented

1) <a href="https://redis.io/commands/set">SET</a> key value
2) <a href="https://redis.io/commands/set">SET</a> key value EX seconds (need not implement other SET options)
3) <a href="https://redis.io/commands/get">GET</a> key
4) <a href="https://redis.io/commands/del">DEL</a> key
5) <a href="https://redis.io/commands/dbsize">DBSIZE</a>
6) <a href="https://redis.io/commands/incr">INCR</a> key
7) <a href="https://redis.io/commands/zadd">ZADD</a> key score member
8) <a href="https://redis.io/commands/zcard">ZCARD</a> key
9) <a href="https://redis.io/commands/zrank">ZRANK</a> key member
10) <a href="https://redis.io/commands/zrange">ZRANGE</a> key start stop

For close, run command:
```
exit
```

### How to execution

#### Command Line

 1) After project clone, go to folder of project and run command
 ```
 dotnet restore
 ```
 2) After run command:
 ```
 dotnet build
 ```
 3) Go to folder SmallRedis.App and execute the command:
 ```
 dotnet run
 ```
 
 #### Visual Studio
 
  1) After project clone, open project in Visual Studio and push F5. :-)
  
  ### Improvements for the future

* Implement docker image for execute application.
* Improvements in the logs and exceptions, send data for logstash, per example.
* Implement web service for to make available the functions.
* Implement load tests (Check performance with dependency injection).