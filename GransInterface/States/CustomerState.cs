namespace GransInterface.States
{
    [GenerateSerializer]
    public class CustomerState
    {
        [Id(0)]
        public int Id { get; set; }
        [Id(1)]
        public string Name { get; set; }
    }
}
