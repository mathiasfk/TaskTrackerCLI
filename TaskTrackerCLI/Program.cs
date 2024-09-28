using System.Text.Json;
using TaskTrackerCLI;

const string CMD_ADD = "add";
const string CMD_UPDATE = "update";
const string CMD_DELETE = "delete";
const string CMD_MARK_IN_PROGRESS = "mark-in-progress";
const string CMD_MARK_DONE = "mark-done";
const string CMD_LIST = "list";

string filename = "task-tracket-state.json";

int maxID = 0;

// LOAD
List<TaskRecord> tasks;
if (File.Exists(filename))
{
    string jsonString = File.ReadAllText(filename);
    tasks = JsonSerializer.Deserialize<List<TaskRecord>>(jsonString);
    if (tasks.Count > 0)
    {
        maxID = tasks.Max(r => r.Id);
    }

}
else
{
    tasks = new List<TaskRecord>();
}

// PARSE
int id = 0;
string description = "";
string status = "";

switch (args[0])
{
    case CMD_ADD:
        description = args[1];
        id = maxID + 1;
        status = "new";
        break;
    case CMD_UPDATE:
        id = int.Parse(args[1]);
        description = args[2];
        break;
    default:
        Console.WriteLine("Invalid command");
        return;
}

// ACT
switch (args[0])
{
    case CMD_ADD:
        var newTask = new TaskRecord()
        {
            Id = id,
            Description = description,
            Status = status
        };
        tasks.Add(newTask);
        break;

    case CMD_UPDATE:
        var updatedTask = tasks.Where(r => r.Id == id).FirstOrDefault();
        updatedTask.Description = description;
        break;

    default:
        Console.WriteLine("Invalid command");
        break;
}


// FLUSH
string outputString = JsonSerializer.Serialize(tasks);
Console.WriteLine(outputString);
File.WriteAllText(filename, outputString);