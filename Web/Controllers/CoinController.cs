using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Web.Application;
using Web.Context;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/coins")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly RepositoryCoin _repositoryCoins;

        public CoinController(IApplicationContext db)
        {
            _repositoryCoins = new RepositoryCoin(db);

            foreach (EnumCoin par in Enum.GetValues(typeof(EnumCoin)))
                if (_repositoryCoins.Get(new Coin(par)) == null)
                _repositoryCoins.Add(new Coin(par), 0);
        }

        // GET: api/coins
        [HttpGet]
        public IEnumerable<MachineStack<Coin>> Get()
        {
            return _repositoryCoins.Get().OrderBy(x => x.Entity.Par);
        }

        // GET api/coins/5
        [HttpGet("{id}")]
        public MachineStack<Coin> Get(int id)
        {
            return _repositoryCoins.Get(id);
        }

        // PUT api/coins/5/17
        [HttpPut("{id}/{quantity}")]
        public ActionResult Put(int id, int quantity)
        {
            if (id <= 0)
                return BadRequest();

            var machineStackCoin = _repositoryCoins.Get(id);
            if (machineStackCoin == null) return NotFound();


            _repositoryCoins.Add(machineStackCoin.Entity, quantity);

            return Ok(machineStackCoin);
        }

        // PUT api/coins/5/true
        [HttpPut("{id}/blocking/{blocking}")]
        public ActionResult ChangeBlocking(int id, bool blocking)
        {
            if (id <= 0)
                return BadRequest();

            var machineStackCoin = _repositoryCoins.Get(id);
            if (machineStackCoin == null) return NotFound();


            _repositoryCoins.ChangeBlocking(machineStackCoin.Entity, blocking);

            return Ok(machineStackCoin);
        }

        // PUT api/coins/5
        [HttpPut("{id}/giveOut")]
        public ActionResult GiveOut(int id)
        {
            if (id <= 0)
                return BadRequest();

            var machineStackCoin = _repositoryCoins.Get(id);
            if (machineStackCoin == null) return NotFound();


            _repositoryCoins.GiveOut(machineStackCoin.Entity, 1);

            return Ok(machineStackCoin);
        }

        // POST api/coins/putDepositReturnBalance
        [HttpPost("putDepositReturnBalance/{balance}")]
        public ActionResult PutDepositReturnBalance([FromBody]MachineStackCoinModel[] model, [FromRoute]int balance)
        {
            if (balance <= 0) throw new ArgumentOutOfRangeException(nameof(balance));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (!model.Any()) return BadRequest();

            var returnedCoins = _repositoryCoins.ReturnBalance(balance);

            foreach (var item in model)
                if (item.Quantity>0)
                    if (_repositoryCoins.Get(new Coin(item.Par)).Entity.Blocking)
                        throw new Exception($"Монеты достоинством {item.Par} не принимаются.");

            foreach (var itemPut in model)
                if (itemPut.Quantity > 0)
                    _repositoryCoins.Add(new Coin(itemPut.Par), itemPut.Quantity);

            foreach (var itemReturn in returnedCoins)
                _repositoryCoins.Add(new Coin(itemReturn.Entity.Par), -itemReturn.Quantity);

            return Ok(returnedCoins.OrderBy(x => x.Entity.Par));
        }
    }
}
