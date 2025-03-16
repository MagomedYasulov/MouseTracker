
namespace MouseTracker.Domain.Data.Entites
{
    public class Position : BaseEntity
    {
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime MoveTime { get; set; }
    }
}
