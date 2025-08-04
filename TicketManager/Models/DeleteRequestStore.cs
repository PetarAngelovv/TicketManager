namespace TicketManager.Web.Models
{
    public static class DeleteRequestStore
    {
        private static readonly HashSet<string> requestedUsers = new();
        private static readonly object lockObj = new();

        public static IEnumerable<string> RequestedUsers
        {
            get
            {
                lock (lockObj)
                {
                    return requestedUsers.ToList();
                }
            }
        }

        public static bool HasRequest(string username)
        {
            lock (lockObj)
            {
                return requestedUsers.Contains(username);
            }
        }

        public static void AddRequest(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return;

            lock (lockObj)
            {
                requestedUsers.Add(username);
            }
        }

        public static void RemoveRequest(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return;

            lock (lockObj)
            {
                requestedUsers.Remove(username);
            }
        }
    }
}
