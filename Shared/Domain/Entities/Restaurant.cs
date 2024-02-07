namespace Shared.Domain.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string SwitchT { get; set; }



        public Restaurant(int id, string switchT)
        {
            Id = id;
            SwitchT = switchT;
        }

        public static Restaurant Find(int restaurantId)
        {
            return new Restaurant(restaurantId, "EjemploSwitchT");
        }
    }
}
