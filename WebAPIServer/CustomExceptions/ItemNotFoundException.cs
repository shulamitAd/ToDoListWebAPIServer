namespace WebAPIServer.CustomExceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(int id) : base($"Item with ID {id} not found.")
        {
        }
    }
}
