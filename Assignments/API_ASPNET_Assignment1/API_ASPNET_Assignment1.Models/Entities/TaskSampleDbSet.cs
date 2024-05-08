namespace API_ASPNET_Assignment1.Models.Entities
{
    public static class TaskSampleDbSet
    {
        public static List<TaskModel> taskModels = GenerateTaskModels(10);
        public static List<TaskModel> GenerateTaskModels(int count) {
            List<TaskModel> tasks = new List<TaskModel>();

            // Generate some sample tasks
            for (int i = 0; i < count; i++)
            {
                tasks.Add(new TaskModel
                {
                    Id = Guid.NewGuid(),
                    Title = $"Task {i + 1}",
                    IsCompleted = i % 2 == 0 // Alternate between true and false for IsCompleted
                });
            }

            return tasks;
        }
    }
}
