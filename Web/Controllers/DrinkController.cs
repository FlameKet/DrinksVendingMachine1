using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Web.Application;
using Web.Context;
using Web.Models;
using AutoMapper;

namespace Web.Controllers
{
    [Route("api/drinks")]
    [ApiController]
    public class DrinkController : ControllerBase
    {
        private readonly RepositoryDrink _repositoryDrink;
        private readonly IMapper _mapper;

        public DrinkController(IApplicationContext db, IMapper mapper)
        {
            _repositoryDrink = new RepositoryDrink(db);
            _mapper = mapper;
        }

        // GET: api/drinks
        [HttpGet]
        public IEnumerable<DrinkModel> Get()
        {
            return _mapper.Map<IEnumerable<DrinkModel>>(
                _repositoryDrink.Get().OrderBy(x => x.Entity.Name).ToArray());
        }
        
        // POST api/drinks
        [HttpPost]
        public IActionResult Post([FromForm]DrinkModel stackDrink)
        {
            if (stackDrink == null) throw new ArgumentNullException(nameof(stackDrink));

            _repositoryDrink.Add(new Drink(stackDrink.Name, stackDrink.Image, stackDrink.Volume, stackDrink.Price), stackDrink.Quantity);

           return Ok(_mapper.Map<IEnumerable<DrinkModel>>(
                _repositoryDrink.Get().OrderBy(x => x.Entity.Name).ToArray()));
        }

        // PUT api/drinks/5/17
        [HttpPut("{id}/price/{price}")]
        public ActionResult Put(int id, int price)
        {
            if (id <= 0)
                return BadRequest();

            var machineStackDrink = _repositoryDrink.Get(id);
            if (machineStackDrink==null) return NotFound();


            _repositoryDrink.ChangePrice(machineStackDrink.Entity, price);

            return Ok(_mapper.Map<DrinkModel>(machineStackDrink));
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

            return Ok(_mapper.Map<DrinkModel>(machineStackDrink));
        }
    }
}
