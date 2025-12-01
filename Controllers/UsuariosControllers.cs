using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Data;
using UsuariosAPI.Models;

namespace UsuariosAPI.Controllers
{
    [Route("api/[controller]")] // Ruta: /api/usuarios
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Where(u => u.Activo)
                .OrderByDescending(u => u.FechaCreacion)
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
            {
                return BadRequest(new { mensaje = "El email ya está registrado" });
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUsuario),
                new { id = usuario.Id },
                usuario
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest(new { mensaje = "ID no coincide" });
            }

            var existe = await _context.Usuarios.AnyAsync(u => u.Id == id);
            if (!existe)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar" });
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            usuario.Activo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}/permanente")]
        public async Task<IActionResult> DeleteUsuarioPermanente(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}