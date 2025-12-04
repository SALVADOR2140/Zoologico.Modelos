using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoologico.Modelos;

namespace Zoologico.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspeciesController : ControllerBase
    {
        private readonly ZoologicoAPIContext _context;

        public EspeciesController(ZoologicoAPIContext context)
        {
            _context = context;
        }

        // GET: api/Especies
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Especie>>>> GetEspecie()
        {
            try
            {
                var data = await _context.Especies.ToListAsync();
                return ApiResult<List<Especie>>.Ok(data);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Especie>>.Fail(ex.Message);
            }
        }
        // GET: api/Especies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Especie>>> GetEspecie(int id)
        {
            try
            {
                var especie = await _context
                    .Especies
                    .Include(e => e.Animales)
                    .FirstOrDefaultAsync(e => e.Codigo == id);

                if (especie == null)
                {
                    // Retorna un ApiResult indicando que no existe esa especie
                    return ApiResult<Especie>.Fail("Especie no encontrada");
                }

                // Todo bien, retorna Ok con la especie y sus animales relacionados
                return ApiResult<Especie>.Ok(especie);
            }
            catch (Exception ex)
            {
                // En caso de error inesperado, retornamos el mensaje en un ApiResult de fallo
                return ApiResult<Especie>.Fail($"Error al obtener la especie: {ex.Message}");
            }
        }

        // PUT: api/Especies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Especie>>> PutEspecie(int id, Especie especie)
        {
            if (id != especie.Codigo)
            {
                return ApiResult<Especie>.Fail("No coinciden los identificadores.");
            }

            _context.Entry(especie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                // Puedes devolver la especie actualizada o simplemente null
                return ApiResult<Especie>.Ok(especie);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Especies.Any(e => e.Codigo == id))
                {
                    return ApiResult<Especie>.Fail("Especie no encontrada.");
                }
                else
                {
                    return ApiResult<Especie>.Fail("Error de concurrencia al actualizar la especie.");
                }
            }
            catch (Exception ex)
            {
                return ApiResult<Especie>.Fail($"Error inesperado: {ex.Message}");
            }
        }

        // POST: api/Especies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult<Especie>>> PostEspecie(Especie especie)
        {
            try
            {
                _context.Especies.Add(especie);
                await _context.SaveChangesAsync();

                // Puedes regresar el CreatedAt con ApiResult también si quieres, para mantener el enlace REST:
                // return CreatedAtAction("GetEspecie", new { id = especie.Codigo }, ApiResult<Especie>.Ok(especie));

                return ApiResult<Especie>.Ok(especie);
            }
            catch (Exception ex)
            {
                return ApiResult<Especie>.Fail($"Error al crear la especie: {ex.Message}");
            }
        }

        // DELETE: api/Especies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<Especie>>> DeleteEspecie(int id)
        {
            try
            {
                var especie = await _context.Especies.FindAsync(id);
                if (especie == null)
                {
                    return ApiResult<Especie>.Fail("Especie no encontrada.");
                }

                _context.Especies.Remove(especie);
                await _context.SaveChangesAsync();

                // Puedes devolver la especie eliminada o solo Ok(null)
                return ApiResult<Especie>.Ok(especie);
            }
            catch (Exception ex)
            {
                return ApiResult<Especie>.Fail($"Error al eliminar la especie: {ex.Message}");
            }
        }

        private bool EspecieExists(int id)
        {
            return _context.Especies.Any(e => e.Codigo == id);
        }
    }
}
