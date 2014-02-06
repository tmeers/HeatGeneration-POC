namespace HeatGeneration.Models
{
    public class Racer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Finished { get; set; }

        public Racer() { }

        public Racer(int emptyId)
        {
            Id = -1;
            Name = "Empty";
        }
    }
}
