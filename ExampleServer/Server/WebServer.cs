// Define our actrual class that servers and handles web requests
//  Listen to HTTP requests, handle back and forth with our client
// Client being out web page (HTML, CSS, JS)
using ExampleServer.Data;

namespace ExampleServer.Server;

public class WebServer
{
private readonly TaskRepository _taskRepostiory;

public WebServer(TaskRepository repository)
{
    // Dependency Injection (injecting something we're dependent on)
    // Passing the TaskRepostiory into our class
    // rather than making a new repository
    _taskRepostiory = repository;
}


}