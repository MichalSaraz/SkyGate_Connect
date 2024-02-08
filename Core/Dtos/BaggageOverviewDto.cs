namespace Core.Dtos
{
    public class BaggageOverviewDto
    {
        public Guid Id { get; set; }
        public string TagNumber { get; set; }
        public int Weight { get; set; }
        public string FinalDestination { get; set; }
    }
}