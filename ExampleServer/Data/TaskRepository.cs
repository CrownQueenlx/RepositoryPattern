/* TaskRepository is responsible for storing and manipulating
our collection of data, in this case TaskModels
*/
namespace ExampleServer.Data;

public class TaskRepository
{
    //define our collection of our fake database connection
    // Data storage (All of our Tasks)
    private readonly List<TaskModel> _taskList = new List<TaskModel>();

    // Create method (does not initiate objects (instances), it is just a reachthough entry, it is a new entry in (manipulation to) the list)
    // access modifier, return type, methodName, (type, parameterName)
    public void AddTask(TaskModel task)
    {
        _taskList.Add(task);
        // _taskList.Contains(task); //retruns true false about the parameter in the list
    }

    // Read method, communicates with the data storage
    public List<TaskModel> GetTasks() //no parameter lets us get all our tasks
    {
        // return _tasklist;
        return new List<TaskModel>(_taskList);
    }

    public List<TaskModel> GetTasksByStatus(bool IsComplete)
    {
        // Start a new list
        // local variable doesn't need access modifier
        List<TaskModel> tasks = new List<TaskModel>();

        // Iterate though all tasks and check the status 
        foreach (TaskModel task in _taskList)
        {
            // Add a task to the new List if its status matches the parameter
            if (task.IsComplete == IsComplete)
            {
                tasks.Add(task);
            }
        }


        // Retrun the new list
        return tasks;

    }

    // Update method
    public bool MarkTaskAsComplete(int taskId)
    {
        // Returns the first element of the sequence that satifies
        // a condition or a default value if no such eleemnt is found.
        TaskModel? task = _taskList.FirstOrDefault(tM => tM.Id ==taskId);

        // check if the task doesn't exist OR
        //  check if the task is already complete
        if (task == null || task.IsComplete)
        {
            // return false to indicate it didn't change anything
            return false;
        }
        task.IsComplete = true;
        return true;
    }

    // Delete method
    public bool DeleteTaskById(int id)
    {   
        //loop though each task
        foreach (var task in _taskList)
        {   
            //Check the task Id against our parameter
            if (task.Id == id)
            {   
                //It we find it remove the task and retun true/false
                return _taskList.Remove(task);
            }
        }
       
        // Retrun false if we don't find the Id in the loop
        return false;
    }
}