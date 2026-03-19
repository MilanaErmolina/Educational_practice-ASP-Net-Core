using Microsoft.AspNetCore.Mvc;

namespace CalculatorWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        [HttpPost("calculate")]
        public IActionResult Calculate([FromBody] CalculationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Expression))
                return BadRequest(new { error = "Expression is required" });

            try
            {
                var result = new System.Data.DataTable().Compute(request.Expression, null);
                return Ok(new { result = result.ToString() });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Invalid expression", details = ex.Message });
            }
        }
    }

    public class CalculationRequest
    {
        public string Expression { get; set; }
    }
}