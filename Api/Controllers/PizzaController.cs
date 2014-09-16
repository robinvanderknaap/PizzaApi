using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Api.Infrastructure.Services;
using Api.Models;

namespace Api.Controllers
{
    [RoutePrefix("Pizza")]
    [Authorize]
    public class PizzaController : ApiController
    {
        private readonly IPizzaOven _pizzaOven;

        public PizzaController(IPizzaOven pizzaOven)
        {
            _pizzaOven = pizzaOven;
        }

        [Route("{pizzaId}")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetPizza(int pizzaId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Pizza{ Id = pizzaId, Name = "Calzone" });
        }

        [Route("")]
        [HttpPost]
        public HttpResponseMessage CreatePizza(Pizza pizza)
        {
            _pizzaOven.Bake(pizza);
            return Request.CreateResponse(HttpStatusCode.Created, pizza);
        }

        [Route("")]
        [HttpPut]
        public HttpResponseMessage UpdatePizza(Pizza pizza)
        {
            return Request.CreateResponse(HttpStatusCode.OK, pizza);
        }

        [Route("BadPizza")]
        [HttpGet]
        public HttpResponseMessage ThrowBadPizza()
        {
            throw new Exception("Bad pizza!");
        }

        [Route("UnknownPizza")]
        [HttpGet]
        public HttpResponseMessage UnknownPizza()
        {
            return Request.CreateResponse(HttpStatusCode.NotFound, "Unknown pizza");
        }

        [Route("{pizzaId}/ingredients/{ingredientId}")]
        [HttpGet]
        public HttpResponseMessage GetIngredient(int pizzaId, int ingredientId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Ansjovis");
        }
    }
}
