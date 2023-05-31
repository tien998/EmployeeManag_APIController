using System.Diagnostics;
using EmpManagement_APIController.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpManagement_APIController.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    //get
    [HttpGet]
    public async Task<ActionResult<List<Employee>>> GetAllAsync() => await EmployDb.Get();

    //get by id
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> Get(int id) => await EmployDb.Get(id);
    
    //post
    [HttpPost]  
    public IActionResult Create(Employee e)
    {
        EmployDb.Create(e);
        return CreatedAtAction("Get", new {id= e.ID}, e);
    }

    //put
    [HttpPut]
    public IActionResult Update(Employee e)
    {
        // e = await EmployDb.Get(id);
        EmployDb.Update(e);
        Debug.WriteLine($"Employee: {e.ID} : {e.Name} : {e.DateOfBirth}");
        return NoContent();
    }
    //delete
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        EmployDb.Delete(id);
        return NoContent();
    }
}