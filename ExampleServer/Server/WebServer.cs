// Define our actrual class that servers and handles web requests
//  Listen to HTTP requests, handle back and forth with our client
// Client being out web page (HTML, CSS, JS)
using System.Text.Json;
using System.Net;
using ExampleServer.Data;
using System.Text;
using ExampleServer.Models;

namespace ExampleServer.Server;

public class WebServer
{
    //private field
    private readonly TaskRepository _taskRepostiory;
    private readonly HttpListener _httpListener = new(); //networking parts

    public WebServer(TaskRepository repository, string url)
    {
        // Dependency Injection (injecting something we're dependent on)
        // Passing the TaskRepostiory into our class
        // rather than making a new repository
        _taskRepostiory = repository;

        _httpListener.Prefixes.Add(url);
    }

    //so we can do our server.run functionality
    public void Run()
    {
        // Start the server(http listener)
        _httpListener.Start();

        // Add some debug feedback (console writeline)
        Console.WriteLine($"Listening for connections on {_httpListener.Prefixes.First()}");
        // Handle our incoming connections/requests ( will handle the bulk of our logic)
        HandleIncomingRequests();
        // Stop the server
        _httpListener.Stop();
    }

    private void HandleIncomingRequests()
    {
        while (true)
        {
            // have the server sit and wait for a connection request
            // once there is a connetion request it will return the context
            HttpListenerContext context = _httpListener.GetContext();

            //Get the request and the responce objects from the context
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            switch (request.HttpMethod)
            {
                case "GET":
                    //handle the GET requests
                    HandleGetRequests(request, response);
                    break;
                case "POST":
                    // handle POST requests
                    HandlePostRequests(request, response);
                    break;
                case "PUT":
                    HandlePutRequests(request, response);
                    break;
                case "OPTIONS":
                    HandleOptionsRequests(response);
                    break;
                default:
                    SendResponse(response, HttpStatusCode.NotFound, null);
                    break;
            }


        }
    }

    private void HandleGetRequests(HttpListenerRequest request, HttpListenerResponse response)
    {
        if (request.Url?.AbsolutePath == "/")
        {
            var tasks = _taskRepostiory.GetTasks();
            SendResponse(response, HttpStatusCode.OK, tasks);
        }
        else
        {
            SendResponse(response, HttpStatusCode.NotFound, null);
        }

    }

    private void HandlePostRequests(HttpListenerRequest request, HttpListenerResponse response)
    {
        // Check that the request has a body
        if (request.HasEntityBody)
        {
            // Deserialize our request body into the C# request type
            TaskCreateRequest? body = JsonSerializer.Deserialize<TaskCreateRequest>(request.InputStream);

            // Check to make sure it is not null
            if (body != null)
            {
                // Create new TaskModel
                TaskModel newTask = new TaskModel(body.Title ?? "Title", body.Description ?? "Description");

                // Add that task to our repository
                _taskRepostiory.AddTask(newTask);

                // Create a response message
                string logOutput = $"You created a new task: #{newTask.Id}: {newTask.Title}";
                Console.WriteLine(logOutput);
                // Send that response
                SendResponse(response, HttpStatusCode.Created, newTask);
            }
        }
        else
        {
            // if our POST request doesn't have a body
            string errorMessage = "Failed to add task as there was no request body."; //string
            Console.WriteLine(errorMessage);

            ErrorResponse error = new ErrorResponse(errorMessage); // string passed into method
            SendResponse(response, HttpStatusCode.BadRequest, error);
        }
    }

    // Handle Update Requests
    private void HandlePutRequests(HttpListenerRequest req, HttpListenerResponse res)
    {
        if (req.HasEntityBody)
        {
            CompleteTaskRequest? body = JsonSerializer.Deserialize<CompleteTaskRequest>(req.InputStream);

            bool result = _taskRepostiory.MarkTaskAsComplete(body?.TaskId ?? 0);

            HttpStatusCode code = result ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            SendResponse(res, code, null);

        }
        else
        {
            string errorMessage = "Could not update task.";
            Console.WriteLine(errorMessage);
            ErrorResponse error = new ErrorResponse(errorMessage);
            SendResponse(res, HttpStatusCode.BadRequest, error);
        }
    }

    private void HandleOptionsRequests(HttpListenerResponse res)
    {
        res.AddHeader("Access-Control-Allow-Methods", "*");
        SendResponse(res, HttpStatusCode.OK, null);
    }



    //method
    private void SendResponse(HttpListenerResponse response, HttpStatusCode statusCode, object? data)
    {
        // convert our C# object to Json which allows our browser to understand it
        // We need to tell our response the content is JSON
        string payload = JsonSerializer.Serialize(data);
        response.ContentType = "Application/json";

        // Convert our JSON to a byte[] -> basic numbers we can send over the internet
        // Breaking down JSON to a steam of numbers
        byte[] buffer = Encoding.UTF8.GetBytes(payload);
        //We need to tell the response how much content to listen for
        // Tells the revipient (browser) how much of the data stream is the content
        response.ContentLength64 = buffer.Length;

        //Setting our response status code (ok, bad, good, etc.)
        // Casting our statusCode variable from type enum to type int
        response.StatusCode = (int)statusCode;

        // Simply here becasue CORS sucks
        response.AddHeader("Access-control-Allow-Origin", "*");

        // Writing/ sending our response
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.Close();
    }
}