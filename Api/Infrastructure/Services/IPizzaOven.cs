using Api.Models;

namespace Api.Infrastructure.Services
{
    public interface IPizzaOven
    {
        void Bake(Pizza pizza);
    }
}
