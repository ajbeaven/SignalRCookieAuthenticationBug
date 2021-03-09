# Repro: Bug with SignalR Cookie Authentication for issue [#30786](https://github.com/dotnet/aspnetcore/issues/30786)

To reproduce the issue:
1. Clone the project
2. Put a breakpoint in ChatHub
3. Set solution to "multiple startup projects" so both Chat and Web start when you begin debugging
4. Start debugging
5. Click button to log in as "MyUser"
6. Click button to send a message to the ChatHub via SignalR
7. Breakpoint should be hit. Step over to see user is not authenticated
