using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

[Route("api/categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get(
        [FromServices]DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().ToListAsync(); //AsNoTracking tras de forma rapida os dados do banco, ganhar performace
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetId(
        int id,
        [FromServices]DataContext context)
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return Ok(category);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Post(
        [FromBody] Category model,
        [FromServices]DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Categories.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { message = $"Ocorreu um erro ao salvar categoria, detalhes: {ex}" });
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put(
        int id,
        [FromBody]Category model,
        [FromServices] DataContext context)
    {
        if (model.Id != id)
            return NotFound(new { message = "Categoria não encontrada"});

        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync(); //persiste no banco
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Este registro já foi atualizado." });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { message = $"Erro ao atualizar registro, detalhes: {ex}" });
        }
        
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Delete(
        int id,
        [FromServices]DataContext context
        )
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id); //recuperamos ela aqui do bancopara remover ela em baixo
        if (category == null)
            return NotFound(new { message = "Categoria não encontrada" });

        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new {message = $"Categoria {category.Title} removida com sucesso!" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { message = $"Não foi possivel remover a categoria, detalhes: {ex}" });
        }       
    }

}

