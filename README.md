# TaskTrackerCLI

Basic task tracker CLI project, inspired by https://roadmap.sh/projects/task-tracker.

Here are some usage examples:

```sh
# Adding a new task
TaskTrackerCLI.exe add "Buy groceries"
# Output: Task added successfully (ID: 1)

# Updating and deleting tasks
TaskTrackerCLI.exe update 1 "Buy groceries and cook dinner"
TaskTrackerCLI.exe delete 1

# Marking a task as in progress or done
TaskTrackerCLI.exe mark-in-progress 1
TaskTrackerCLI.exe mark-done 1

# Listing all tasks
TaskTrackerCLI.exe list

# Listing tasks by status
TaskTrackerCLI.exe list done
TaskTrackerCLI.exe list todo
TaskTrackerCLI.exe list in-progress
```