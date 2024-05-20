using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPERACION_OMM.Models;

namespace OPERACION_OMM.Controllers
{
    //usar los cors
    [EnableCors("ReglasCors")]

    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly DBApiContext _context;

        public CuentaController(DBApiContext context)
        {
            _context = context;
        }

        // GET: api/Cuentas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> GetCuenta()
        {
            return await _context.Cuenta.ToListAsync();
        }

        // GET: api/Cuentas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cuenta>> GetCuenta(string id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);

            if (cuenta == null)
            {
                return NotFound();
            }

            return cuenta;
        }

        // PUT: api/Cuentas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Cuenta>> PutCuenta(string id, Cuenta cuenta)
        {
            if (id != cuenta.NroCuenta)
            {
                return BadRequest();
            }

            _context.Entry(cuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return await GetCuenta(id);
        }

        // POST: api/Cuentas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cuenta>> PostCuenta(Cuenta cuenta)
        {
            _context.Cuenta.Add(cuenta);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CuentaExists(cuenta.NroCuenta))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCuenta", new { id = cuenta.NroCuenta }, cuenta);
        }

        // DELETE: api/Cuentas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(string id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            _context.Cuenta.Remove(cuenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CuentaExists(string id)
        {
            return _context.Cuenta.Any(e => e.NroCuenta == id);
        }
    }
}
