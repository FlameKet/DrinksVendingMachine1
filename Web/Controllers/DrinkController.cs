using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Web.Application;
using Web.Context;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/drinks")]
    [ApiController]
    public class DrinkController : ControllerBase
    {
        private readonly RepositoryDrink _repositoryDrink;

        public DrinkController(ApplicationContext db)
        {
            _repositoryDrink = new RepositoryDrink(db);
        }

        // GET: api/drinks
        [HttpGet]
        public IEnumerable<MachineStack<Drink>> Get()
        {
            return _repositoryDrink.Get().OrderBy(x => x.Entity.Name);
        }

        // GET api/drinks/5
        [HttpGet("{id}")]
        public MachineStack<Drink> Get(int id)
        {
            return _repositoryDrink.Get(id);
        }

        // POST api/drinks
        [HttpPost]
        public IActionResult Post([FromForm]MachineStackDrinkModel stackDrink)
        {
            if (stackDrink == null) throw new ArgumentNullException(nameof(stackDrink));

            _repositoryDrink.Add(new Drink(stackDrink.Name, stackDrink.Image, stackDrink.Volume, stackDrink.Price), stackDrink.Quantity);

           return Ok(Get());
        }

        // PUT api/drinks/5/17
        [HttpPut("{id}/{price}")]
        public ActionResult Put(int id, int price)
        {
            if (id <= 0)
                return BadRequest();

            var machineStackDrink = _repositoryDrink.Get(id);
            if (machineStackDrink==null) return NotFound();


            _repositoryDrink.ChangePrice(machineStackDrink.Entity, price);

            return Ok(machineStackDrink);
        }

        // DELETE api/drinks/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            var machineStackDrink = _repositoryDrink.Get(id);
            if (machineStackDrink == null) return NotFound();

            _repositoryDrink.Delete(machineStackDrink.Entity);

            return Ok();
        }

        // PUT api/drinks/5
        [HttpPut("{id}/giveOut")]
        public ActionResult GiveOut(int id)
        {
            if (id <= 0)
                return BadRequest();

            var machineStackDrink = _repositoryDrink.Get(id);
            if (machineStackDrink == null) return NotFound();


            _repositoryDrink.GiveOut(machineStackDrink.Entity, 1);

            return Ok(machineStackDrink);
        }
    }
}
