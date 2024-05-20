using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OPERACION_OMM.Models;

namespace OPERACION_OMM.Controllers
{
    //usar los cors
    [EnableCors("ReglasCors")]

    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly DBApiContext _context;

        public MovimientoController(DBApiContext context)
        {
            _context = context;
        }

        // GET: api/Movimiento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movimiento>>> GetMovimientos()
        {
            return await _context.Movimientos.ToListAsync();
        }

        // GET: api/Movimiento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movimiento>> GetMovimiento(DateTime id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);

            if (movimiento == null)
            {
                return NotFound();
            }

            return movimiento;
        }

        // PUT: api/Movimiento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Movimiento>> PutMovimiento(DateTime id, Movimiento movimiento)
        {
            if (id != movimiento.Fecha)
            {
                return BadRequest();
            }

            _context.Entry(movimiento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimientoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return await GetMovimiento(id);
        }

        // POST: api/Movimiento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movimiento>> PostMovimiento(Movimiento movimiento)
        {

            movimiento.Fecha = DateTime.Now;
            movimiento.Tipo = movimiento.Importe > 0 ? "D" : "A";

            _context.Movimientos.Add(movimiento);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MovimientoExists(movimiento.Fecha))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMovimiento", new { id = movimiento.Fecha }, movimiento);
        }

        // DELETE: api/Movimiento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimiento(DateTime id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }

            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimientoExists(DateTime id)
        {
            return _context.Movimientos.Any(e => e.Fecha == id);
        }

        // trnsacciones 
        [HttpGet]
        [Route("transferencia")]
        public async Task<ActionResult<IEnumerable<MovimientoSaldo>>> GetMovimientosSaldo()
        {
            var movimientos = await _context.Movimientos
                .GroupBy(m => m.NroCuenta)
                .Select(g => new MovimientoSaldo
                {
                    NroCuenta = g.Key,
                    Saldo = g.Sum(m => m.Importe)
                })
                .ToListAsync();

            return movimientos;
        }

        //obtener movimientos por cuenta
        // GET: api/Movimiento/lista/5
        //[HttpGet("/eso/{idCuenta}")]
        //[Route("lista")]
        //public async Task<ActionResult<Movimiento>> GetMovimientoCuenta(string idCuenta)
        //{
        //    var movimiento = await _context.Movimientos.FindAsync(idCuenta);

        //    if (movimiento == null)
        //    {
        //        return NotFound();
        //    }

        //    return movimiento;
        //}

        [HttpGet("lista/{nroCuenta}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMovimientosPorNroCuenta(string nroCuenta)
        {
            var movimientos = await _context.Movimientos
                .Where(m => m.NroCuenta == nroCuenta)
                .Include(m => m.oCuenta)  // Include the related Cuenta entity
                .ToListAsync();

            if (movimientos == null || movimientos.Count == 0)
            {
                return NotFound();
            }

            // Transform the results to exclude 'Movimientos' property from 'Cuenta'
            var result = movimientos.Select(m => new
            {
                m.Fecha,
                m.Tipo,
                m.Importe,
                m.NroCuenta,
                oCuenta = new
                {
                    m.oCuenta.NroCuenta,
                    m.oCuenta.Tipo,
                    m.oCuenta.Moneda,
                    m.oCuenta.Nombre,
                    m.oCuenta.Saldo
                }
            });

            return Ok(result);
        }


    }
}
