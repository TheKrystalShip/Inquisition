## Discord bot
Inquisition is a Discord bot written in c# using the Discord API Wrapper made by @RogueException, mostly for personal use and learning purposes, code is completly free for anyone to grab and use on anything they wish.

## Docker-compose

Use this command to deploy Inquisition and it's database container to Docker.

 - Inquisition is set to work with a Linux container (Ubuntu 16.04) that has the .Net Core runtime. 
  It's set to automatically create the database upon launch
 - The database is a SQL Server container also running on a Linux (Ubuntu 16.04) container.

```powershell 
docker-compose  -f "docker-compose.yml" -p dockercompose9695837040845040217 --no-ansi up -d --force-recreate --remove-orphans
```