using Api.Models;

namespace Api.Infrastructure.Services
{
    public class PizzaOven : IPizzaOven
    {
        public void Bake(Pizza pizza)
        {
            pizza.Baked = true;
        }
    }
}