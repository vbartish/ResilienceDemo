using System.Threading.Tasks;

namespace ResilienceDemo.Battery
{
    public interface IHowitzer
    {
        int Id { get; }
        
        bool IsOperational { get; }

        Task<Howitzer> ToArms();
    }
}