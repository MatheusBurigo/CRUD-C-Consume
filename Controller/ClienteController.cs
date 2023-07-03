using CrudUsuarios.Domain;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CrudUsuarios.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IMongoCollection<Cliente> _collection;

        public ClienteController(IMongoCollection<Cliente> collection)
        {
            _collection = collection;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> Get()
        {
            var clientes = _collection.Find(_ => true).ToList();
            return clientes;
        }

        [HttpPost]
        public ActionResult<Cliente> Create(Cliente cliente)
        {
            _collection.InsertOne(cliente);
            return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{name}")]
        public IActionResult Update(string name, Cliente updatedCliente)
        {
            var filter = Builders<Cliente>.Filter.Eq("Name", name);
            var update = Builders<Cliente>.Update
                .Set("Name", updatedCliente.Name)
                .Set("Email", updatedCliente.Email);

            var result = _collection.UpdateOne(filter, update);

            if (result.ModifiedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var filter = Builders<Cliente>.Filter.Eq("Name", name);
            var result = _collection.DeleteOne(filter);

            if (result.DeletedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
